class Org {
  //constructor(type: string) {
  //  this.name = type;
  //  this.inputText = input;
  //}
  id: string = '';
  name: string = '';
  isDefault: boolean;
  readings: ReadingInfo[];
  students: Student[];
  tags: Tag[];
}

class ReadingInfo {
  //constructor(type: string) {
  //  this.name = type;
  //  this.inputText = input;
  //}
  id: string = '';
  title: string = '';
  text;
  newWords: number;
  knownWords: number;
  inProgressWords: number;
}

class Student {
  //constructor(type: string) {
  //  this.name = type;
  //  this.inputText = input;
  //}
  id: string = '';
  firstName: string = '';
  lastName: string = '';
  email: string = '';
  tags: Tag[] = [];
  knownWordsCount: number;
  inProgressWordsCount: number;
  newWordsCount: number;
  newReadingsCount: number;
}

class Tag {
  //constructor(type: string) {
  //  this.name = type;
  //  this.inputText = input;
  //}
  id: string = '';
  value: string = '';
  isDefault: boolean = false;
}

class WordAttemptSummary {
  wordEntryId: string = '';
  word: string = '';
  spellingFailures: string = '';
  spellingAttempts: string = '';
  meaningFailures: string = '';
  meaningAttempts: string = '';
  clozeFailures: string = '';
  clozeAttempts: string = '';
}

export {
  Org,
  ReadingInfo,
  Student,
  Tag,
  WordAttemptSummary
};
