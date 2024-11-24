using QuanLiKhachSan.DAO;
using System;
using System.Windows;
using System.Windows.Controls;
using QuanLiKhachSan.Class;

namespace QuanLiKhachSan
{
    /// <summary>
    /// Interaction logic for UcDoanhThu.xaml
    /// </summary>
    public partial class UcDoanhThu : UserControl
    {
        DoanhThuDao doanhThuDao = new DoanhThuDao();
        ThongKe thongKe = new ThongKe();
        public UcDoanhThu()
        {
            InitializeComponent();
            this.DataContext = thongKe;
            doanhThuDao.LayDoanhThu(thongKe);
        }

        private void dtpNgayBatDau_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            doanhThuDao.LayDoanhThu(thongKe);
            doanhThuDao.LayTongDoanhThu(thongKe);
        }

        private void dtgNgayKetThuc_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            doanhThuDao.LayDoanhThu(thongKe);
            doanhThuDao.LayTongDoanhThu(thongKe);    
        }

        private void btnMotNamQua_Click(object sender, RoutedEventArgs e)
        {
            thongKe.NgayBatDau = DateTime.Now.AddYears(-1);
            thongKe.NgayKetThuc = DateTime.Now;
        }

        private void btn6ThangQua_Click(object sender, RoutedEventArgs e)
        {
            thongKe.NgayBatDau = DateTime.Now.AddMonths(-6);
            thongKe.NgayKetThuc = DateTime.Now;
        }
    }
}
