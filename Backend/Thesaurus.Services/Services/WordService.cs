using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Thesaurus.DAL;
using Thesaurus.DAL.Entities;
using Thesaurus.DAL.Interfaces;
using Thesaurus.Services.BusinessEntities;

namespace Thesaurus.Services
{

    /// <summary>
    /// Logical operations related Word entity
    /// </summary>
    public class WordService : IWordService
    {

        private readonly IMemoryCache _memoryCache;

        private readonly IMapper _mapper;
        readonly IDalSession _dalSession = null;
        readonly IWordRepository _wordRepository;

        private static readonly SemaphoreSlim GetUsersSemaphore = new SemaphoreSlim(1, 1);

        #region public methods
        public WordService(IDalSession dalSession, IWordRepository wordRepository, IMemoryCache memoryCache, IMapper mapper)
        {
            _wordRepository = wordRepository;
            _dalSession = dalSession;
            _memoryCache = memoryCache;
            _mapper = mapper;            
        }

        /// <summary>
        /// Add new word to Database
        /// </summary>
        /// <param name="entity">word</param>
        /// <returns></returns>
        public async Task<Word> AddAsync(Word entity)
        {
            try
            {
                var wordDTO = _mapper.Map<WordDTO>(entity);

                _ = await WordDataInsertValidationsAsync(entity).ConfigureAwait(false);

                _dalSession.UnitOfWork.Begin();

                //We could have done adding word and synonym both by using the Word repo only as by passing both word and synonym to Stored proc in one go.
                //But i choose to use both repositories to maintian singular responsiblity
                var newWordId = await _wordRepository.AddAsync(wordDTO).ConfigureAwait(false);

                _dalSession.UnitOfWork.Commit();

                entity.WordId = newWordId;

                if (newWordId > 0)
                {
                    //fetch freshly inserted object -- or may be we can just update the id in the input object and return that,
                    //but as synonmys are multiple items and we are inserting them in one go, so we can't update their id's
                    var result = await _wordRepository.GetByIdAsync(newWordId).ConfigureAwait(false);
                    entity = _mapper.Map<Word>(result);
                }
                else
                {
                    _dalSession.UnitOfWork.Rollback();
                    throw new HttpStatusException(HttpStatusCode.InternalServerError, $" Synonyms fortThe word {entity.Title} could not be saved.");
                }

                _ = await AddWordToCache(entity.Title).ConfigureAwait(false);
                _dalSession.UnitOfWork.Commit();
            }
            catch
            {
                _dalSession.UnitOfWork.Rollback();
                throw;
            }

            return entity;
        }

        /// <summary>
        /// Delete word by Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _ = await WordDataDeleteValidationsAsync(id);

                _dalSession.UnitOfWork.Begin();

                var numberOfRecords = await _wordRepository.DeleteAsync(id).ConfigureAwait(false);
                if (numberOfRecords == 0)
                {
                    _dalSession.UnitOfWork.Rollback();
                    throw new HttpStatusException(HttpStatusCode.NotFound, $"The word Id: {id} is not present.");
                }

                _dalSession.UnitOfWork.Commit();
                var res = await LoadCacheFromDB(true).ConfigureAwait(false);

                return true;
            }
            catch
            {
                _dalSession.UnitOfWork.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Get all words from DB along with synonym
        /// </summary>
        /// <returns></returns>
        public async Task<PagedOutput> GetAllAsync(int pageSize, int pageNum)
        {
            PagedOutput output;

            //Here we have breached the singular responsiblity principle to save the multiple DB calls (getting both word and synonym through word repository only
            //Because synonym can't exist without word so combining both. Although i have implemented the functionality in Synonym repository as well in case we want to maintain single responsiblity
            var result = await _wordRepository.GetAllAsync(pageSize, pageNum).ConfigureAwait(false);

            if (result != null && result.Any())
            {
                var words = _mapper.Map<List<WordDTO>, List<Word>>(result.ToList());

                int totalRecords = await _wordRepository.GetTotalWordsCountAsync().ConfigureAwait(false);

                output = new PagedOutput()
                {
                    TotalItems = totalRecords,
                    CurrentPage = pageNum,
                    TotalPages = (int)Math.Ceiling(totalRecords / (decimal)pageSize),
                    Words = words
                };
            }
            else
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, $"Word's not available.");
            }

            return output;
        }

        /// <summary>
        /// Get the word by Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<Word> GetByIdAsync(int id)
        {
            Word entity;

            if (id <= 0)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Word Id: {id} is not available."),
                    ReasonPhrase = "Word ID Not Found"
                };
                throw new HttpResponseException(resp);
            }
            var result = await _wordRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (result == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Word Id: {id} is not available."),
                    ReasonPhrase = "Word ID Not Found"
                };
                throw new HttpResponseException(resp);
            }

            entity = _mapper.Map<Word>(result);

            return entity;
        }

        /// <summary>
        /// get word by Title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<Word> GetWordByTitleAsync(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new HttpStatusException(HttpStatusCode.Conflict, "Please provide title.");
            }
            Word entity;

            var result = await _wordRepository.GetWordByTitleAsync(title).ConfigureAwait(false);
            if (result == null)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, $"Word: {title} is not available.");
            }
            entity = _mapper.Map<Word>(result);

            return entity;
        }

        /// <summary>
        /// Update word
        /// </summary>
        /// <param name="entity">word</param>
        /// <returns></returns>
        public async Task<Word> UpdateAsync(Word entity)
        {
            try
            {
                _ = await WordDataUpdateValidationsAsync(entity).ConfigureAwait(false);

                _dalSession.UnitOfWork.Begin();

                var wordDTO = _mapper.Map<WordDTO>(entity);

                var numberOfRecords = await _wordRepository.UpdateAsync(wordDTO).ConfigureAwait(false);
                if (numberOfRecords > 0)
                {
                    _dalSession.UnitOfWork.Commit();

                    var res = await LoadCacheFromDB(true).ConfigureAwait(false);
                    var freshUpdated = await _wordRepository.GetByIdAsync(entity.WordId);
                    entity = _mapper.Map<Word>(freshUpdated);
                    
                }
                else
                {
                    _dalSession.UnitOfWork.Rollback();
                    throw new HttpStatusException(HttpStatusCode.NotFound, $"Unable to update the word Id: {entity.WordId}.");
                }
            }
            catch
            {
                _dalSession.UnitOfWork.Rollback();
                throw;
            }
            return entity;
        }

        /// <summary>
        /// Returns all the titles
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetAllWordTitlesAsync()
        {
            var result = await _wordRepository.GetAllWordTitlesAsync().ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Fuzzy logic to get the strings from cache
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<string>> GetTitleSuggestions(string input)
        {
            if(string.IsNullOrEmpty(input))
            {
                return new List<string>();
            }
            var loaded = await LoadCacheFromDB(false).ConfigureAwait(false);
            var result = await Task.Run(() => ThesaurusCache.GetWordSuggestions(input));
            return result;
        }

        #endregion

        #region private methods
        private async Task<bool> WordDataInsertValidationsAsync(Word entity)
        {
            if (entity == null)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, $"The word data is empty.");
            }
            //Check for existing title
            var existingTitle = await _wordRepository.GetWordByTitleAsync(entity.Title).ConfigureAwait(false);
            if (existingTitle != null)
            {
                throw new HttpStatusException(HttpStatusCode.Conflict, $"The word {entity.Title} is already present.");
            }
            return true;
        }

        private async Task<bool> WordDataUpdateValidationsAsync(Word entity)
        {
            if (entity == null)
            {
                throw new HttpStatusException(HttpStatusCode.Conflict, "The word data is empty.");
            }
            else if (entity.WordId <= 0)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, $"Word Id: {entity.WordId} is wrong.");
            }

            //Check for existing ID
            var existingId = await _wordRepository.GetByIdAsync(entity.WordId).ConfigureAwait(false);
            if (existingId == null)
            {
                throw new HttpStatusException(HttpStatusCode.Conflict, $"The word Id: {entity.WordId} is not available.");
            }

            //Check for existing title
            var existingWord = await _wordRepository.GetWordByTitleAsync(entity.Title).ConfigureAwait(false);
            if (existingWord != null && existingWord.WordId != entity.WordId)
            {
                throw new HttpStatusException(HttpStatusCode.Conflict, $"The word: {entity.Title} is already present.");
            }

            return true;
        }

        private async Task<bool> WordDataDeleteValidationsAsync(int id)
        {
            //Chek for existing
            var existing = await _wordRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (existing == null)
            {
                throw new HttpStatusException(HttpStatusCode.Conflict, $"The word Id: {id} is not available.");
            }
            return true;
        }

        private async Task<bool> LoadCacheFromDB(bool forceRefresh)
        {
            var result = await Task.Run(async () =>
            {
                if (forceRefresh || !_memoryCache.TryGetValue("WordsLoaded", out bool WordsLoaded))
                {
                    try
                    {
                        //Lock before load the cache from DB
                        await GetUsersSemaphore.WaitAsync();

                        //Double check in case of 2 requests try to load the cache 
                        if (forceRefresh || !_memoryCache.TryGetValue("WordsLoaded", out bool WordsLoadedAgain))
                        {
                            var words = await _wordRepository.GetAllWordTitlesAsync().ConfigureAwait(false); ;
                            ThesaurusCache.LoadDictonaryFromDB(words, forceRefresh);
                            _memoryCache.Set("WordsLoaded", true, new MemoryCacheEntryOptions() { Priority = CacheItemPriority.NeverRemove });
                        }
                    }
                    finally
                    {
                        GetUsersSemaphore.Release();
                    }
                }
                return true;
            });

            return result;
        }

        private async Task<bool> AddWordToCache(string word)
        {
            var result = await Task.Run(async () =>
            {
                 //if cache dictonary is loaded then get the word from cache
                if (_memoryCache.TryGetValue("WordsLoaded", out bool WordsLoaded))
                {
                    try
                    {
                        //Lock before load the cache from DB
                        await GetUsersSemaphore.WaitAsync();
                        ThesaurusCache.AddWordToTextDictionary(word, 11);
                    }
                    finally
                    {
                        GetUsersSemaphore.Release();
                    }
                }
                else // First create new cache dictionary then add the word to it
                {
                    try
                    {
                       var res = await LoadCacheFromDB(false).ConfigureAwait(false);

                        if (res)
                        {
                            //Lock before load the cache from DB
                            await GetUsersSemaphore.WaitAsync();
                            ThesaurusCache.AddWordToTextDictionary(word, 11);
                        }
                    }
                    finally
                    {
                        GetUsersSemaphore.Release();
                    }
                    
                }
                return true;
            });

            return result;
        }
        #endregion
    }
}

