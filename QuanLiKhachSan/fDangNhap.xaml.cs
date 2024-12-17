using QuanLiKhachSan.Class;
using QuanLiKhachSan.DAO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiKhachSan
{
    /// <summary>
    /// Interaction logic for fDangNhap.xaml
    /// </summary>
    public partial class fDangNhap : Window
    {
        TaiKhoan taiKhoan = new TaiKhoan();
        AccountDao accountDao = new AccountDao();
        public fDangNhap()
        {
            InitializeComponent();
            DataContext = taiKhoan;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnTat_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAnManHinh_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnDangNhap_Click(object sender, RoutedEventArgs e)
        {
            DangNhap();
        }

        private void pswMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                DangNhap();
        }

        private void txtTenTaiKhoan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                DangNhap();
        }
        
        //Hàm đăng nhập từ việc kiểm tra tài khoản
        private void DangNhap()
        {
            //taiKhoan.TenTaiKhoan = "adminuser";
            //taiKhoan.MatKhau = "admin";
            taiKhoan.TenTaiKhoan = "lbacsys";
            taiKhoan.MatKhau = "lbacsys";
            bool isSuccess = accountDao.Login(taiKhoan.TenTaiKhoan, taiKhoan.MatKhau);
            if(isSuccess)
            {
                fUngDung2 ungDung = new fUngDung2();
                ungDung.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Tên tài khoản hoặc mật khẩu không đúng");
            }
        }

        private void pswMatKhau_PasswordChanged(object sender, RoutedEventArgs e)
        {
            taiKhoan.MatKhau = pswMatKhau.Password;
        }

        private void txtMatKhau_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtMatKhau.Visibility == Visibility.Visible)
                pswMatKhau.Password = taiKhoan.MatKhau;
        }
    }
}
