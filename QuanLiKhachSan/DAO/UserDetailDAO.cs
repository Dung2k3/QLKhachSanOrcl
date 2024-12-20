using System;
using System.Data;
using System.Data.Common;
using System.Transactions;
using System.Windows;
using Oracle.ManagedDataAccess.Client;

namespace QuanLiKhachSan.DAO
{
    public class UserDetailDAO
    {
        public void InsertUserDetail(string username, string fullName, string address, string phoneNumber, string email)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Username and Full Name cannot be null or empty.");

            string query = @"INSERT INTO SYS.USER_DETAILS (USERNAME, FULL_NAME, ADDRESS, PHONE_NUMBER, EMAIL)
                            VALUES (:username, :fullName, :address, :phoneNumber, :email)";

            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                OracleCommand cmd = new(query, conn);
                cmd.Parameters.Add(new OracleParameter("username", username));
                cmd.Parameters.Add(new OracleParameter("fullName", fullName));
                cmd.Parameters.Add(new OracleParameter("address", address));
                cmd.Parameters.Add(new OracleParameter("phoneNumber", phoneNumber));
                cmd.Parameters.Add(new OracleParameter("email", email));
                cmd.ExecuteNonQuery();
                OracleTransaction transaction = conn.BeginTransaction();
                transaction.Commit();
                MessageBox.Show("User detail inserted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public void UpdateUserDetail(string username, string fullName, string address, string phoneNumber, string email)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be null or empty.");

            string query = @"UPDATE SYS.USER_DETAILS
                            SET FULL_NAME = :fullName,
                                ADDRESS = :address,
                                PHONE_NUMBER = :phoneNumber,
                                EMAIL = :email,
                                UPDATED_DATE = SYSDATE
                            WHERE USERNAME = '" + username +"'";
            MessageBox.Show(username);
            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                OracleTransaction transaction = conn.BeginTransaction();
                OracleCommand cmd = new(query, conn);
                cmd.Parameters.Add(new OracleParameter("fullName", fullName));
                cmd.Parameters.Add(new OracleParameter("address", address));
                cmd.Parameters.Add(new OracleParameter("phoneNumber", phoneNumber));
                cmd.Parameters.Add(new OracleParameter("email", email));
                cmd.Transaction = transaction;
                if(cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("User detail updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("User detail updated fail.", "Fail", MessageBoxButton.OK, MessageBoxImage.Information);
                OracleCommand cmd2 = new("commit", conn);
                cmd2.ExecuteNonQuery();
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

        public void DeleteUserDetail(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be null or empty.");

            string query = "DELETE FROM SYS.USER_DETAILS WHERE USERNAME = :username";

            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                OracleTransaction transaction = conn.BeginTransaction();
                OracleCommand cmd = new(query, conn);
                cmd.Parameters.Add(new OracleParameter("username", username));
                cmd.ExecuteNonQuery();
                cmd.Transaction = transaction;
                transaction.Commit();
                MessageBox.Show("User detail deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
        public DataTable GetLists()
        {
            string query = "SELECT * FROM SYS.USER_DETAILS";

            return ExecuteQuery(query);
        }
        private DataTable ExecuteQuery(string query)
        {
            DataTable dt = new();
            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                OracleDataAdapter adapter = new(query, conn);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing query: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
    }
}
