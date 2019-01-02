import { DataSource } from '@angular/cdk/collections';

import { CallResource } from '../models/CallResource';
import { Observable, BehaviorSubject } from 'rxjs';
import { MatPaginator } from '@angular/material';
import { CallResourcesService, GetCallResourcesOptions } from '../backend-services/call-resources.service';

export class CallResourceDataSource extends DataSource<CallResource> {

  private data = new BehaviorSubject<CallResource[]>([]);

  private getCallResourceOptions = new GetCallResourcesOptions();

  constructor(private paginator: MatPaginator | number,
    private callResourcesService: CallResourcesService) {
    super();
    if (typeof paginator === 'number') {
      this.getCallResourceOptions.page = 1;
      this.getCallResourceOptions.pageSize = paginator;
    }
  }

  connect(): Observable<CallResource[]> {
    if (this.paginator instanceof MatPaginator) {
      this.paginator.page.subscribe((_) => this.refresh());
    }
    return this.data.asObservable();
  }

  disconnect() {

  }

  set directionFilter(value: string[]) { this.getCallResourceOptions.directionFilter = value; }
  get directionFilter(): string[] { return this.getCallResourceOptions.directionFilter; }

  set statusFilter(value: string[]) { this.getCallResourceOptions.statusFilter = value; }
  get statusFilter(): string[] { return this.getCallResourceOptions.statusFilter; }

  public async refresh() {
    if (this.paginator instanceof MatPaginator) {
      this.getCallResourceOptions.page = this.paginator.pageIndex + 1;
      this.getCallResourceOptions.pageSize = this.paginator.pageSize;
    }
    const page = await this.callResourcesService.getCallResources(this.getCallResourceOptions);
    if (this.paginator instanceof MatPaginator) {
      this.paginator.length = page.totalItems;
    }
    this.data.next(page.items);
  }
}
