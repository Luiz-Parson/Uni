using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ConnectorAccess
{
    class SystemUser
    {
        protected static readonly Logger Logger = new Logger();

        public int Id { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }
        public string Password { get; set; }

        private const string EncryptionKey = "MAKV2SPBNI99212";

        public static DataTable GetAll()
        {
            DataTable dt = new DataTable();

            string sql = "SELECT Id, Username, IsAdmin FROM SystemUser ORDER BY Id DESC";

            try
            {
                using (var cn = new SqlConnection(Conn.strConn))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cn.Open();
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro GetAll SystemUser", ex);
                throw ex;
            }
            return dt;
        }

        public static DataTable GetSearchByFilter(string txtSearch)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT Id, Username, IsAdmin FROM SystemUser WHERE ";

            int n;
            bool isNumeric = int.TryParse(txtSearch, out n);
            if (isNumeric)
                sql += "Id=@txtSearch ";
            else
                sql += "Username LIKE @txtSearch ";

            sql += "ORDER BY Id DESC";
            try
            {
                using (var cn = new SqlConnection(Conn.strConn))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@txtSearch", (isNumeric ? txtSearch : "%" + txtSearch + "%"));
                        cn.Open();
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro GetSearchByFilter SystemUser", ex);
                throw ex;
            }
            return dt;
        }

        public static SystemUser GetById(int id)
        {
            SystemUser result = null;

            string sql = "SELECT Id, Username, IsAdmin, Password FROM SystemUser WHERE Id=@id";

            try
            {
                using (var cn = new SqlConnection(Conn.strConn))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@id", id);
                        cn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                result = new SystemUser();
                                result.Id = Convert.ToInt32(reader[0]);
                                result.Username = reader[1].ToString();
                                result.IsAdmin = (bool)reader[2];
                                result.Password = Decrypt(reader[3].ToString());
                            }
                        }
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro GetById SystemUser", ex);
                throw ex;
            }
            return result;
        }

        public static bool DeleteById(int id)
        {
            var isSuccess = false;

            string sql = "DELETE FROM SystemUser WHERE Id=@id";

            try
            {
                using (var cn = new SqlConnection(Conn.strConn))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@id", id);
                        cn.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                            isSuccess = true;
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro DeleteById SystemUser", ex);
                throw ex;
            }
            return isSuccess;
        }

        public static bool Insert(SystemUser obj)
        {
            var isSuccess = false;

            string sql = "INSERT INTO SystemUser (Username, IsAdmin, Password, CreatedBy, UpdatedBy) VALUES (@username, @isadmin, @password, @createdby, @updatedby)";

            try
            {
                using (var cn = new SqlConnection(Conn.strConn))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@username", obj.Username);
                        cmd.Parameters.AddWithValue("@isadmin", obj.IsAdmin);
                        cmd.Parameters.AddWithValue("@password", Encrypt(obj.Password));
                        cmd.Parameters.AddWithValue("@createdby", Program.systemUserLogged.Username);
                        cmd.Parameters.AddWithValue("@updatedby", Program.systemUserLogged.Username);
                        cn.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                            isSuccess = true;
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro Insert SystemUser", ex);
                throw ex;
            }
            return isSuccess;
        }

        public static bool Update(SystemUser obj)
        {
            var isSuccess = false;

            string sql = "UPDATE SystemUser SET Password=@password, IsAdmin=@isadmin, UpdatedBy=@updatedby, UpdatedOn=GETDATE() WHERE Id=@id";

            try
            {
                using (var cn = new SqlConnection(Conn.strConn))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@password", Encrypt(obj.Password));
                        cmd.Parameters.AddWithValue("@isadmin", obj.IsAdmin);
                        cmd.Parameters.AddWithValue("@updatedby", Program.systemUserLogged.Username);
                        cmd.Parameters.AddWithValue("@id", obj.Id);
                        cn.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                            isSuccess = true;
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro Update SystemUser", ex);
                throw ex;
            }
            return isSuccess;
        }

        public static SystemUser Login(string usrname, string pwd)
        {
            SystemUser result= null;
            string sql = "SELECT Id, Username, IsAdmin FROM SystemUser WHERE Username=@Username AND Password=@Password";
            try
            {
                DataTable dt = new DataTable();
                using (var cn = new SqlConnection(Conn.strConn))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@Username", usrname);
                        cmd.Parameters.AddWithValue("@Password", Encrypt(pwd));
                        cn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                result = new SystemUser();
                                result.Id = Convert.ToInt32(reader[0]);
                                result.Username = reader[1].ToString();
                                result.IsAdmin = (bool)reader[2];
                            }
                        }
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro Login SystemUser", ex);
                throw ex;
            }
            return result;
        }

        private static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        private static string Decrypt(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}
