import { Component, OnInit, Input } from '@angular/core';
import { CallResourceDataSource } from 'src/app/data-sources/CallResourceDataSource';

@Component({
  selector: 'app-calls-table',
  templateUrl: './calls-table.component.html',
  styleUrls: ['./calls-table.component.scss']
})
export class CallsTableComponent {

  @Input() dataSource: CallResourceDataSource;

  callColumns = [ 'direction', 'sid', 'from', 'to', 'status' ];

  constructor() { }

}
