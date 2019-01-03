import { Injectable, Inject } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Subject, Observable } from 'rxjs';
import { ConferenceResource } from '../models/ConferenceResource';

@Injectable({
  providedIn: 'root'
})
export class ConferenceResourcesHubService {

  private hubConnection!: HubConnection;

  private conferenceResourceUpdate: Subject<ConferenceResource> = new Subject<ConferenceResource>();

  constructor(@Inject('BACKEND_BASE_URL') private backendBaseUrl: string) { }

  public async connect(automaticReconnect: boolean = false): Promise<void> {
    if (this.hubConnection) {
        this.disconnect();
    }
    this.hubConnection = new HubConnectionBuilder().withUrl(this.backendBaseUrl + 'hubs/conferenceresources').build();

    this.hubConnection.on('ConferenceResourceUpdate', (conferenceResource) => this.conferenceResourceUpdate.next(conferenceResource));

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

public observeConferenceResourceUpdate(): Observable<ConferenceResource> { return this.conferenceResourceUpdate.asObservable(); }

}
