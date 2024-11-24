using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLiKhachSan.DAO
{
    class RoomTypeDao
    {
        DBConnection dBConnection = new DBConnection();

        public DataTable LayDanhSach()
        {
            return dBConnection.LayDanhSach("Select * from View_Room_Type");
        }

        public void Them(string room_type_name, float price, float discount_room)
        {
            SqlConnection conn = DBConnection.conn;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_add_room_type";
            cmd.Parameters.Add("@room_type_name", SqlDbType.NVarChar).Value = room_type_name;
            cmd.Parameters.Add("@price", SqlDbType.Float).Value = price;
            cmd.Parameters.Add("@discount_room", SqlDbType.Float).Value = discount_room;

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
        public void Sua(int id, string room_type_name, float price, float discount_room)
        {
            SqlConnection conn = DBConnection.conn;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_update_room_type";
            cmd.Parameters.Add("@room_type_id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@room_type_name", SqlDbType.NVarChar).Value = room_type_name;
            cmd.Parameters.Add("@price", SqlDbType.Float).Value = price;
            cmd.Parameters.Add("@discount_room", SqlDbType.Float).Value = discount_room;

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
            cmd.CommandText = "proc_delete_room_type";
            cmd.Parameters.Add("@room_type_id", SqlDbType.Int).Value = id;

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
        public DataTable TimKiemThongTinLoaiPhong(string information)
        {
            if (information == null || information == "")
            {
                return LayDanhSach();
            }
            else
            {
                string sql = "SELECT * FROM func_search_room_type(@string)";
                DataTable dt = new DataTable();
                SqlConnection conn = DBConnection.conn;
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@string", information);
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
        public DataTable TimKiemTheoKhoangGia(float fromPrice, float toPrice)
        {
            if (fromPrice == null || toPrice == null)
            {
                return LayDanhSach();
            }
            else
            {
                string sql = "SELECT * FROM func_search_room_type_by_price (@fromprice, @toprice)";
                DataTable dt = new DataTable();
                SqlConnection conn = DBConnection.conn;
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@fromprice", fromPrice);
                    cmd.Parameters.AddWithValue("@toprice", toPrice);
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