import { Synonym } from './synonym.model';

export class Word {
  title: string;
  wordId!: number;
  description!: string;
  synonyms: Synonym[] = new Array();
  synonymstring!: string;

  constructor(
    wordId: number,
    title: string,
    description: string,
    synonymstring: string
  ) {
    this.wordId = wordId;
    this.title = title;
    this.description = description;
    this.synonymstring = synonymstring;
    if (!(synonymstring === null || synonymstring.match(/^ *$/) !== null)) {
      synonymstring
        .split(',')
        .forEach((x) => this.synonyms.push(new Synonym(0, 0, x)));
    }
  }
}
