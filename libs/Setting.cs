using ASRS.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LIBS
{
    public class Setting
    {
        private string _dbPath;
        private static Setting _instance = null;
        private USER _loginUser = null;
        Setting():this("")
        {

        }
        public bool Save()
        {
            return true;
        }
        public bool Load()
        {
            return true;
        }

        public static Setting instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Setting();
                }
                return _instance;
            }
        }
        public Setting(string dbPath= "")
        {
            if (String.IsNullOrEmpty(dbPath))
            {
                _dbPath = Application.StartupPath + "\\ASRS.accdb";
            }
            else
            {
                _dbPath = dbPath;
            }

            Setting._instance = this;
        }

        public string locationDB()
        {
            return $"{_dbPath}";
        }

        public string conString
        {
            get
            {
                if(File.Exists(_dbPath))
                {
                    return "Provider=Microsoft.ACE.OLEDB.12.0;" + $"Data Source={_dbPath}";
                }
                else
                {
                    return "";
                }                           
            }
        }

        public USER LoginUser { get => _loginUser; set { _loginUser = value; } }

        ~Setting()
        {

        }
    }
}
