class Edit {
  constructor(
    index: number,
    type: string,
    start: number,
    end: number,
    value: string
  ) {
    this.index = index;
    this.type = type;
    this.start = start;
    this.end = end;
    this.value = value;
  }
  index: number;
  type: string = '';
  start: number;
  end: number;
  value: string = '';
}

export {
  Edit
};
