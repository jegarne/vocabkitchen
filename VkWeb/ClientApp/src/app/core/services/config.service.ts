import { Injectable, Inject } from '@angular/core';

@Injectable()
export class ConfigService {

  _apiURI: string;

  constructor(@Inject('BASE_URL') baseUrl: string) {
    this._apiURI = baseUrl + 'api';
  }

  getApiURI() {
    return this._apiURI;
  }
}
