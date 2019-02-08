import { Injectable, Inject } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Subject, Observable } from 'rxjs';
import { ResourceCudNotification } from '../models/ResourceCudNotification';

@Injectable()
export class ResourceCudNotificationHubService<TResource> {

  private hubConnection!: HubConnection;

  private resourceCudOperationNotification: Subject<ResourceCudNotification<TResource>> =
    new Subject<ResourceCudNotification<TResource>>();

  constructor(@Inject('BACKEND_BASE_URL') private backendBaseUrl: string) { }

  public async connect(hubName: string, automaticReconnect: boolean = false): Promise<void> {
    if (this.hubConnection) {
        this.disconnect();
    }
    this.hubConnection = new HubConnectionBuilder().withUrl(this.backendBaseUrl + 'hubs/' + hubName).build();

    this.hubConnection.on('ResourceCudOperation', (resource, operation) =>
      this.resourceCudOperationNotification.next(new ResourceCudNotification<TResource>(resource, operation)));

    if (automaticReconnect) {
    this.hubConnection.onclose(async () => {
      await this.connect(hubName, automaticReconnect);
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

  public observeResourceCudOperationNotifications(): Observable<ResourceCudNotification<TResource>> {
    return this.resourceCudOperationNotification.asObservable();
  }

}
