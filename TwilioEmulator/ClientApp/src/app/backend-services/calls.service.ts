import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Call } from '../models/Call';
import { CallsPage } from '../models/CallsPage';
import { objectToQueryString } from './utils';

export class GetCallsOptions {
  To?: string;
  From?: string;
  ParentCallSid?: string;
  Status?: string[];
  Direction?: string[];
  StartTime?: Date;
  // StartTimeBefore?: Date;
  // StartTimeAfter?: Date;
  EndTime?: Date;
  // EndTimeBefore?: Date;
  // EndTimeAfter?: Date;
  Page?: number;
  PageSize?: number;
  PageToken?: string;
}

@Injectable({
  providedIn: 'root'
})
export class CallsService {

  constructor(private http: HttpClient,
    @Inject('BACKEND_BASE_URL') private backendBaseUrl: string,
    @Inject('ACCOUNT_SID') private accountSid: string) { }

  createCall(from: string, method: string, to: string, url: string): Promise<Call> {
    const payload = new HttpParams()
      .set('from', from)
      .set('method', method)
      .set('to', to)
      .set('url', url);
    return this.http.post<Call>(`${this.backendBaseUrl}2010-04-01/Accounts/${this.accountSid}/Calls.json`, payload).toPromise();
  }

  public createIncomingCall(from: string, to: string, url: string, httpMethod: string): Promise<Call> {
    const payload = new HttpParams()
      .set('from', from)
      .set('to', to)
      .set('url', url)
      .set('httpMethod', httpMethod);
    return this.http.post<Call>(this.backendBaseUrl + 'api/Calls/Incoming', payload).toPromise();
  }

  getCall(callSid: string): Promise<Call> {
    return this.http.get<Call>(`${this.backendBaseUrl}2010-04-01/Accounts/${this.accountSid}/Calls/${callSid}.json`).toPromise();
  }

  getCalls(options?: GetCallsOptions): Promise<CallsPage> {
    // tslint:disable-next-line:max-line-length
    return this.http.get<CallsPage>(`${this.backendBaseUrl}2010-04-01/Accounts/${this.accountSid}/Calls.json${objectToQueryString(options)}`).toPromise();
  }
}
