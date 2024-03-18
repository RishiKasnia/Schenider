import {
  Component,
  HostBinding,
  Input,
  OnInit,
  Output,
  EventEmitter,
  Inject,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Word } from '../../Models/word.model';
import { ThesaurusService } from '../../Services/thesaurus.service';

@Component({
  selector: 'app-word',
  standalone: true,
  templateUrl: './word.component.html',
  styleUrls: ['./word.component.css'],
  imports: [CommonModule],
})
export class wordComponent implements OnInit {
  @Input() wordModel!: Word;
  @HostBinding('attr.class') cssClass = 'row';
  @Output() RemovedWordValues = new EventEmitter<number>();

  constructor(
    @Inject(ThesaurusService) private _thesurusApiService: ThesaurusService
  ) {}

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

  ngOnInit(): void {}
  deleteWord(): boolean {
    this.deleteWordFromBackend();
    return false;
  }

  deleteWordFromBackend() {
    this._thesurusApiService
      .deleteWordById(
        'http://localhost:5000/api/v1/Word?id=' + this.wordModel.wordId
      )
      .subscribe((data) => {
        this.RemovedWordValues.emit(this.wordModel.wordId);
      });
  }
}
