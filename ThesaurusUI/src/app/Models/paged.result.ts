import { Word } from './word.model';

export class pagedmodel {
  totalitems!: number;
  totalpages!: number;
  currentPage!: number;
  Words: Word[] = new Array();

  constructor(
    totalitems: number,
    totalpages: number,
    currentPage: number,
    Words: Word[]
  ) {
    this.totalitems = totalitems;
    this.totalitems = totalitems;
    this.currentPage = currentPage;
    this.Words = Words;
  }
}
