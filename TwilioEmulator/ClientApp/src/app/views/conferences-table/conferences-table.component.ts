import { Component, Input } from '@angular/core';
import { ConferenceResourceDataSource } from 'src/app/data-sources/ConferenceResourceDataSource';

@Component({
  selector: 'app-conferences-table',
  templateUrl: './conferences-table.component.html',
  styleUrls: ['./conferences-table.component.scss']
})
export class ConferencesTableComponent {

  @Input() dataSource: ConferenceResourceDataSource;

  conferenceColumns = [ 'sid', 'friendlyName', 'status', 'participants' ];

  constructor() { }

}
