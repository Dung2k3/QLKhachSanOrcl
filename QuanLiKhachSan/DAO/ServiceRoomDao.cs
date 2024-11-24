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
    class ServiceRoomDao
    {
        DBConnection dBConnection = new DBConnection();
        public DataTable LayDanhSach()
        {
            return dBConnection.LayDanhSach("select * from View_Service");
           
        }
        public DataTable LayDanhSachTenDichVu()
        {
            return dBConnection.LayDanhSach("select* from View_Service_Name");
        }

        public void Them(string serviceRoomName, float serviceRoomPrice, float discountService)
        {
            SqlConnection conn = DBConnection.conn;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_insertServiceRoom";
            cmd.Parameters.Add("@service_room_name", SqlDbType.NVarChar).Value = serviceRoomName;
            cmd.Parameters.Add("@service_room_price", SqlDbType.Float).Value = serviceRoomPrice;
            cmd.Parameters.Add("@discount_service", SqlDbType.Float).Value = discountService;
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

        public void Sua(int id, string serviceRoomName,bool serviceRoomStatus, float serviceRoomPrice, float discountService)
        {
            SqlConnection conn = DBConnection.conn;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_updateServiceRoom";
            cmd.Parameters.Add("@service_room_id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@service_room_status", SqlDbType.Bit).Value = serviceRoomStatus;
            cmd.Parameters.Add("@service_room_name", SqlDbType.NVarChar).Value = serviceRoomName;
            cmd.Parameters.Add("@service_room_price", SqlDbType.Float).Value = serviceRoomPrice;
            cmd.Parameters.Add("@discount_service", SqlDbType.Float).Value = discountService;

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
            cmd.CommandText = "proc_deleteServiceRoom";
            cmd.Parameters.Add("@service_room_id", SqlDbType.Int).Value = id;

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
        public DataTable TimKiemTenDV(string nameService)
        {
            if (nameService == null || nameService == "")
            {
                return LayDanhSach();
            }
            else
            {
                string sql = "SELECT * FROM func_searchByServiceName (@service_room_name)";
                DataTable dt = new DataTable();
                SqlConnection conn = DBConnection.conn;
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@service_room_name", nameService);
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
        public DataTable TimKiemTheoKhoangGia(float fromPrice,float toPrice)
        {
            if (fromPrice == null || toPrice == null)
            {
                return LayDanhSach();
            }
            else
            {
                string sql = "SELECT * FROM func_searchInPriceRange(@service_room_price1, @service_room_price2)";
                DataTable dt = new DataTable();
                SqlConnection conn = DBConnection.conn;
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@service_room_price1", fromPrice);
                    cmd.Parameters.AddWithValue("@service_room_price2", toPrice);
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
