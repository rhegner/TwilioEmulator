import { Injectable, Inject } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Subject, Observable } from 'rxjs';
import { ActivityLog } from '../models/ActivityLog';

@Injectable({
  providedIn: 'root'
})
export class ActivityLogsHubService {

  private hubConnection!: HubConnection;

  private newActivityLog: Subject<ActivityLog> = new Subject<ActivityLog>();

  constructor(@Inject('BACKEND_BASE_URL') private backendBaseUrl: string) { }

  public async connect(automaticReconnect: boolean = false): Promise<void> {
    if (this.hubConnection) {
        this.disconnect();
    }
    this.hubConnection = new HubConnectionBuilder().withUrl(this.backendBaseUrl + 'hubs/activitylogs').build();

    this.hubConnection.on('NewActivityLog', (apiCall) => this.newActivityLog.next(apiCall));

    if (automaticReconnect) {
    this.hubConnection.onclose(async () => {
      await this.connect(automaticReconnect);
    });
  }

    await this.hubConnection.start();
  }

  public disconnect(): void {
    if (!this.hubConnection) {
        return;
    }
    this.hubConnection.stop();
  }

  public observeNewActivityLogs(): Observable<ActivityLog> { return this.newActivityLog.asObservable(); }
}
