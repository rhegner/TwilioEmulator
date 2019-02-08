import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { CallsDataSource } from 'src/app/data-sources/CallsDataSource';
import { CallsService } from 'src/app/backend-services/calls.service';
import { ResourceCudNotificationHubService } from 'src/app/backend-services/resource-cud-notification-hub.service';
import { Call } from 'src/app/models/Call';
import { ConferencesDataSource } from 'src/app/data-sources/ConferencesDataSource';
import { ConferencesService } from 'src/app/backend-services/conferences.service';
import { Conference } from 'src/app/models/Conference';

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.scss']
})
export class OverviewComponent implements OnInit {

  public callsDataSource: CallsDataSource;
  public conferencesDataSource: ConferencesDataSource;

  constructor(private callService: CallsService,
    @Inject('CallCudNotificationHubService') private callsHub: ResourceCudNotificationHubService<Call>,
    private conferencesService: ConferencesService,
    @Inject('ConferenceCudNotificationHubService') private conferencesHub: ResourceCudNotificationHubService<Conference>) {
  }

  ngOnInit() {
    this.callsDataSource = new CallsDataSource(20, this.callService, this.callsHub);
    this.callsDataSource.statusFilter = [ 'queued', 'ringing', 'in-progress' ];
    this.callsDataSource.refresh();
    this.conferencesDataSource = new ConferencesDataSource(20, this.conferencesService, this.conferencesHub);
    this.conferencesDataSource.statusFilter = [ 'init', 'in-progress' ];
    this.conferencesDataSource.refresh();
  }

}
