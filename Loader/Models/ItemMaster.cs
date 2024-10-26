using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    public class ItemMaster
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }
        public int ItemTypeId { get; set; }
        public int CatagoryId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public int UnitOfMeasureId { get; set; }
        public int ManifactureId { get; set; }
        public int BrandId { get; set; }
        public int ModelId { get; set; }
        public string UniversalProductCode { get; set; } // Matches UniversalProductCode from the database
        public string ManifacturePartNo { get; set; } // Matches ManifacturePartNo from the database
        public string InternationalArticleNo { get; set; }
        public string ISBN { get; set; }
        public string Barcode { get; set; }
        public decimal PurchaseRate { get; set; }
        public int PurchaseTaxId { get; set; }
        public decimal SalesRate { get; set; }
        public int SalesTaxid { get; set; } // Matches SalesTaxid from the database
        public int MaxStocklvl { get; set; } // Matches MaxStocklvl from the database
        public int MinStocklvl { get; set; } // Matches MinStocklvl from the database
        public int DangerStocklvl { get; set; } // Matches DangerStocklvl from the database
        public int EOQlevel { get; set; } // Matches EOQlevel from the database
        public int ReorderPoint { get; set; } // Matches ReorderPoint from the database
        public int Status { get; set; }
        public int? MinLeadtime { get; set; } // Matches MinLeadtime from the database
        public int? MaxLeadTime { get; set; }
        public int? MinConsume { get; set; }
        public int? MaxConsume { get; set; }
        public int PostedBy { get; set; }
        public DateTime PostedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; } // Changed from int to DateTime

    }
}