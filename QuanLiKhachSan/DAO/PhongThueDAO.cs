using QuanLiKhachSan.Class;
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
    class PhongThueDAO
    {
        DBConnection dbConnection = new DBConnection();

        public PhongThueDAO()
        {
        }

        public DataTable layDanhSach()
        {
            return dbConnection.LayDanhSach("Select* from View_Booking_Record");
        }
        /// <summary>
        /// @return
        /// </summary>
        public DataTable LayDanhSachPhongThue()
        {
            // TODO implement here
            return dbConnection.LayDanhSach("Select* from View_Booking_Record");
        }

        public DataTable LayDsHoSoDaCheckin()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = DBConnection.conn;
            try
            {
                conn.Open();
                String sql = "Select* from View_Confirmed_Booking_Record";
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
        public DataTable LayDsHoSoChuaCheckin()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = DBConnection.conn;
            try
            {
                conn.Open();
                String sql = "Select* from View_Pending_Booking_Record";
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
        public DataTable LayDsHoSoDaBiHuy()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = DBConnection.conn;
            try
            {
                conn.Open();
                String sql = "Select* from View_Canceled_Booking_Record";
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
        public DataTable LayDsHoSoDaDatCoc()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = DBConnection.conn;
            try
            {
                conn.Open();
                String sql = "Select* from View_Deposit_Booking_Record";
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
        public DataTable LayDsHoSoChuaDatCoc()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = DBConnection.conn;
            try
            {
                conn.Open();
                String sql = "Select* from View_No_Deposit_Booking_Record";
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
        public void Delete(int bookingRecordId)
        {
            SqlConnection conn = DBConnection.conn;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_deleteBookingRecord";
            cmd.Parameters.Add("@booking_record_id", SqlDbType.Int).Value = bookingRecordId;

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
        public void Update(int bookingRecordId, DateTime? expectedCheckinDate, DateTime? expectedCheckoutDate,
            DateTime? actualCheckinDate, DateTime? actualCheckoutDate, float deposit, float surcharge,
            string note, string status, int representativeId, int roomId)
        {
            SqlConnection conn = DBConnection.conn;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_updateBookingRecord";
            cmd.Parameters.AddWithValue("@booking_record_id", bookingRecordId);
            cmd.Parameters.AddWithValue("@expected_checkin_date", (object)expectedCheckinDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@expected_checkout_date", (object)expectedCheckoutDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@actual_checkin_date", (object)actualCheckinDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@actual_checkout_date", (object)actualCheckoutDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@deposit", deposit);
            cmd.Parameters.AddWithValue("@surcharge", surcharge);
            cmd.Parameters.AddWithValue("@note", note);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@representative_id", representativeId);
            cmd.Parameters.AddWithValue("@room_id", roomId);
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
        public void Insert(DateTime? expectedCheckinDate, DateTime? expectedCheckoutDate,
            DateTime? actualCheckinDate, DateTime? actualCheckoutDate, float deposit, float surcharge,
            string note, string status, int representativeId, int roomId)
        {
            SqlConnection conn = DBConnection.conn;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_insertBookingRecord";
            cmd.Parameters.AddWithValue("@expected_checkin_date", (object)expectedCheckinDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@expected_checkout_date", (object)expectedCheckoutDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@actual_checkin_date", (object)actualCheckinDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@actual_checkout_date", (object)actualCheckoutDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@deposit", deposit);
            cmd.Parameters.AddWithValue("@surcharge", surcharge);
            cmd.Parameters.AddWithValue("@note", note);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@representative_id", representativeId);
            cmd.Parameters.AddWithValue("@room_id", roomId);

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

        /// <summary>
        /// @param phongThue
        /// </summary>
        public void TaoPhongThue(PhongThue phongThue)
        {
            string sqlQuery = $@"
                DECLARE @expected_checkin_date DATETIME = '{((object)phongThue.ThoiGianCheckIn ?? DBNull.Value):yyyy-MM-dd}';
                DECLARE @expected_checkout_date DATETIME = '{((object)phongThue.ThoiGianCheckOut ?? DBNull.Value):yyyy-MM-dd}';
                DECLARE @actual_checkin_date DATETIME = '{((object)DateTime.Now ?? DBNull.Value):yyyy-MM-dd}';
                DECLARE @actual_checkout_date DATETIME = '{((object)DateTime.Now ?? DBNull.Value):yyyy-MM-dd}';
                DECLARE @deposit DECIMAL(18, 2) = {phongThue.TienCoc};
                DECLARE @surcharge DECIMAL(18, 2) = {phongThue.PhuPhi};
                DECLARE @note NVARCHAR(MAX) = N'{phongThue.GhiChu.Replace("'", "''")}';
                DECLARE @status NVARCHAR(50) = N'Chờ xác nhận';
                DECLARE @representative_id INT = {phongThue.MaKhachHang};
                DECLARE @room_id INT = {phongThue.MaPhong};

                EXEC proc_insertBookingRecord 
                    @expected_checkin_date = @expected_checkin_date,
                    @expected_checkout_date = @expected_checkout_date,
                    @actual_checkin_date = @actual_checkin_date,
                    @actual_checkout_date = @actual_checkout_date,
                    @deposit = @deposit,
                    @surcharge = @surcharge,
                    @note = @note,
                    @status = @status,
                    @representative_id = @representative_id,
                    @room_id = @room_id;
                ";

            dbConnection.ThucThi(sqlQuery);

        }

        public DataTable TimKiemHopDongTheoTenKhachHang(string customerName)
        {
            if (customerName == null || customerName == "")
            {
                return LayDsHoSoDaDatCoc();
            }
            else
            {
                string sql = "SELECT * FROM func_getBookingRecordByCustomerName(@customer_name)";
                DataTable dt = new DataTable();
                SqlConnection conn = DBConnection.conn;
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@customer_name", customerName);
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
        public DataTable TimKiemHopDongTheoTenPhong(string roomName)
        {
            if (roomName == null || roomName == "")
            {
                return LayDsHoSoDaDatCoc();
            }
            else
            {
                string sql = "SELECT * FROM func_getBookingRecordByRoomName(@room_name)";
                DataTable dt = new DataTable();
                SqlConnection conn = DBConnection.conn;
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@room_name", roomName);
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