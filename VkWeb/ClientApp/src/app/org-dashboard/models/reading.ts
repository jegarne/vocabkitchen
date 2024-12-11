import { Tag } from '@models/org';

class Reading {
  constructor(
    orgId: string,
    title: string,
    text: string,
  ) {
    this.orgId = orgId;
    this.title = title;
    this.text = text;
  }
  id: string;
  orgId: string;
  title: string = '';
  text: string;
  contentItems: ContentItem[];
  tags: Tag[];
  newWords: number;
  knownWords: number;
  inProgressWords: number;
}

class ContentItem {
  constructor(
    index: number,
    wordId: string,
    value: string,
    definition: string
  ) {
    this.index = index;
    this.wordId = wordId;
    this.value = value;
    this.definition = definition;
  }
  id: string;
  index: number;
  wordId: string;
  value: string = '';
  definition: string;
  annotationId: string;
  annotationContextId: string;
  definitionUsedByWordsCount: number;
  definitionUsedByStudentsCount: number;
}

class Definition {
  constructor(
    readingId: string,
    contentItemStartIndex: number,
    contentItemEndIndex: number,
    start: number,
    end: number,
    word: string
  ) {
    this.readingId = readingId;
    this.contentItemStartIndex = contentItemStartIndex;
    this.contentItemEndIndex = contentItemEndIndex;
    this.start = start;
    this.end = end;
    this.word = word;
  }
  readingId: string;
  contentItemStartIndex: number;
  contentItemEndIndex: number;
  start: number;
  end: number;
  word: string = '';
  partOfSpeech: string;
  definition: string;
  source: string;
  annotationId: string;
}

class SuggestedDefinition {
  partOfSpeech: string;
  source: string;
  imageUrl: string;
  value: string = '';
  annotationId: string;
  lastUpdateDate: string;
  isEditable: boolean;
  isMind: boolean;
}

class DefinitionSource {
  code: string = '';
  displayName: string;
  attributionText: string;
}

export {
  Reading,
  ContentItem,
  Definition,
  SuggestedDefinition,
  DefinitionSource
};
