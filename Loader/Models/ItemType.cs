using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    public class ItemType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemTypeId { get; set; }
        public string ItemTypeName { get; set; }
        public int Status { get; set; }
        public int isParent { get; set; }
    }
}