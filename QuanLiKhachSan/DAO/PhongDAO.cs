using QuanLiKhachSan.Class;
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
    class PhongDAO
    {
        DBConnection dBConnection = new DBConnection();

        public PhongDAO()
        {
        }
        public DataTable LayDanhSach()
        {

            return dBConnection.LayDanhSach("select * from View_Room");
        }
        /*
        public void Them(string tenPhong, int sucChua, string trangThai, string moTa, string anh, int loaiPhong)
        {
            SqlConnection conn = DBConnection.conn;
            string sql = $"insert into Room(room_name, room_capacity, room_status, room_description, room_image, room_type_id) " +
                $"values('{tenPhong}', {sucChua},'{trangThai}',N'{moTa}',{anh},{loaiPhong})";
            MessageBox.Show("OK");
            try
            {
                conn.Open();
                SqlCommand cm = new SqlCommand(sql, conn);
                if (cm.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("thanh cong");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message );
            }
            finally
            {
                conn.Close();
            }
        }
        */
        public DataTable LayDanhSachTenPhong()
        {
            return dBConnection.LayDanhSach("select* from View_Room_Name");
        }
        public void Them(string roomName, int roomCapacity, string roomStatus, string roomDescription,
            byte[] roomImage, int roomTypeId)
        {
            SqlConnection conn = DBConnection.conn;
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "proc_add_room";
                cmd.Parameters.Add("@room_name", SqlDbType.NVarChar).Value = roomName;
                cmd.Parameters.Add("@room_capacity", SqlDbType.Int).Value = roomCapacity;
                cmd.Parameters.Add("@room_status", SqlDbType.NVarChar).Value = roomStatus;
                cmd.Parameters.Add("@room_description", SqlDbType.NVarChar).Value = roomDescription;
                cmd.Parameters.Add("@room_image", SqlDbType.VarBinary).Value = roomImage;
                cmd.Parameters.Add("@room_type_id", SqlDbType.Int).Value = roomTypeId;
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
        public void Sua(int roomId, string roomName, int roomCapacity, string roomStatus, string roomDescription,
            byte[] roomImage, int roomTypeId)
        {
            SqlConnection conn = DBConnection.conn;
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "proc_update_room";
                cmd.Parameters.Add("@room_id", SqlDbType.Int).Value = roomId;
                cmd.Parameters.Add("@room_name", SqlDbType.NVarChar).Value = roomName;
                cmd.Parameters.Add("@room_capacity", SqlDbType.Int).Value = roomCapacity;
                cmd.Parameters.Add("@room_status", SqlDbType.NVarChar).Value = roomStatus;
                cmd.Parameters.Add("@room_description", SqlDbType.NVarChar).Value = roomDescription;
                cmd.Parameters.Add("@room_image", SqlDbType.VarBinary).Value = roomImage;
                cmd.Parameters.Add("@room_type_id", SqlDbType.Int).Value = roomTypeId;
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
        public void Xoa(int roomId)
        {
            SqlConnection conn = DBConnection.conn;
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "proc_delete_room";
                cmd.Parameters.Add("@room_id", SqlDbType.Int).Value = roomId;
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

        public Phong GetPhongById(int roomId)
        {
            Phong phong = null;
            string sql = "select * from Room Where room_id = @roomId";
            SqlConnection conn = DBConnection.conn;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@roomId", roomId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    phong = new Phong();
                    phong.MaPhong = Convert.ToInt32(reader["room_id"]);
                    phong.TenPhong = reader["room_name"].ToString();
                    phong.TrangThai = reader["room_status"].ToString();
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
            return phong;
        }

        /// <summary>
        /// @return
        /// </summary>
        public DataTable LayDanhSachPhong()
        {
            // TODO implement here
            return null;
        }

        /// <summary>
        /// @param phong
        /// </summary>
        public void ThemPhong(Phong phong)
        {
            // TODO implement here
        }

        /// <summary>
        /// @param phong
        /// </summary>
        public void CapNhat(Phong phong)
        {
            // TODO implement here
        }


    }
}
