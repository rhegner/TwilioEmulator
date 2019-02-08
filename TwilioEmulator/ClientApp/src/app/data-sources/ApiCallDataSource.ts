import { DataSource } from '@angular/cdk/table';
import { ApiCall } from '../models/ApiCall';
import { BehaviorSubject, Observable } from 'rxjs';
import { ApiCallsHubService } from '../backend-services/api-calls-hub.service';

export class ApiCallDataSource extends DataSource<ApiCall> {

  private data = new BehaviorSubject<ApiCall[]>([]);

  public callSid: string;

  constructor(/* private callResourcesService: CallResourcesService, */
    private apiCallsHub: ApiCallsHubService) {
    super();
  }

  connect(): Observable<ApiCall[]> {
    this.apiCallsHub.observeNewApiCalls().subscribe((newApiCall) => {
      // TODO: Insert the newApiCall into the existing array instead of doing a full refresh
      this.refresh();
    });
    this.apiCallsHub.connect();
    return this.data.asObservable();
  }

  disconnect() {
    // TODO: Make sure we don't have a scope problem here.
    // The hub is injected in a global scope, but we are connecting/disconnecting it on a component scope here.
    this.apiCallsHub.disconnect();
  }

  public async refresh() {
    /*
    if (this.callSid) {
      const apiCalls = await this.callResourcesService.getApiCalls(this.callSid);
      this.data.next(apiCalls);
    } else {
      this.data.next([]);
    }
    */
  }

}
