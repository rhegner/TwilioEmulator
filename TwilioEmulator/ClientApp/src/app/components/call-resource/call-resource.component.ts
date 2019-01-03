import { Component, OnInit } from '@angular/core';
import { CallResourcesService } from 'src/app/backend-services/call-resources.service';
import { ActivatedRoute } from '@angular/router';
import { CallResource } from 'src/app/models/CallResource';
import { ApiCallDataSource } from 'src/app/data-sources/ApiCallDataSource';
import { ApiCallsHubService } from 'src/app/backend-services/api-calls-hub.service';
import { ApiCallDirection, ApiCallType } from 'src/app/models/Enums';
import { ApiCall } from 'src/app/models/ApiCall';
import { trigger, state, style, transition, animate } from '@angular/animations';

@Component({
  selector: 'app-call-resource',
  templateUrl: './call-resource.component.html',
  styleUrls: ['./call-resource.component.scss'],
  animations: [  trigger('detailExpand', [
    state('collapsed', style({height: '0px', minHeight: '0', display: 'none'})),
    state('expanded', style({height: '*'})),
    transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)'))
  ])]
})
export class CallResourceComponent implements OnInit {

  public ApiCallDirection = ApiCallDirection;
  public ApiCallType = ApiCallType;

  public call: CallResource;

  public apiCallsDataSource: ApiCallDataSource;
  public expandedElement: ApiCall | null;

  displayedColumns = [ 'direction', 'type', 'url', 'responseStatusCode' ];

  constructor(private route: ActivatedRoute,
    private callResourcesService: CallResourcesService,
    private apiCallsHub: ApiCallsHubService) { }

  async ngOnInit() {
    const callSid = this.route.snapshot.params['callSid'];
    this.call = await this.callResourcesService.getCallResource(callSid);
    this.apiCallsDataSource = new ApiCallDataSource(this.callResourcesService, this.apiCallsHub);
    this.apiCallsDataSource.callSid = callSid;
    this.apiCallsDataSource.refresh();
  }

}
