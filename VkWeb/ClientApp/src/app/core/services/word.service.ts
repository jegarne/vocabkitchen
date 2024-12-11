import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from '@services/config.service';
import { Observable } from 'rxjs/Rx';
import { SuggestedDefinition, DefinitionSource } from '@models/reading';

@Injectable()
export class WordService {
  baseUrl: string = '';

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.baseUrl = configService.getApiURI();
  }

  getDefinitions(word: string): Observable<SuggestedDefinition[]> {
    return this.http.get<SuggestedDefinition[]>(this.baseUrl + `/word/${word}`);
  }

  deleteDefinition(annotationId: string): Observable<SuggestedDefinition> {
    return this.http.delete<SuggestedDefinition>(this.baseUrl + `/word/definition/${annotationId}`);
  }

  getDefinitionSources(): Observable<DefinitionSource[]> {
    return this.http.get<DefinitionSource[]>(this.baseUrl + `/word/sources`);
  }
}
