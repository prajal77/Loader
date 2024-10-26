using System;
using System.Runtime.Serialization;

namespace Loader
{ 
    [Serializable]
    [DataContract(IsReference = true)]
    public abstract class EntityBase
    {
    }
}
