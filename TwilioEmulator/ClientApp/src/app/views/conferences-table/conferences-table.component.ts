import { Component, Input } from '@angular/core';
import { ConferencesDataSource } from 'src/app/data-sources/ConferencesDataSource';

@Component({
  selector: 'app-conferences-table',
  templateUrl: './conferences-table.component.html',
  styleUrls: ['./conferences-table.component.scss']
})
export class ConferencesTableComponent {

  @Input() dataSource: ConferencesDataSource;

  conferenceColumns = [ 'sid', 'friendlyName', 'status', 'participants' ];

  constructor() { }

}
