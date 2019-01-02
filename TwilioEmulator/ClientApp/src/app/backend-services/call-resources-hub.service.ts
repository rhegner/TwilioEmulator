import { Injectable, Inject } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Subject, Observable } from 'rxjs';
import { CallResource } from '../models/CallResource';

@Injectable({
  providedIn: 'root'
})
export class CallResourcesHubService {

  private hubConnection!: HubConnection;

  private callResourceUpdate: Subject<CallResource> = new Subject<CallResource>();

  constructor(@Inject('BACKEND_BASE_URL') private backendBaseUrl: string) { }

  public async connect(automaticReconnect: boolean = false): Promise<void> {
    if (this.hubConnection) {
        this.disconnect();
    }
    this.hubConnection = new HubConnectionBuilder().withUrl(this.backendBaseUrl + 'hubs/callresources').build();

    this.hubConnection.on('CallResourceUpdate', (callResource, isNew) => this.callResourceUpdate.next(callResource));

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

public observeCallResourceUpdate(): Observable<CallResource> { return this.callResourceUpdate.asObservable(); }

}
