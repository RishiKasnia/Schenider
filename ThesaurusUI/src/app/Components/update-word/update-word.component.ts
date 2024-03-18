import {
  Component,
  OnInit,
  Output,
  EventEmitter,
  Input,
  Inject,
} from '@angular/core';
import { Word } from '../../Models/word.model';
import { Synonym } from '../../Models/synonym.model';
import { ThesaurusService } from '../../Services/thesaurus.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ThesaurusConstants } from '../../Services/constants';

@Component({
  selector: 'app-update-word',
  templateUrl: './update-word.component.html',
  styleUrls: ['./update-word.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule],
})
export class UpdateWordComponent implements OnInit {
  @Output() UpdatedWordValues = new EventEmitter<Word>();
  @Input() wordModel!: Word;

  synonymForUpdate: Synonym[] = new Array();
  synonymForInsert: Synonym[] = new Array();

  constructor(
    @Inject(ThesaurusService) private _thesurusApiService: ThesaurusService
  ) {}

  updateWord(
    title: HTMLInputElement,
    description: HTMLInputElement,
    synonym: HTMLInputElement
  ): boolean {
    this.synonymForUpdate = new Array();
    this.synonymForInsert = new Array();

    var inputSynonyms = synonym.value.split(',');

    inputSynonyms.forEach((x, i) => {
      if (this.wordModel.synonyms.length > i) {
        if (this.wordModel.synonyms[i].title !== x) {
          //update synonym
          this.synonymForUpdate.push(this.wordModel.synonyms[i]);
          this.wordModel.synonyms[i].title = x;
        }
      } else {
        //added new synonym
        var newSynonym = new Synonym(0, this.wordModel.wordId, x);

        this.synonymForInsert.push(newSynonym);
        this.wordModel.synonyms.push(newSynonym);
      }
    });

    if (this.synonymForUpdate.length > 0) {
      this.updateSynonyms();
    } else if (this.synonymForInsert.length > 0) {
      this.addSynonyms();
    } else {
      this.updateWordToBackend();
    }
    this.resetFields(title, description, synonym);
    return false;
  }

  resetFields(
    title: HTMLInputElement,
    description: HTMLInputElement,
    synonym: HTMLInputElement
  ): boolean {
    title.value = '';
    synonym.value = '';
    description.value = '';

    return false;
  }

  updateSynonyms() {
    this._thesurusApiService
      .updateSynonyms(
        this.synonymForUpdate,
        ThesaurusConstants.BASEURL_SYNONYM + '/synonyms'
      )
      .subscribe((data) => {
        if (this.synonymForInsert.length > 0) {
          this.addSynonyms();
        } else {
          this.updateWordToBackend();
        }
      });
  }

  addSynonyms() {
    this._thesurusApiService
      .addSynonyms(
        this.synonymForInsert,
        'http://localhost:5000/api/v1/Synonym/synonyms'
      )
      .subscribe((data) => {
        this.updateWordToBackend();
      });
  }

  updateWordToBackend() {
    this._thesurusApiService
      .updateWord(this.wordModel, 'http://localhost:5000/api/v1/Word')
      .subscribe((data) => {
        this.UpdatedWordValues.emit(data as Word);
      });
  }

  getSynonymString(synonyms: Synonym[]): string {
    var joinedString = synonyms.map((e) => e.title).join();
    return joinedString;
  }
  ngOnInit(): void {}
}
