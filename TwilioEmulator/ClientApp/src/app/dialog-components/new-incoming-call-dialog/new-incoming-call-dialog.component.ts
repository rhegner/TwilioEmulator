import { Component, OnInit } from '@angular/core';
import { CallResourcesService } from 'src/app/backend-services/call-resources.service';

@Component({
  selector: 'app-new-incoming-call-dialog',
  templateUrl: './new-incoming-call-dialog.component.html',
  styleUrls: ['./new-incoming-call-dialog.component.scss']
})
export class NewIncomingCallDialogComponent implements OnInit {

  public httpMethod = 'post';
  public url = '';
  public from = '';
  public to = '';

  constructor(private callResourcesService: CallResourcesService) { }

  ngOnInit() {
  }

  public async ok() {
    await this.callResourcesService.createIncomingCall(this.from, this.to, this.url, this.httpMethod);
  }
}
