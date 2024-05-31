using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows.Forms;

namespace ASRS.libs
{
    public class dbAccess : IDisposable 
    {
        private bool _isConnected = false;
        private string ConnectionString;// = "Driver={Microsoft Access Driver (*.mdb, *.accdb)}; Dbq=C:\\Users\\sorush\\Documents\\nameOfDatabase.accdb; Uid = Admin; Pwd =; ",

        public dbAccess()
        {

        }

        public void connect(string strConnection)
        {
            try
            {
                if (string.IsNullOrEmpty(strConnection)) throw new ArgumentNullException();

                ConnectionString = strConnection;

                using (OleDbConnection conn = new OleDbConnection(ConnectionString))
                {
                    conn.Open();
                    /*using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM ASRS_Product", conn))
                    {
                        conn.Open();
                    }*/
                }
            }
            catch (Exception)
            {
                throw;
            }            
        }

        public void RunQuery(string query)
        {
            OdbcCommand command = new OdbcCommand(query);

            using (OdbcConnection connection = new OdbcConnection(ConnectionString))
            {
                command.Connection = connection;
                connection.Open();
                var reader = command.ExecuteReader();
            }
        }

        public List<ASRS_Inventory> GetPeople(string query)
        {
            var people = new List<ASRS_Inventory>();
            OdbcCommand command = new OdbcCommand(query);

            using (OdbcConnection connection = new OdbcConnection(ConnectionString))
            {
                command.Connection = connection;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var person = new ASRS_Inventory();
                        /*person.Name = reader.SafeGetString(0);
                        person.Height = reader.SafeGetDouble(1);
                        person.IsEmployed = reader.SafeGetBool(2);*/
                        people.Add(person);
                    }
                };
            }
            return people;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public bool isConnected { get { return _isConnected; } }
    }

    public static class SafeGetMethods
    {
        public static string SafeGetString(this OdbcDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }
        public static int SafeGetInt(this OdbcDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetInt32(colIndex);
            return 0;
        }
        public static double SafeGetDouble(this OdbcDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetDouble(colIndex);
            return 0;
        }
        public static DateTime SafeGetDate(this OdbcDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetDateTime(colIndex);
            return new DateTime();
        }
        public static bool SafeGetBool(this OdbcDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetBoolean(colIndex);
            return false;

        }
    }
}
