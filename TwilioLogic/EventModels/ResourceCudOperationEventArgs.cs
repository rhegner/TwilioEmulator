using System;
using TwilioLogic.Interfaces;

namespace TwilioLogic.EventModels
{
    public enum ResourceCudOperation
    {
        Reset = 0,
        Create = 1,
        Update = 2,
        Delete = 3,
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
