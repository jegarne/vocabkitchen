"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var StudentWord = /** @class */ (function () {
    function StudentWord(id, annotationId, word, definition, sentences) {
        this.word = '';
        this.definition = '';
        this.id = id;
        this.annotationId = annotationId;
        this.word = word;
        this.definition = definition;
        this.sentences = sentences;
    }
    return StudentWord;
}());
exports.StudentWord = StudentWord;
var WordAttempt = /** @class */ (function () {
    function WordAttempt() {
    }
    return WordAttempt;
}());
exports.WordAttempt = WordAttempt;
//# sourceMappingURL=student-word.js.map