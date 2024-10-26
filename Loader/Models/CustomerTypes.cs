using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    [Table("CustomerType", Schema = "DBO")]
    public class CustomerTypes
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CtypeId")]
        public int CtypeId { get; set; }
        public string CustomerType { get; set; }
        public int Status { get; set; }
    }
}