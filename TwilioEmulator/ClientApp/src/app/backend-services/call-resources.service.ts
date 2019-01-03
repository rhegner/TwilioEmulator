import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CallResource } from '../models/CallResource';
import { Page } from '../models/Page';
import { objectToQueryString } from './utils';
import { ApiCall } from '../models/ApiCall';
import { ActivityLog } from '../models/ActivityLog';

export class GetCallResourcesOptions {
  directionFilter?: string[];
  statusFilter?: string[];
  page?: number;
  pageSize?: number;
}

@Injectable({
  providedIn: 'root'
})
export class CallResourcesService {

  constructor(private http: HttpClient,
    @Inject('BACKEND_BASE_URL') private backendBaseUrl: string) { }

  public createIncomingCall(from: string, to: string, url: string, httpMethod: string): Promise<CallResource> {
    const payload = new HttpParams()
      .set('from', from)
      .set('to', to)
      .set('url', url)
      .set('httpMethod', httpMethod);
    return this.http.post<CallResource>(this.backendBaseUrl + 'api/CallResources/Incoming', payload).toPromise();
  }

  public getCallResource(callSid: string): Promise<CallResource> {
    return this.http.get<CallResource>(this.backendBaseUrl + 'api/CallResources/' + callSid).toPromise();
  }

  public getCallResources(options?: GetCallResourcesOptions): Promise<Page<CallResource>> {
    return this.http.get<Page<CallResource>>(this.backendBaseUrl + 'api/CallResources' + objectToQueryString(options)).toPromise();
  }

  public getApiCalls(callSid: string): Promise<ApiCall[]> {
    return this.http.get<ApiCall[]>(this.backendBaseUrl + 'api/CallResources/' + callSid + '/ApiCalls').toPromise();
  }

  public getActivityLogs(callSid: string): Promise<ActivityLog[]> {
    return this.http.get<ApiCall[]>(this.backendBaseUrl + 'api/CallResources/' + callSid + '/ActivityLogs').toPromise();
  }

}
