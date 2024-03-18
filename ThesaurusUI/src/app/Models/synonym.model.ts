export class Synonym {
  synonymId!: number;
  title: string;
  wordId!: number;

  constructor(synonymId: number, wordId: number, title: string) {
    this.synonymId = synonymId;
    this.wordId = wordId;
    this.title = title;
  }
}
