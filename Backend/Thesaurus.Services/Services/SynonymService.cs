using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Thesaurus.DAL;
using Thesaurus.DAL.Entities;
using Thesaurus.DAL.Interfaces;
using Thesaurus.Services.BusinessEntities;

namespace Thesaurus.Services
{
    /// <summary>
    ///  operations related Synonym entity
    /// </summary>
    public class SynonymService : ISynonymService
    {

        private readonly IMapper _mapper;
        readonly IDalSession _dalSession = null;
        readonly ISynonymRepository _synonymRepository;


        public SynonymService(IDalSession dalSession,  ISynonymRepository synonymRepository, IMapper mapper)
        {
            _synonymRepository = synonymRepository;
            _dalSession = dalSession;
            _mapper = mapper;
        }

        #region Public Methods
        /// <summary>
        /// Add new Synonym to Database
        /// </summary>
        /// <param name="entity">Synonym</param>
        /// <returns></returns>
        public async Task<Synonym> AddAsync(Synonym entity)
        {        
            try
            {
                _ =  SynonymDataAddValidations(entity);

                _dalSession.UnitOfWork.Begin();

                var SynonymDTO = _mapper.Map<SynonymDTO>(entity);

                var newId = await _synonymRepository.AddAsync(SynonymDTO).ConfigureAwait(false);
                
                if (newId > 0)
                {
                    _dalSession.UnitOfWork.Commit();
                    //fetch freshly inserted object -- or may be we can just update the id in the input object and return that,
                    var result = await _synonymRepository.GetByIdAsync(newId).ConfigureAwait(false);                  

                    entity = _mapper.Map<Synonym>(result);
                }
                else
                {
                    _dalSession.UnitOfWork.Rollback();
                    throw new HttpStatusException(HttpStatusCode.InternalServerError, $"Unable to add synonym.");
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
        /// Add multiple synonyms
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>true if added succesfully</returns>
        public async Task<bool> AddRangeAsync(List<Synonym> entities)
        {         
            try
            {
                _ = SynonymDataAddRangeValidations(entities);

                _dalSession.UnitOfWork.Begin();

                var synonymDTOList = _mapper.Map<List<Synonym>, List<SynonymDTO>>(entities);

                var numberOfRecords = await _synonymRepository.AddRangeAsync(synonymDTOList).ConfigureAwait(false);

                _dalSession.UnitOfWork.Commit();

                return true;
            }
            catch
            {
                _dalSession.UnitOfWork.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Delete all synonym's of a word
        /// </summary>
        /// <param name="wordId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAllByWordIdAsync(int wordId)
        {            
            try
            {
                if (wordId <= 0)
                {
                    throw new HttpStatusException(HttpStatusCode.BadRequest, $"Word Id: {wordId} for delete is not correct.");
                }
                _dalSession.UnitOfWork.Begin();

                var numberOfRecords = await _synonymRepository.DeleteAllByWordIdAsync(wordId);
                if (numberOfRecords == 0)
                {
                    _dalSession.UnitOfWork.Rollback();
                    throw new HttpStatusException(HttpStatusCode.NotFound, $"Unable to delete the synonym for word Id: {wordId}.");
                }

                _dalSession.UnitOfWork.Commit();

                return true;
            }
            catch
            {
                _dalSession.UnitOfWork.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Delete Synonym by Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _ =  SynonymDataDeleteValidationsAsync(id);

                _dalSession.UnitOfWork.Begin();

                var numberOfRecords = await _synonymRepository.DeleteAsync(id);
                if (numberOfRecords == 0)
                {
                    _dalSession.UnitOfWork.Rollback();
                    throw new HttpStatusException(HttpStatusCode.NotFound, $"The Synonym Id: {id} is not present.");
                }

                _dalSession.UnitOfWork.Commit();
                return true;
            }
            catch
            {
                _dalSession.UnitOfWork.Rollback();
                throw;
            }
        }


        /// <summary>
        /// Get the synonym by Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<Synonym> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, $"Synonym Id {id} is not present.");
            }
            Synonym entity;
            var synonym = await _synonymRepository.GetByIdAsync(id);
            if (synonym == null)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, $"Synonym Id {id} is not present.");
            }

            entity = _mapper.Map<Synonym>(synonym);

            return entity;
        }

        /// <summary>
        /// Update Synonym
        /// </summary>
        /// <param name="entity">Synonym</param>
        /// <returns></returns>
        public async Task<Synonym> UpdateAsync(Synonym entity)
        {          
            try
            {
                _ = SynonymDataUpdateValidations(entity);

                _dalSession.UnitOfWork.Begin();

                var synonymDTO = _mapper.Map<SynonymDTO>(entity);

                var numberOfRecords = await _synonymRepository.UpdateAsync(synonymDTO);
               
                if (numberOfRecords > 0)
                {
                    _dalSession.UnitOfWork.Commit();
                    var freshUpdated = await _synonymRepository.GetByIdAsync(entity.SynonymId);
                    entity = _mapper.Map<Synonym>(freshUpdated);                  
                }
                else
                {
                    _dalSession.UnitOfWork.Rollback();
                    throw new HttpStatusException(HttpStatusCode.NotFound, $"Unable to update the synonym: {entity.Title}.");
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
        /// update list of synonyms
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateRangeAsync(List<Synonym> entities)
        {
            try
            {
                entities.ForEach(entity =>
                {
                    _ = SynonymDataUpdateValidations(entity);
                });

                _dalSession.UnitOfWork.Begin();


                var synonymDTOList = _mapper.Map<List<Synonym>, List<SynonymDTO>>(entities);

                var numberOfRecords = await _synonymRepository.UpdateRangeAsync(synonymDTOList);

                if (numberOfRecords <= 0)
                {
                    _dalSession.UnitOfWork.Rollback();
                    throw new HttpStatusException(HttpStatusCode.NotFound, $"Unable to update the synonyms.");
                }

                _dalSession.UnitOfWork.Commit();

                return true;
            }

            catch
            {
                _dalSession.UnitOfWork.Rollback();
                throw;
            }

        }

        #endregion

        #region Private Members
        private bool SynonymDataAddValidations(Synonym entity)
        {
            if (entity == null)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, $"The synonym data is empty.");
            }
            else if (entity.WordId <= 0)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, $"Synonym: {entity.Title} dont have word Id.");
            }

            return true;
        }

        private bool SynonymDataAddRangeValidations(List<Synonym> entities)
        {
            if (entities == null || !entities.Any())
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, $"The synonym data is empty.");
            }
            else if (!entities.All(e => e.WordId > 0))
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, $"one or more synonym's dont have word Id.");
            }

            return true;
        }

        private bool SynonymDataUpdateValidations(Synonym entity)
        {
            if (entity.WordId <= 0)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, $"Word Id: {entity.WordId} is wrong.");
            }
            else if(entity.SynonymId <= 0)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, $"Synonym Id: { entity.SynonymId } is wrong.");
            }
            return true;
        }

        private bool SynonymDataDeleteValidationsAsync(int id)
        {
            if (id <= 0)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, $"Synonym Id: {id} is wrong.");
            }
            return true;
        }

        #endregion
    }
}

