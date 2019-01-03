import { DataSource } from '@angular/cdk/collections';

import { ConferenceResource } from '../models/ConferenceResource';
import { Observable, BehaviorSubject } from 'rxjs';
import { MatPaginator } from '@angular/material';
import { ConferenceResourcesService, GetConferenceResourcesOptions } from '../backend-services/conference-resources.service';
import { ConferenceResourcesHubService } from '../backend-services/conference-resources-hub.service';

export class ConferenceResourceDataSource extends DataSource<ConferenceResource> {

  private data = new BehaviorSubject<ConferenceResource[]>([]);

  private getConferenceResourceOptions = new GetConferenceResourcesOptions();

  constructor(private paginator: MatPaginator | number,
    private conferenceResourcesService: ConferenceResourcesService,
    private conferenceResourcesHub: ConferenceResourcesHubService) {
    super();
    if (typeof paginator === 'number') {
      this.getConferenceResourceOptions.page = 1;
      this.getConferenceResourceOptions.pageSize = paginator;
    }
  }

  connect(): Observable<ConferenceResource[]> {
    if (this.paginator instanceof MatPaginator) {
      this.paginator.page.subscribe((_) => this.refresh());
    }
    this.conferenceResourcesHub.observeConferenceResourceUpdate().subscribe((updatedConferenceResource) => {
      // TODO: Insert the updatedConferenceResource into the existing array instead of doing a full refresh
      this.refresh();
    });
    this.conferenceResourcesHub.connect();
    return this.data.asObservable();
  }

  disconnect() {
    // TODO: Make sure we don't have a scope problem here.
    // The hub is injected in a global scope, but we are connecting/disconnecting it on a component scope here.
    this.conferenceResourcesHub.disconnect();
  }

  set statusFilter(value: string[]) { this.getConferenceResourceOptions.statusFilter = value; }
  get statusFilter(): string[] { return this.getConferenceResourceOptions.statusFilter; }

  public async refresh() {
    if (this.paginator instanceof MatPaginator) {
      this.getConferenceResourceOptions.page = this.paginator.pageIndex + 1;
      this.getConferenceResourceOptions.pageSize = this.paginator.pageSize;
    }
    const page = await this.conferenceResourcesService.getConferenceResources(this.getConferenceResourceOptions);
    if (this.paginator instanceof MatPaginator) {
      this.paginator.length = page.totalItems;
    }
    this.data.next(page.items);
  }
}
