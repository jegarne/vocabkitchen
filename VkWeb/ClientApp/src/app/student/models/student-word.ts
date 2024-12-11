class StudentWord {
  constructor(
    id: string,
    annotationId: string,
    word: string,
    definition: string,
    sentences: string[]
    ) {
    this.id = id;
    this.annotationId = annotationId;
    this.word = word;
    this.definition = definition;
    this.sentences = sentences;
  }
  id: string;
  annotationId: string;
  isKnown: boolean;
  word: string = '';
  definition: string = '';
  sentences: string[];
  attempts: WordAttempt[];
}

class WordAttempt {
  id: string;
  attemptDate: string;
  attemptType: string;
  wasSuccessful: boolean;
}

export {
  StudentWord,
  WordAttempt
};
