import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Word } from './Models/word.model';
import { ThesaurusService } from './Services/thesaurus.service';
import { pagedmodel } from './Models/paged.result';
import { wordComponent } from './Components/word/word.component';
import { SearchComponent } from './Components/search/search.component';
import { NewWordComponent } from './Components/new-word/new-word.component';
import { MatTabsModule } from '@angular/material/tabs';
import { CommonModule } from '@angular/common';
import { ThesaurusConstants } from './Services/constants';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    wordComponent,
    SearchComponent,
    NewWordComponent,
    MatTabsModule,
    CommonModule,
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css', '../styles.scss'],
})
export class AppComponent {
  WordsFromApi: Word[] = new Array();
  pagedResult!: pagedmodel;

  AddNewWordValues(value: Word) {
    this.WordsFromApi.push(value);
  }
  UpdateWordValues(updatedWord: Word) {
    this.WordsFromApi.forEach((value, index) => {
      if (value.wordId == updatedWord.wordId) {
        value.title = updatedWord.title;
        value.description = updatedWord.description;
        value.synonyms = updatedWord.synonyms;
      }
    });
  }

  RemovedWordValue(deletedId: number) {
    this.WordsFromApi.forEach((value, index) => {
      if (value.wordId == deletedId) this.WordsFromApi.splice(index, deletedId);
    });
  }
  constructor(private _thesurusApiService: ThesaurusService) {}

  ngOnInit(): void {
    this._thesurusApiService
      .getAllWords(
        ThesaurusConstants.BASEURL_WORD + '/words?pageSize=100&pageNum=1'
      )

      .subscribe({
        next: (result) => {
          console.log(result);
          this.pagedResult = new pagedmodel(
            result['totalItem'],
            result['totalPages'],
            result['currentPage'],
            result['words']
          );

          this.WordsFromApi = this.pagedResult.Words;
        },
        error: (error) => {
          console.error(error);
        },
      });
  }
  sortedArray(): Word[] {
    return this.WordsFromApi;
  }
}
