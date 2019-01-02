import { Component } from '@angular/core';
import { MatDialog } from '@angular/material';
import { NewIncomingCallDialogComponent } from './dialog-components/new-incoming-call-dialog/new-incoming-call-dialog.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  constructor(private dialogService: MatDialog) {}

  public async makeCall(): Promise<void> {
    const dialogRef = this.dialogService.open(NewIncomingCallDialogComponent);
    await dialogRef.afterClosed().toPromise();
  }
}
