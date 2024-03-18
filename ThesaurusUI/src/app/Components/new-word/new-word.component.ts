import { Component, OnInit, Output, EventEmitter, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Word } from '../../Models/word.model';
import { ThesaurusService } from '../../Services/thesaurus.service';
import { CommonModule } from '@angular/common';
import { ThesaurusConstants } from '../../Services/constants';

@Component({
  selector: 'app-new-word',
  standalone: true,
  templateUrl: './new-word.component.html',
  styleUrls: ['./new-word.component.css'],
  imports: [FormsModule, CommonModule],
})
export class NewWordComponent implements OnInit {
  @Output() NewWordValues = new EventEmitter<Word>();

  constructor(
    @Inject(ThesaurusService) private _thesurusApiService: ThesaurusService
  ) {}
  model = new Word(0, '', '', '');
  submitted = false;

  onSubmit() {
    this.submitted = true;
  }

  addWord(): boolean {
    var newword = new Word(
      0,
      this.model.title,
      this.model.description,
      this.model.synonymstring
    );
    this.addtoBackend(newword);
    return false;
  }

  addNewWordMethod(addneword: any) {
    this.addWord();
  }

  resetModel(): boolean {
    this.model.title = '';
    this.model.synonymstring = '';
    this.model.synonyms = new Array();
    this.model.description = '';
    return false;
  }

  addtoBackend(newword: Word) {
    this._thesurusApiService
      .addWord(newword, ThesaurusConstants.BASEURL_WORD)
      .subscribe((data) => {
        alert('New word added!');
        this.NewWordValues.emit(newword);
        this.resetModel();
      });
  }

  ngOnInit(): void {}
}
