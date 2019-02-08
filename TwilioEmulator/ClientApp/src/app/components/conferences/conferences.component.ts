import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { MatPaginator } from '@angular/material';
import { ConferencesDataSource } from 'src/app/data-sources/ConferencesDataSource';
import { ConferencesService } from 'src/app/backend-services/conferences.service';
import { ResourceCudNotificationHubService } from 'src/app/backend-services/resource-cud-notification-hub.service';
import { Conference } from 'src/app/models/Conference';

@Component({
  selector: 'app-conferences',
  templateUrl: './conferences.component.html',
  styleUrls: ['./conferences.component.scss']
})
export class ConferencesComponent implements OnInit {

  @ViewChild(MatPaginator) conferencesPaginator: MatPaginator;

  public conferencesDataSource: ConferencesDataSource;

  constructor(private conferencesService: ConferencesService,
    @Inject('ConferenceCudNotificationHubService') private conferencesHub: ResourceCudNotificationHubService<Conference>) { }

  ngOnInit() {
    // tslint:disable-next-line:max-line-length
    this.conferencesDataSource = new ConferencesDataSource(this.conferencesPaginator, this.conferencesService, this.conferencesHub);
    this.conferencesDataSource.refresh();
  }

}
