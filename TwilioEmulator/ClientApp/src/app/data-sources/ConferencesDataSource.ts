import { DataSource } from '@angular/cdk/collections';

import { Conference } from '../models/Conference';
import { Observable, BehaviorSubject } from 'rxjs';
import { MatPaginator } from '@angular/material';
import { ConferencesService, GetConferencesOptions } from '../backend-services/conferences.service';
import { ResourceCudNotificationHubService } from '../backend-services/resource-cud-notification-hub.service';

export class ConferencesDataSource extends DataSource<Conference> {

  private data = new BehaviorSubject<Conference[]>([]);

  private getConferencesOptions = new GetConferencesOptions();

  constructor(private paginator: MatPaginator | number,
    private conferenceService: ConferencesService,
    private conferencesHub: ResourceCudNotificationHubService<Conference>) {
    super();
    if (typeof paginator === 'number') {
      this.getConferencesOptions.Page = 0;
      this.getConferencesOptions.PageSize = paginator;
    }
  }

  connect(): Observable<Conference[]> {
    if (this.paginator instanceof MatPaginator) {
      this.paginator.page.subscribe((_) => this.refresh());
    }

    this.conferencesHub.observeResourceCudOperationNotifications().subscribe((notification) => {
      // TODO: do full refresh only if necessary
      this.refresh();
    });
    this.conferencesHub.connect('conferences');

    return this.data.asObservable();
  }

  disconnect() {
    // TODO: Make sure we don't have a scope problem here.
    // The hub is injected in a global scope, but we are connecting/disconnecting it on a component scope here.
    this.conferencesHub.disconnect();
  }

  set statusFilter(value: string[]) { this.getConferencesOptions.Status = value; }
  get statusFilter(): string[] { return this.getConferencesOptions.Status; }

  public async refresh() {
    if (this.paginator instanceof MatPaginator) {
      this.getConferencesOptions.Page = this.paginator.pageIndex;
      this.getConferencesOptions.PageSize = this.paginator.pageSize;
    }
    const page = await this.conferenceService.getConferences(this.getConferencesOptions);
    /*
    if (this.paginator instanceof MatPaginator) {
      this.paginator.length = page.totalItems;
    }
    */
    this.data.next(page.conferences);
  }
}
