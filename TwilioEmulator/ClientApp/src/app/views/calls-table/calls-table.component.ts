import { Component, OnInit, Input } from '@angular/core';
import { CallsDataSource } from 'src/app/data-sources/CallsDataSource';

@Component({
  selector: 'app-calls-table',
  templateUrl: './calls-table.component.html',
  styleUrls: ['./calls-table.component.scss']
})
export class CallsTableComponent {

  @Input() dataSource: CallsDataSource;

  callColumns = [ 'direction', 'sid', 'from', 'to', 'status' ];

  constructor() { }

}
