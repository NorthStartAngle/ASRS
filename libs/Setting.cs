using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ASRS.libs
{
    public class Setting
    {
        private string _dbName;
        private string _dbPath;
        private static Setting _instance = null;

        Setting():this("","")
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
        public Setting(string dbPath= "",string dbName ="")
        {
            string localPath = Application.StartupPath;
            if (String.IsNullOrEmpty(dbPath))
            {
                _dbPath = localPath;
                _dbName = "db.accdb";
            }
            else
            {
                if (!Path.IsPathRooted(dbPath))
                {
                    _dbPath = Path.Combine(localPath, _dbPath);
                }
            }
            
            Setting._instance = this;
        }

        public string locationDB()
        {
            return $"{_dbPath}{Path.PathSeparator}{_dbName}";
        }

        public string conString
        {
            get
            {
                if(File.Exists(Path.Combine(_dbPath,_dbName)))
                {
                    return $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={_dbPath}{Path.PathSeparator}{_dbName};Persist Security Info=False;";
                }
                else
                {
                    return "";
                }
                
            }
        }
        string connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=<path to your .mdb>";

        ~Setting()
        {

        }
    }
}
