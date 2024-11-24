using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLiKhachSan.DAO
{
    class BillDao
    {
        public DataTable LayDanhSach()
        {
            DataTable dt = new DataTable();
            string sql = "select* from View_Bill";
            SqlConnection conn = DBConnection.conn;
            try
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }
            return dt;
        }

        public void Them(float costs_incurred, string content_incurred, float total_cost, string payment_method, DateTime? pay_time, int booking_record_id, int employee_id)
        {
            SqlConnection conn = DBConnection.conn;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_add_new_bill"; 
            cmd.Parameters.Add("@costs_incurred", SqlDbType.Float).Value = costs_incurred;
            cmd.Parameters.Add("@content_incurred", SqlDbType.NVarChar).Value = content_incurred;
            cmd.Parameters.Add("@total_cost", SqlDbType.Float).Value = total_cost;
            cmd.Parameters.Add("@payment_method", SqlDbType.NVarChar).Value = payment_method;
            cmd.Parameters.Add("@pay_time", SqlDbType.DateTime).Value = pay_time;
            cmd.Parameters.Add("@booking_record_id", SqlDbType.Int).Value = booking_record_id;
            cmd.Parameters.Add("@employee_id", SqlDbType.Int).Value = employee_id;

            try
            {
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Thêm thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void Sua(int id, float costs_incurred, string content_incurred, float total_cost, string payment_method, DateTime? pay_time)
        {
            SqlConnection conn = DBConnection.conn;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_update_bill";
            cmd.Parameters.Add("@bill_id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@costs_incurred", SqlDbType.Float).Value = costs_incurred;
            cmd.Parameters.Add("@content_incurred", SqlDbType.NVarChar).Value = content_incurred;
            cmd.Parameters.Add("@total_cost", SqlDbType.Float).Value = total_cost;
            cmd.Parameters.Add("@payment_method", SqlDbType.NVarChar).Value = payment_method;
            cmd.Parameters.Add("@pay_time", SqlDbType.DateTime).Value = pay_time;

            try
            {
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Sửa thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void Xoa(int id)
        {
            SqlConnection conn = DBConnection.conn;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_delete_bill";
            cmd.Parameters.Add("@bill_id", SqlDbType.Int).Value = id;

            try
            {
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Xóa thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable TimKiemHoaDon(string searchString)
        {
            string sql = "SELECT * FROM func_search_bill(@string)";
            DataTable dt = new DataTable();
            SqlConnection conn = DBConnection.conn;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@string", searchString);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }

            return dt;
        }

        public DataTable TimKiemHoaDonTheoTrangThai(string searchString)
        {
            string sql = "SELECT * FROM func_search_bill_by_status(@string)";
            DataTable dt = new DataTable();
            SqlConnection conn = DBConnection.conn;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@string", searchString);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
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
