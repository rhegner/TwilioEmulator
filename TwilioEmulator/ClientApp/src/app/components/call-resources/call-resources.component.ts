import { Component, OnInit, ViewChild } from '@angular/core';
import { CallResourceDataSource } from 'src/app/data-sources/CallResourceDataSource';
import { CallResourcesService } from 'src/app/backend-services/call-resources.service';
import { CallResourcesHubService } from 'src/app/backend-services/call-resources-hub.service';
import { MatPaginator } from '@angular/material';

@Component({
  selector: 'app-call-resources',
  templateUrl: './call-resources.component.html',
  styleUrls: ['./call-resources.component.scss']
})
export class CallResourcesComponent implements OnInit {

  @ViewChild(MatPaginator) callsPaginator: MatPaginator;

  public callsDataSource: CallResourceDataSource;

  constructor(private callResourcesService: CallResourcesService,
    private callResourceHub: CallResourcesHubService) { }

  ngOnInit() {
    this.callsDataSource = new CallResourceDataSource(this.callsPaginator, this.callResourcesService, this.callResourceHub);
    this.callsDataSource.refresh();
  }

}
