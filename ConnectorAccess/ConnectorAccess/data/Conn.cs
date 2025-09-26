using System.Configuration;

namespace ConnectorAccess
{
    class Conn
    {
        private static string server = ConfigurationManager.AppSettings["ServerDb"];
        private static string database = ConfigurationManager.AppSettings["Database"];
        private static string username = ConfigurationManager.AppSettings["UserDb"];
        private static string password = ConfigurationManager.AppSettings["PwdDb"];

        static public string strConn = $"Data Source={server};Initial Catalog={database}; Connect Timeout=120;UID={username};PWD={password}";
    }
}
