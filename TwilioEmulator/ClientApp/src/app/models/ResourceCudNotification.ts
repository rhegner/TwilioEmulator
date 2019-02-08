import { ResourceCudOperation } from './Enums';

export class ResourceCudNotification<TResource> {
  constructor(resource?: TResource, operation?: ResourceCudOperation) {
    this.Resource = resource;
    this.Operation = operation;
  }

  Resource?: TResource;
  Operation?: ResourceCudOperation;
}
