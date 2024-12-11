class ProfilerRequest {
  constructor(type: string, input: string) {
    this.profilerType = type;
    this.inputText = input;
  }
  profilerType: string;
  inputText: string;
  totalWordCount: number;
  paragraphHtml: string;
  tableResult: ProfilerTableResult[];
}

interface ProfilerTableResult {
  percentage: number;
  columnHtml: string;
}

export {
  ProfilerRequest
};
