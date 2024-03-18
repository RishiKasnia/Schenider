import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Word } from '../Models/word.model';
import { Synonym } from '../Models/synonym.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ThesaurusService {
  constructor(private httpService: HttpClient) {}

  public getAllWords = (route: string): Observable<any> => {
    return this.httpService.get(route);
  };

  addWord(newWord: Word, route: string): Observable<any> {
    const headers = { 'content-type': 'application/json' };
    const body = JSON.stringify(newWord);
    return this.httpService.post(route, body, { headers: headers });
  }

  updateWord(updatedWord: Word, route: string): Observable<any> {
    const headers = { 'content-type': 'application/json' };
    const body = JSON.stringify(updatedWord);
    return this.httpService.put(route, body, { headers: headers });
  }

  addSynonyms(newSynonym: Synonym[], route: string): Observable<any> {
    const headers = { 'content-type': 'application/json' };
    const body = JSON.stringify(newSynonym);
    return this.httpService.post(route, body, { headers: headers });
  }

  updateSynonyms(updatedSynonym: Synonym[], route: string): Observable<any> {
    const headers = { 'content-type': 'application/json' };
    const body = JSON.stringify(updatedSynonym);
    return this.httpService.put(route, body, { headers: headers });
  }

  public getWordByTtile = (route: string) => {
    return this.httpService.get(route);
  };

  public getWordSuggestions = (route: string) => {
    return this.httpService.get(route);
  };

  public deleteWordById = (route: string) => {
    return this.httpService.delete(route);
  };
}
