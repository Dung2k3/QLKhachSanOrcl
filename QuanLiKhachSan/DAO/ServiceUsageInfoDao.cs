using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLiKhachSan.DAO
{
    class ServiceUsageInfoDao
    {
        public DataTable LayDanhSach()
        {
            string sql = "select* from View_Service_Usage_Info";
            DataTable dt = new DataTable();
            SqlConnection conn = DBConnection.conn;
            try
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sql,conn);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);    
            }
            finally { conn.Close(); }
            return dt;
        }
        public void Them(int numberOfService, float totalFee, int bookingRecordId, int serviceRoomId)
        {
            SqlConnection conn = DBConnection.conn;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_insertServiceUsageInfor";
            cmd.Parameters.Add("@number_of_service", SqlDbType.Int).Value = numberOfService;
            cmd.Parameters.Add("@total_fee", SqlDbType.Float).Value = totalFee;
            cmd.Parameters.Add("@booking_record_id", SqlDbType.Int).Value = bookingRecordId;
            cmd.Parameters.Add("@service_room_id", SqlDbType.Int).Value = serviceRoomId;

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
        public void Sua(int id, int numberOfService, float totalFee, int bookingRecordId, int serviceRoomId)
        {
            SqlConnection conn = DBConnection.conn;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_updateServiceUsageInfor";
            cmd.Parameters.Add("@service_usage_infor_id", SqlDbType.Int).Value=id;
            cmd.Parameters.Add("@number_of_service", SqlDbType.Int).Value = numberOfService;
            cmd.Parameters.Add("@total_fee", SqlDbType.Float).Value = totalFee;
            cmd.Parameters.Add("@booking_record_id", SqlDbType.Int).Value = bookingRecordId;
            cmd.Parameters.Add("@service_room_id", SqlDbType.Int).Value = serviceRoomId;

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
            cmd.CommandText = "proc_deleteServiceUsageInfor";
            cmd.Parameters.Add("@service_usage_infor_id", SqlDbType.Int).Value = id;

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
        public DataTable TimKiemThangSuDung(string monthUse)
        {
            if (monthUse == null || monthUse == "")
            {
                return LayDanhSach();
            }
            else
            {
                string sql = "SELECT * FROM func_searchServiceUsageInforByMonth(@month)";
                DataTable dt = new DataTable();
                SqlConnection conn = DBConnection.conn;
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@month", monthUse);
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
        public DataTable TimKiemTenKH(string nameUse)
        {
            string sql = "SELECT * FROM func_searchServiceUsageInforByCustomerName(@customer_name)";
            if (nameUse == null || nameUse == "")
            {
                return LayDanhSach();
            }
            else
            {
                DataTable dt = new DataTable();
                SqlConnection conn = DBConnection.conn;
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@customer_name", nameUse);
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
}
