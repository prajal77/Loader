using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Loader
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class Product : EntityBase
    {
        [DataMember]
        [Display(Name = "ID")]
        public Int32 ProductID { get; set; }

        [DataMember]
        [Display(Name = "Name")]
        public String Name { get; set; }

        [DataMember]
        [Display(Name = "Unit Price")]
        public Decimal UnitPrice { get; set; }

        [DataMember]
        [Display(Name = "Quantity")]
        public Decimal Quantity { get; set; }
    }
}