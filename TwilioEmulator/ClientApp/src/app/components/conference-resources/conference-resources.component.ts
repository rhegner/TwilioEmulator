import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material';
import { ConferenceResourceDataSource } from 'src/app/data-sources/ConferenceResourceDataSource';
import { ConferenceResourcesService } from 'src/app/backend-services/conference-resources.service';
import { ConferenceResourcesHubService } from 'src/app/backend-services/conference-resources-hub.service';

@Component({
  selector: 'app-conference-resources',
  templateUrl: './conference-resources.component.html',
  styleUrls: ['./conference-resources.component.scss']
})
export class ConferenceResourcesComponent implements OnInit {

  @ViewChild(MatPaginator) conferencesPaginator: MatPaginator;

  public conferencesDataSource: ConferenceResourceDataSource;

  constructor(private conferenceResourcesService: ConferenceResourcesService,
    private conferenceResourceHub: ConferenceResourcesHubService) { }

  ngOnInit() {
    // tslint:disable-next-line:max-line-length
    this.conferencesDataSource = new ConferenceResourceDataSource(this.conferencesPaginator, this.conferenceResourcesService, this.conferenceResourceHub);
    this.conferencesDataSource.refresh();
  }

}
