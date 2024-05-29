using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.libs
{
    [Table("ASRS_Inventory")]
    public class ASRS_Inventory
    {
        [PrimaryKey]
        [Column("Location_RowCol")]
        public string Location_RowCol { get; set; }
        public string SKU { get; set; }
        public string ProductID { get; set; }
        public DateTime TimeIN { get; set; }
        public bool DoubleDeep { get; set; }
        public bool ReserveEmpty { get; set; }

        public bool Full { get; set; }
    }

    [Table("ProductLookup")]
    public class ProductLookup
    {
        [PrimaryKey,AutoIncrement]
        [Column("ID")]
        public int ID { get; set; }
        public string ProductID { get; set; }
        public string Image { get; set; }
        public string SKU { get; set; }
        public string Barcodes { get; set; }
    }

    [Table("PTL_Bay")]
    public class PTL_Bay
    {
        [PrimaryKey, AutoIncrement]
        [Column("BayID")]
        public string BayID { get; set; }
        public string SKU_Assigned { get; set; }
    }
}
