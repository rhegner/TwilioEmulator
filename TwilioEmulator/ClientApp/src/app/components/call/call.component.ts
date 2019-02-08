import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiCallDataSource } from 'src/app/data-sources/ApiCallDataSource';
import { ApiCallsHubService } from 'src/app/backend-services/api-calls-hub.service';
import { ApiCallDirection, ApiCallType } from 'src/app/models/Enums';
import { ApiCall } from 'src/app/models/ApiCall';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { CallsService } from 'src/app/backend-services/calls.service';
import { Call } from 'src/app/models/Call';

@Component({
  selector: 'app-call',
  templateUrl: './call.component.html',
  styleUrls: ['./call.component.scss'],
  animations: [  trigger('detailExpand', [
    state('collapsed', style({height: '0px', minHeight: '0', display: 'none'})),
    state('expanded', style({height: '*'})),
    transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)'))
  ])]
})
export class CallComponent implements OnInit {

  public ApiCallDirection = ApiCallDirection;
  public ApiCallType = ApiCallType;

  public call: Call;

  public apiCallsDataSource: ApiCallDataSource;
  public expandedElement: ApiCall | null;

  displayedColumns = [ 'direction', 'type', 'url', 'responseStatusCode' ];

  constructor(private route: ActivatedRoute,
    private callsService: CallsService,
    private apiCallsHub: ApiCallsHubService) { }

  async ngOnInit() {
    const callSid = this.route.snapshot.params['callSid'];
    this.call = await this.callsService.getCall(callSid);
    /*
    this.apiCallsDataSource = new ApiCallDataSource(this.callResourcesService, this.apiCallsHub);
    this.apiCallsDataSource.callSid = callSid;
    this.apiCallsDataSource.refresh();
    */
  }

}
