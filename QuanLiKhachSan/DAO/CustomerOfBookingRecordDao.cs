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
    class CustomerOfBookingRecordDao
    {
        DBConnection dBConnection = new DBConnection();
        public DataTable LayDanhSach()
        {
            return dBConnection.LayDanhSach("Select* from View_Customer_Of_Booking_Record");
            
        }
        public void Them(int customerId, int bookingRecordId)
        {
            SqlConnection conn = DBConnection.conn;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_add_customer_of_booking_record";
            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customerId;
            cmd.Parameters.Add("@booking_record_id", SqlDbType.Int).Value = bookingRecordId;

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

        public void Xoa(int customerId, int bookingRecordId)
        {
            SqlConnection conn = DBConnection.conn;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_delete_customer_of_booking_record";
            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customerId;
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
    }
}
