using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using QuanLiKhachSan.Class;

namespace QuanLiKhachSan.DAO
{
    class DoanhThuDao
    {
        public void LayDoanhThu(ThongKe thongKe)
        {
            string sql = "select* from f_Calculate_Revenue(@StartDay, @EndDay)";
            SqlConnection conn = DBConnection.conn;
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@StartDay", thongKe.NgayBatDau);
                cmd.Parameters.AddWithValue("@EndDay", thongKe.NgayKetThuc);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable tb = new DataTable();
                adapter.Fill(tb);

                List<double> listDoanhThu = new List<double>();
                DateTime current = thongKe.NgayBatDau;
                while (current <= thongKe.NgayKetThuc)
                {
                    listDoanhThu.Add(tb.AsEnumerable().Where(x => x.Field<int>("Month") == current.Month && x.Field<int>("Year") == current.Year)
                                                     .Select(x => x.Field<double>("ToTal")).DefaultIfEmpty(0).FirstOrDefault());
                    current = current.AddMonths(1);
                    if (current.Year == thongKe.NgayKetThuc.Year && current.Month == thongKe.NgayKetThuc.Month)
                        current = thongKe.NgayKetThuc;
                }

                List<string> labels = new List<string>();
                current = thongKe.NgayBatDau;
                while (current <= thongKe.NgayKetThuc)
                {
                    labels.Add(current.ToString("MM/yyyy"));
                    current = current.AddMonths(1);
                    if (current.Year == thongKe.NgayKetThuc.Year && current.Month == thongKe.NgayKetThuc.Month)
                        current = thongKe.NgayKetThuc;
                }
                thongKe.Labels = labels;
                SeriesCollection seriesCollection = new SeriesCollection()
                {
                     new LineSeries() { Title = "Doanh thu", Values = listDoanhThu.AsChartValues()}
                };
                thongKe.SeriesCollection = seriesCollection;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
        public void LayTongDoanhThu(ThongKe thongKe)
        {
            string sql = "SELECT dbo.f_Calculate_Total_Revenue (@StartDay, @EndDay)";
            SqlConnection conn = DBConnection.conn;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                DataTable dt = new DataTable();
                cmd.Parameters.AddWithValue("@StartDay", thongKe.NgayBatDau);
                cmd.Parameters.AddWithValue("@EndDay", thongKe.NgayKetThuc);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                if(dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                {
                    thongKe.TongDoanhThu = (double)dt.Rows[0][0];
                }
            }
            catch(Exception ex)
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