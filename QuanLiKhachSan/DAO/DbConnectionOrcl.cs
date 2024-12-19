using QuanLiKhachSan.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Oracle.ManagedDataAccess.Client;
using System.Security.Cryptography;

namespace QuanLiKhachSan.DAO
{
    class DbConnectionOrcl
    {
        public static string host = "localhost";
        public static string port = "1521";
        public static string service_name = "ORCLPDB";
        public static UserPrivilege userPrivilege = new UserPrivilege();
        public static OracleConnection CreateConnOrcl(string username, string password)
        {
            string connStr = $"User Id={username};Password={password};Data Source=" +
                       $"(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT={port})))" +
                       $"(CONNECT_DATA=(SERVICE_NAME={service_name})))";
    
            return new OracleConnection(connStr);
        }

        public static OracleConnection conn;
        //public static OracleConnection connAdmin = CreateConnOrcl("HotelCheckLogin", "login");


        public static DataTable ExecuteTable(string query)
        {
            DataTable dt = new();
            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                OracleDataAdapter adapter = new(query, conn);
                adapter.Fill(dt);
            }
            catch (OracleException ex)
            {
                if (ex.Number == 1935)
                    MessageBox.Show($"Invalid username", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (ex.Number == 1920)
                    MessageBox.Show($"Username already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if(ex.Number == 942 || ex.Number == 1031) 
                    MessageBox.Show("Error: You do not have permission to perform this operation. Please check your access rights.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show($"Error when insert user: {ex.Message} -- {ex.Number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public static List<String> ExecuteListString(string query)
        {
            List<string> listStr = new List<string>();
            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(query, conn);
                OracleDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    listStr.Add(reader[0].ToString());
                }
            }
            catch (OracleException ex)
            {
                if (ex.Number == 1935)
                    MessageBox.Show($"Invalid username", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (ex.Number == 1920)
                    MessageBox.Show($"Username already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (ex.Number == 942 || ex.Number == 1031)
                    MessageBox.Show("Error: You do not have permission to perform this operation. Please check your access rights.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show($"Error when insert user: {ex.Message} -- {ex.Number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conn.Close();
            }
            return listStr;
        }

        public static void ExecuteNonQuery(string sql, string successMessage = null)
        {
            OracleConnection conn = DbConnectionOrcl.conn;
            OracleCommand cmd = new(sql, conn);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                if (!string.IsNullOrEmpty(successMessage))
                    MessageBox.Show(successMessage, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (OracleException ex)
            {
                if (ex.Number == 1935)
                    MessageBox.Show($"Invalid username", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (ex.Number == 1920)
                    MessageBox.Show($"Username already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else if (ex.Number == 942 || ex.Number == 1031 || ex.Number == 1924)
                    MessageBox.Show("Error: You do not have permission to perform this operation. Please check your access rights.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show($"Error when insert user: {ex.Message} -- {ex.Number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// @param sql
        /// </summary>
        public void ThucThi(string sql)
        {
            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally { conn.Close(); }
        }

        /// <summary>
        /// @param sql
        /// </summary>
        public DataTable LayDanhSach(string sql)
        {
            DataTable dt = new DataTable();
            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                OracleDataAdapter adapter = new OracleDataAdapter(sql, conn);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }
            return dt;
        }

    }
}
