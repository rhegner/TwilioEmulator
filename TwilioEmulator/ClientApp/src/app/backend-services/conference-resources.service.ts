import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConferenceResource } from '../models/ConferenceResource';
import { Page } from '../models/Page';
import { objectToQueryString } from './utils';

export class GetConferenceResourcesOptions {
  statusFilter?: string[];
  page?: number;
  pageSize?: number;
}

@Injectable({
  providedIn: 'root'
})
export class ConferenceResourcesService {

  constructor(private http: HttpClient,
    @Inject('BACKEND_BASE_URL') private backendBaseUrl: string) { }

  public getConferenceResource(sid: string): Promise<ConferenceResource> {
    return this.http.get<ConferenceResource>(this.backendBaseUrl + 'api/ConferenceResources/' + sid).toPromise();
  }

  public getConferenceResources(options?: GetConferenceResourcesOptions): Promise<Page<ConferenceResource>> {
    // tslint:disable-next-line:max-line-length
    return this.http.get<Page<ConferenceResource>>(this.backendBaseUrl + 'api/ConferenceResources' + objectToQueryString(options)).toPromise();
  }

}
