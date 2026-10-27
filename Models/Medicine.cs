using System;

namespace SchoolClinicManagementSystem.Models
{
    public class Medicine
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string BrandName { get; set; }     
        public string GenericName { get; set; }  
        public string Category { get; set; }      
        public string DosageForm { get; set; }   
        public int StockQuantity { get; set; }
        public string Unit { get; set; }          
        public int ReorderLevel { get; set; } = 15;
        public DateTime ExpirationDate { get; set; }

        public string Status
        {
            get
            {
                if (DateTime.Now > ExpirationDate) return "Expired";
                if (StockQuantity <= 0) return "Out of Stock";
                if (StockQuantity <= ReorderLevel) return "Low Stock";
                return "In Stock";
            }
        }

        public string DisplayName => $"{BrandName} ({GenericName} {DosageForm}) - {StockQuantity} {Unit} left";
    }
}
