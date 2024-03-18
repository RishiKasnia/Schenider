import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThesaurusService } from '../../Services/thesaurus.service';
import { FormControl } from '@angular/forms';
import {
  debounceTime,
  tap,
  switchMap,
  finalize,
  startWith,
  distinctUntilChanged,
} from 'rxjs/operators';
import { Word } from '../../Models/word.model';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { wordComponent } from '../word/word.component';
import { UpdateWordComponent } from '../update-word/update-word.component';
import { MatInputModule } from '@angular/material/input';
import { ThesaurusConstants } from '../../Services/constants';

@Component({
  selector: 'app-search',
  standalone: true,
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
  imports: [
    MatInputModule,
    MatAutocompleteModule,
    MatFormFieldModule,
    wordComponent,
    UpdateWordComponent,
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
  ],
})
export class SearchComponent implements OnInit {
  @Output() UpdatedWordEvent = new EventEmitter<Word>();
  @Output() DeletedWordEvent = new EventEmitter<number>();

  searchWordsCtrl = new FormControl();
  filteredWords: any;
  isLoading = false;
  errorMsg!: string;
  searchWord!: Word;
  showUpdateControl: boolean;
  isWordDeleted: boolean;

  constructor(private thesaurusService: ThesaurusService) {
    this.showUpdateControl = false;
    this.isWordDeleted = false;
  }

  ngOnInit(): void {
    this.searchWordsCtrl.valueChanges
      .pipe(
        startWith(' '),
        debounceTime(500),
        distinctUntilChanged(),
        tap(() => {
          this.errorMsg = '';
          this.filteredWords = [];
          this.isLoading = true;
        }),
        switchMap((value) =>
          this.thesaurusService
            .getWordSuggestions(
              ThesaurusConstants.BASEURL_WORD + '/Suggestions?title=' + value
            )
            .pipe(
              finalize(() => {
                this.isLoading = false;
              })
            )
        )
      )
      .subscribe((data) => {
        if (data == undefined) {
          this.errorMsg = data['Error'];
          this.filteredWords = [];
        } else {
          this.errorMsg = '';
          this.filteredWords = data;
        }
      });
  }
  showUpdate() {
    this.showUpdateControl = true;
    this.filteredWords = [];
    return false;
  }
  getWord(title: string) {
    let url = ThesaurusConstants.BASEURL_WORD + '/title?title=' + title;
    this.thesaurusService.getWordByTtile(`${url}`).subscribe((data) => {
      this.searchWord = data as Word;
      this.isWordDeleted = false;
    });
  }

  UpdatedWordValues(value: Word) {
    this.searchWord = value;
    this.showUpdateControl = false;
    this.UpdatedWordEvent.emit(value);
  }

  RemovedWordValue(value: number) {
    this.searchWord = {} as Word;
    this.isWordDeleted = true;
    this.showUpdateControl = false;
    this.DeletedWordEvent.emit(value);
  }
}
