import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material';
import { CallResourceDataSource } from 'src/app/data-sources/CallResourceDataSource';
import { CallResourcesService } from 'src/app/backend-services/call-resources.service';

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.scss']
})
export class OverviewComponent implements OnInit {

  @ViewChild(MatPaginator) callsPaginator: MatPaginator;

  public callsDataSource: CallResourceDataSource;

  displayedColumns = [ 'direction', 'sid', 'from', 'to', 'status' ];

  constructor(private callResourcesService: CallResourcesService) {
  }

  ngOnInit() {
    this.callsDataSource = new CallResourceDataSource(20, this.callResourcesService);
    this.callsDataSource.statusFilter = [ 'queued', 'ringing', 'in-progress' ];
    this.callsDataSource.refresh();
  }

}
