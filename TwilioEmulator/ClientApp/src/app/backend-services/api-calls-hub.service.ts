import { Injectable, Inject } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Subject, Observable } from 'rxjs';
import { ApiCall } from '../models/ApiCall';

@Injectable({
  providedIn: 'root'
})
export class ApiCallsHubService {

  private hubConnection!: HubConnection;

  private newApiCall: Subject<ApiCall> = new Subject<ApiCall>();

  constructor(@Inject('BACKEND_BASE_URL') private backendBaseUrl: string) { }

  public async connect(automaticReconnect: boolean = false): Promise<void> {
    if (this.hubConnection) {
        this.disconnect();
    }
    this.hubConnection = new HubConnectionBuilder().withUrl(this.backendBaseUrl + 'hubs/apicalls').build();

    this.hubConnection.on('NewApiCall', (apiCall) => this.newApiCall.next(apiCall));

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

  public observeNewApiCalls(): Observable<ApiCall> { return this.newApiCall.asObservable(); }

}
