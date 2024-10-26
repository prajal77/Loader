using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    public class InvestmentCodeDetails
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvestmentCodeId { get; set; }
        public string InvestmentCodeName { get; set;}
        public string NavInvCode { get; set;}
    }
}