import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { MatPaginator } from '@angular/material';
import { CallsDataSource } from 'src/app/data-sources/CallsDataSource';
import { CallsService } from 'src/app/backend-services/calls.service';
import { ResourceCudNotificationHubService } from 'src/app/backend-services/resource-cud-notification-hub.service';
import { Call } from 'src/app/models/Call';

@Component({
  selector: 'app-calls',
  templateUrl: './calls.component.html',
  styleUrls: ['./calls.component.scss']
})
export class CallsComponent implements OnInit {

  @ViewChild(MatPaginator) callsPaginator: MatPaginator;

  public callsDataSource: CallsDataSource;

  constructor(private callsService: CallsService,
    @Inject('CallCudNotificationHubService') private callsHub: ResourceCudNotificationHubService<Call>) { }

  ngOnInit() {
    this.callsDataSource = new CallsDataSource(this.callsPaginator, this.callsService, this.callsHub);
    this.callsDataSource.refresh();
  }

}
