import { Component, OnInit, ViewChild } from '@angular/core';
import { CallResourceDataSource } from 'src/app/data-sources/CallResourceDataSource';
import { CallResourcesService } from 'src/app/backend-services/call-resources.service';
import { CallResourcesHubService } from 'src/app/backend-services/call-resources-hub.service';
import { ConferenceResourceDataSource } from 'src/app/data-sources/ConferenceResourceDataSource';
import { ConferenceResourcesService } from 'src/app/backend-services/conference-resources.service';
import { ConferenceResourcesHubService } from 'src/app/backend-services/conference-resources-hub.service';

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.scss']
})
export class OverviewComponent implements OnInit {

  public callsDataSource: CallResourceDataSource;
  public conferencesDataSource: ConferenceResourceDataSource;

  constructor(private callResourcesService: CallResourcesService,
    private callResourceHub: CallResourcesHubService,
    private conferenceResourcesService: ConferenceResourcesService,
    private conferenceResourceHub: ConferenceResourcesHubService) {
  }

  ngOnInit() {
    this.callsDataSource = new CallResourceDataSource(20, this.callResourcesService, this.callResourceHub);
    this.callsDataSource.statusFilter = [ 'queued', 'ringing', 'in-progress' ];
    this.callsDataSource.refresh();
    this.conferencesDataSource = new ConferenceResourceDataSource(20, this.conferenceResourcesService, this.conferenceResourceHub);
    this.conferencesDataSource.statusFilter = [ 'init', 'in-progress' ];
    this.conferencesDataSource.refresh();
  }

}
