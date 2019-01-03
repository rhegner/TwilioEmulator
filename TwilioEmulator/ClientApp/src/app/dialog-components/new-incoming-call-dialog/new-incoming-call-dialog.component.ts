import { Component, OnInit } from '@angular/core';
import { CallResourcesService } from 'src/app/backend-services/call-resources.service';
import { MatDialogRef } from '@angular/material';

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

  constructor(private dialogRef: MatDialogRef<NewIncomingCallDialogComponent>,
    private callResourcesService: CallResourcesService) { }

  ngOnInit() {
    // TODO: get rid of this:
    this.url = 'http://localhost:5000/api/Twilio/Welcome';
    this.from = '+41788831118';
    this.to = '+41435051119';
  }

  public async ok() {
    await this.callResourcesService.createIncomingCall(this.from, this.to, this.url, this.httpMethod);
    this.dialogRef.close();
  }
}
