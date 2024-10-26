using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    [Table("LevelAuthority", Schema = "LG")]
    public class LevelAuthority
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LvlId { get; set; }
        public string LvlName { get; set; }
        public int TypeId { get; set; }
    }
}