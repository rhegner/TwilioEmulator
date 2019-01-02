import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CallResource } from '../models/CallResource';

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
}
