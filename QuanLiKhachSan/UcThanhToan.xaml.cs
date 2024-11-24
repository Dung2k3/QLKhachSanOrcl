using QuanLiKhachSan.DAO;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace QuanLiKhachSan
{
    /// <summary>
    /// Interaction logic for UcThanhToan.xaml
    /// </summary>
    public partial class UcThanhToan : UserControl
    {
        private BillDao billDao = new BillDao();
        public UcThanhToan()
        {
            InitializeComponent();
        }

        public void LayDanhSach()
        {
            dtgDanhSach.ItemsSource = billDao.LayDanhSach().DefaultView;
        }

        private void btnThemBill_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                billDao.Them(float.Parse(txtChiPhiPhatSinh.Text), txtNoiDungPhatSinh.Text, float.Parse(txtTongPhi.Text), ((string)cbHinhThucThanhToan.SelectedValue).Split("|")[0], dtpNgayThanhToan.SelectedDate, int.Parse(txtMaDatPhong.Text), int.Parse(txtMaNhanVien.Text));
                LayDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSuaBill_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int maHoaDon = int.Parse((string)lbMaBill.Content);
                billDao.Sua(maHoaDon, float.Parse(txtChiPhiPhatSinh.Text), txtNoiDungPhatSinh.Text, float.Parse(txtTongPhi.Text), ((string)cbHinhThucThanhToan.SelectedValue).Split("|")[0], dtpNgayThanhToan.SelectedDate);
                LayDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnThongTinBill_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)dtgDanhSach.SelectedValue;
            try
            {
                lbMaBill.Content = drv["bill_id"].ToString();
                txtChiPhiPhatSinh.Text = drv["costs_incurred"].ToString();
                txtNoiDungPhatSinh.Text = drv["content_incurred"].ToString();
                txtTongPhi.Text = drv["total_cost"].ToString();
                if (drv["created_date"] != DBNull.Value)
                {
                    dtpNgayTao.SelectedDate = DateTime.Parse(drv["created_date"].ToString());
                }
                else
                {
                    dtpNgayTao.SelectedDate = null;
                }
                cbHinhThucThanhToan.SelectedValue = drv["payment_method"].ToString();
                if (drv["paytime"] != DBNull.Value)
                {
                    dtpNgayThanhToan.SelectedDate = DateTime.Parse(drv["paytime"].ToString());
                }
                else
                {
                    dtpNgayThanhToan.SelectedDate = null;
                }
                txtMaDatPhong.Text = drv["booking_record_id"].ToString();
                txtMaNhanVien.Text = drv["employee_id"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnXoaBill_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)dtgDanhSach.SelectedValue;
            try
            {
                billDao.Xoa((int)drv["bill_id"]);
                LayDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectedValue = comboBox.SelectedValue.ToString();
            dtgDanhSach.ItemsSource = billDao.TimKiemHoaDonTheoTrangThai(selectedValue).DefaultView;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = ((TextBox)sender).Text;
            dtgDanhSach.ItemsSource = billDao.TimKiemHoaDon(searchText).DefaultView;
        }
    }
}
