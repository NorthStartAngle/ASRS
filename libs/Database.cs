using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.libs
{
    public class Database
    {
        private SQLiteConnection _db = null;
        private bool _isConnected = false;

        public Database() { }

        public bool connect()
        {
            return this.connect(Setting.instance.locationDB());
        }
        private bool connect(string dbPath)
        {
            if (string.IsNullOrEmpty(dbPath) || _db != null)
            {
                return _isConnected;
            }

            try
            {
                _db = new SQLiteConnection(dbPath);
            }

            catch(Exception)
            {
                return false;
            }

            if (_db != null)
            {
                _isConnected = true;
                return true;
            }
            return false;
        }

        public bool isConnected { 
            get {
                return _isConnected;
            } 
        }

        public void createDatabase()
        {
            //var results = _db.Query<Record>("SELECT * FROM records");
            //var results = _db.Table<Record>().ToList();
            //var results = _db.Table<Record>().Where(t => t.Age > 40).OrderByDescending(t => t.Age).ToList();
            //var query = db.Table<Stock>().Where(v => v.Symbol.StartsWith("A"));
            //foreach (var stock in query) Console.WriteLine("Stock: " + stock.Symbol);


            //var results = _db.Query<Record>($"INSERT INTO records VALUES ('{new Guid()}', 'Mark', '23', '{DateTime.Now}')");
            //var results = _db.Insert(new Record { Id = new Guid(), Name = "Mark", Age = 23, Date = DateTime.Now });

            // var results = _db.Update(new Record { Id = id, Name = "Mark", Age = 23, Date = DateTime.Now });

            //_db.Delete<Record>("5");

            //db.Execute("create table Stock(Symbol varchar(100) not null)");
            //db.Execute("insert into Stock(Symbol) values (?)", "MSFT");
            //var stocks = db.Query<Stock>("select * from Stock");
        }

        ~Database() { 
           _db?.Close();

        }
    }
}
