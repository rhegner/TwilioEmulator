import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Conference } from '../models/Conference';
import { Page } from '../models/Page';
import { objectToQueryString } from './utils';
import { ConferencesPage } from '../models/ConferencesPage';

export class GetConferencesOptions {
  Status?: string[];
  Page?: number;
  PageSize?: number;
}

@Injectable({
  providedIn: 'root'
})
export class ConferencesService {

  constructor(private http: HttpClient,
    @Inject('BACKEND_BASE_URL') private backendBaseUrl: string,
    @Inject('ACCOUNT_SID') private accountSid: string) { }

  public getConference(sid: string): Promise<Conference> {
    return this.http.get<Conference>(`${this.backendBaseUrl}2010-04-01/Accounts/${this.accountSid}/Conferences/${sid}.json`).toPromise();
  }

  public getConferences(options?: GetConferencesOptions): Promise<ConferencesPage> {
    // tslint:disable-next-line:max-line-length
    return this.http.get<ConferencesPage>(`${this.backendBaseUrl}2010-04-01/Accounts/${this.accountSid}/Conferences.json${objectToQueryString(options)}`).toPromise();
  }

}
