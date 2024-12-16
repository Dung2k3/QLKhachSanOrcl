using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuanLiKhachSan
{
    /// <summary>
    /// Interaction logic for fUngDung.xaml
    /// </summary>
    public partial class fUngDung2 : Window
    {
        //private FPhong ucPhong = new FPhong();
        //private UcNhanVien ucNhanVien = new UcNhanVien();
        //UcDichVu ucDichVu = new UcDichVu();
        //FKhachHang ucKhachHang = new FKhachHang();
        //UcThanhToan ucThanhToan = new UcThanhToan();
        //UcDoanhThu ucDoanhThu = new UcDoanhThu();
        private UcNhanVien2 ucNhanVien = new UcNhanVien2();
        private UcProfile ucProfile = new UcProfile();
        public fUngDung2()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnAnManHinh_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void toggleBtnKichCo_Click(object sender, RoutedEventArgs e)
        {
            if (toggleBtnKichCo.IsChecked == true)
            {
                var workingArea = SystemParameters.WorkArea;
                Left = workingArea.Left;
                Top = workingArea.Top;
                Width = workingArea.Width;
                Height = workingArea.Height;
            }
            else
            {
                Width = 1200;
                Height = 700;
                Left = (SystemParameters.PrimaryScreenWidth - Width) / 2;
                Top = (SystemParameters.PrimaryScreenHeight - Height) / 2;
            }
        }

        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void btnDangXuat_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn đăng xuất?", "xác nhận đăng xuất", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                fDangNhap dangNhap = new fDangNhap();
                dangNhap.Show();
                Close();
            }
            else
                btnDangXuat.IsChecked = false;
        }

        private void itemNhanVien_Selected(object sender, RoutedEventArgs e)
        {
            grManHinh.Children.Clear();
            ucNhanVien.LayDanhSach();
            grManHinh.Children.Add(ucNhanVien);
        }

        private void itemDichVu_Selected(object sender, RoutedEventArgs e)
        {
            grManHinh.Children.Clear();
            ucProfile.LayDanhSach();
            grManHinh.Children.Add(ucProfile);
        }

        private void itemThanhToan_Selected(object sender, RoutedEventArgs e)
        {
            //grManHinh.Children.Clear();
            //ucThanhToan.LayDanhSach();
            //grManHinh.Children.Add(ucThanhToan);
        }

        private void itemDoanhThu_Selected(object sender, RoutedEventArgs e)
        {
            //grManHinh.Children.Clear();
            //grManHinh.Children.Add(ucDoanhThu);
        }

        private void itemPhong_Selected(object sender, RoutedEventArgs e)
        {
            //grManHinh.Children.Clear();
            //ucPhong.LayDanhSach();
            //grManHinh.Children.Add(ucPhong);
        }

        private void itemKhachHang_Selected(object sender, RoutedEventArgs e)
        {
            //grManHinh.Children.Clear();
            //ucKhachHang.LayDanhSach();
            //grManHinh.Children.Add(ucKhachHang);
        }
    }
}
