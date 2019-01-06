using System;
using TwilioLogic.Interfaces;

namespace TwilioLogic.EventModels
{
    public enum ResourceCudOperation
    {
        Create = 0,
        Update = 1,
        Delete = 2
    }

    public class ResourceCudOperationEventArgs<T> : EventArgs
        where T: IResource
    {
        public ResourceCudOperationEventArgs(T resource, ResourceCudOperation operation)
        {
            Resource = resource;
            Operation = operation;
        }
        public T Resource { get; }
        public ResourceCudOperation Operation { get; }
    }
}
