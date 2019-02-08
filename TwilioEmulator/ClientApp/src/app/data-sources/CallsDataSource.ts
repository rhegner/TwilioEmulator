import { DataSource } from '@angular/cdk/collections';

import { Observable, BehaviorSubject } from 'rxjs';
import { MatPaginator } from '@angular/material';
import { Call } from '../models/Call';
import { GetCallsOptions, CallsService } from '../backend-services/calls.service';
import { ResourceCudNotificationHubService } from '../backend-services/resource-cud-notification-hub.service';

export class CallsDataSource extends DataSource<Call> {

  private data = new BehaviorSubject<Call[]>([]);

  private getCallsOptions = new GetCallsOptions();

  constructor(private paginator: MatPaginator | number,
    private callsService: CallsService,
    private callsHub: ResourceCudNotificationHubService<Call>) {
    super();
    if (typeof paginator === 'number') {
      this.getCallsOptions.Page = 0;
      this.getCallsOptions.PageSize = paginator;
    }
  }

  connect(): Observable<Call[]> {
    if (this.paginator instanceof MatPaginator) {
      this.paginator.page.subscribe((_) => this.refresh());
    }

    this.callsHub.observeResourceCudOperationNotifications().subscribe((notification) => {
      // TODO: do full refresh only if necessary
      this.refresh();
    });
    this.callsHub.connect('calls');

    return this.data.asObservable();
  }

  disconnect() {
    // TODO: Make sure we don't have a scope problem here.
    // The hub is injected in a global scope, but we are connecting/disconnecting it on a component scope here.
    this.callsHub.disconnect();
  }

  set directionFilter(value: string[]) { this.getCallsOptions.Direction = value; }
  get directionFilter(): string[] { return this.getCallsOptions.Direction; }

  set statusFilter(value: string[]) { this.getCallsOptions.Status = value; }
  get statusFilter(): string[] { return this.getCallsOptions.Status; }

  public async refresh() {
    if (this.paginator instanceof MatPaginator) {
      this.getCallsOptions.Page = this.paginator.pageIndex;
      this.getCallsOptions.PageSize = this.paginator.pageSize;
    }
    const page = await this.callsService.getCalls(this.getCallsOptions);
    /*
    if (this.paginator instanceof MatPaginator) {
      this.paginator.length = page.totalItems;
    }
    */
    this.data.next(page.calls);
  }
}
