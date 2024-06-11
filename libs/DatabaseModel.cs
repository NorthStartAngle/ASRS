using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIBS
{
    public class ASRS_Inventory
    {
        private string _Location_RowCol;
        private string _SKU;
        private string _ProductID;
        private DateTime _TimeIN;
        private bool _DoubleDeep;
        private bool _ReserveEmpty;
        private bool _Full;

        public string getLocation_RowCol()
        {
            return _Location_RowCol;
        }
        public ASRS_Inventory setLocation_RowCol(string value)
        {
            _Location_RowCol = value;
            return this;
        }
        public string getSKU()
        {
            return _SKU;
        }
        public ASRS_Inventory setSKU(string value)
        {
            _SKU = value;
            return this;
        }

        public string getProductID()
        {
            return _ProductID;
        }
        public ASRS_Inventory setProductID(string value)
        {
            _ProductID = value;
            return this;
        }

        public DateTime getTimeIN()
        {
            return _TimeIN;
        }
        public ASRS_Inventory setTimeIN(DateTime value)
        {
            _TimeIN = value;
            return this;
        }

        public bool getDoubleDeep()
        {
            return _DoubleDeep;
        }
        public ASRS_Inventory setDoubleDeep(bool value)
        {
            _DoubleDeep = value;
            return this;
        }

        public bool getReserveEmpty()
        {
            return _ReserveEmpty;
        }
        public ASRS_Inventory setReserveEmpty(bool value)
        {
            _ReserveEmpty = value;
            return this;
        }

        public bool getFull()
        {
            return _Full;
        }
        public ASRS_Inventory setFull(bool value)
        {
            _Full = value;
            return this;
        }

        public bool save(dbAccess _db)
        {
            string strQuery;
            if (_db != null)
            {
                strQuery = $"update ASRS_Inventory SET SKU='{_SKU}', ProductID='{_ProductID}', TimeIN=#{DateTime.Now.ToString("YYYY-MM-DD HH:mm")}#,ReserveEmpty={Convert.ToInt32(_ReserveEmpty)} ,Full={Convert.ToInt32(_Full)} where Location_RowCol='{_Location_RowCol}'";
                return _db.RunQueryWithAffecte(strQuery) == 1 ? true : false;
            }
            return false ;
        }
    
        public ASRS_Inventory clone(ASRS_Inventory source)
        {
            this._SKU = source.getSKU();
            this._ProductID = source.getProductID();
            this._TimeIN    = source.getTimeIN();
            this._DoubleDeep    = source.getDoubleDeep();
            this._ReserveEmpty = source.getReserveEmpty();
            this._Full = source.getFull();
            return this;
        }

        public ASRS_Inventory clone(ProductLookup source)
        {
            this._SKU = source.SKU;
            this._ProductID = source.ProductID;
            return this;
        }
    }

    public class ProductLookup
    {
        public int ID { get; set; }
        public string ProductID { get; set; }
        public string Image { get; set; }
        public string SKU { get; set; }
        public string Barcodes { get; set; }
    }

    public class PTL_Bay
    {
        public string BayID { get; set; }
        public string SKU_Assigned { get; set; }
    }

    public class USER
    {
        public int ID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int access_level { get; set; }   
    }
}
