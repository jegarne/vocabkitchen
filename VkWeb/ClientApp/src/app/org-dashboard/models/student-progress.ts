import { Tag } from '@models/org'

class StudentProgress {
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
  totalAttempts: number;
  spellingFailedAttempts: number;
  clozeFailedAttempts: number;
  meaningFailedAttempts: number;
  words: WordProgress[] = [];
}

class WordProgress {
  id: string;
  annotationId: string;
  word: string = '';
  isKnown: boolean;
  isInProgress: boolean;
  isNew: boolean;
  isKnownDate: string;
  totalAttempts: number;
  spellingFailedAttempts: number;
  clozeFailedAttempts: number;
  meaningFailedAttempts: number;
  status: string;
}

export {
  StudentProgress,
  WordProgress
};
