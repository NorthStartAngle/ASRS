using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Data.Common;
using System.Drawing;

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
                }

                _isConnected = true;
            }
            catch (Exception)
            {
                ConnectionString = "";
                _isConnected = false;
                throw;
            }
        }

        public void RunQueryWithCallBack(string query, Action<OleDbDataReader> proc)
        {
            if (!_isConnected) return;

            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                OleDbCommand command = new OleDbCommand(query, connection);
                try
                {
                    connection.Open();

                    proc(command.ExecuteReader());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    proc(null);
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public bool isConnected { get { return _isConnected; } }
    }

    public static class SafeGetMethods
    {
        public static string SafeGetString(this OleDbDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }
        public static int SafeGetInt(this OleDbDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetInt32(colIndex);
            return 0;
        }


        public static double SafeGetDouble(this OleDbDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetDouble(colIndex);
            return 0;
        }
        public static DateTime SafeGetDate(this OleDbDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetDateTime(colIndex);
            return new DateTime();
        }
        public static bool SafeGetBool(this OleDbDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetBoolean(colIndex);
            return false;

        }
    }
}
