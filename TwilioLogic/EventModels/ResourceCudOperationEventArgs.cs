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
        where T: class, IResource
    {
        public ResourceCudOperationEventArgs()
        {
            Resource = null;
            Operation = ResourceCudOperation.Reset;
        }

        public ResourceCudOperationEventArgs(T resource, ResourceCudOperation operation)
        {
            if (operation == ResourceCudOperation.Reset)
            {
                if (resource != null)
                    throw new ArgumentException("Resource must be null for Reset operation.");
            }
            else
            {
                if (resource == null)
                    throw new ArgumentException("Resource must not be null for all operations except for Reset");
            }
            Resource = resource;
            Operation = operation;
        }
        public T Resource { get; }
        public ResourceCudOperation Operation { get; }
    }
}
