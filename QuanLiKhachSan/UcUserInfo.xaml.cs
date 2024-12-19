using QuanLiKhachSan.DAO;
using QuanLiKhachSan.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLiKhachSan
{
    /// <summary>
    /// Interaction logic for UcNhanVien.xaml
    /// </summary>
    public partial class UcUserInfo : UserControl
    {
        UserDAO userDao = new UserDAO();
        NhanVienDAO employeeDao = new NhanVienDAO();
        PhoneNumberOfEmployeeDao phoneDao = new PhoneNumberOfEmployeeDao();
        TableSpaceDAO tablespaceDao = new TableSpaceDAO();
        ProfileDAO profileDao = new ProfileDAO();
        public UcUserInfo()
        {
            InitializeComponent();

        }

        public void LayDanhSach()
        {
            //dtgDanhSachNhanVien.ItemsSource = this.LayDanhSachNhanVien().DefaultView;
            dtgDanhSachTaiKhoan.ItemsSource = userDao.LayDanhSach().DefaultView;
            dtgDanhSachSdtCuaNhanVien.ItemsSource = tablespaceDao.LayDanhSach().DefaultView;
            //DataTable tb = employeeDao.LayDanhSachTenNhanVien();
            //List<string> list = tb.AsEnumerable()
            //                      .Select(x => x.Field<int>("employee_id").ToString() + "|" + x.Field<string>("employee_name"))
            //                      .ToList<string>();
            cbTempTablespace.ItemsSource = tablespaceDao.ListTempTableSpace();
            cbDefaultTablespace.ItemsSource = tablespaceDao.ListDefaultTableSpace();
            cbProfile.ItemsSource = profileDao.ListProfile();
            cbAccountStatus.ItemsSource = new[] { "OPEN", "LOCKED" }.ToList();
            cbTablespace.ItemsSource = tablespaceDao.ListDefaultTableSpace();
            cbTempTablespace.SelectedIndex = 0;
            cbDefaultTablespace.SelectedIndex = 0;
            cbProfile.SelectedIndex = 0;
            cbAccountStatus.SelectedIndex = 0;
            txbUsername.Text = "";
            txbPassword.Text = "";
        }

        /// <summary>
        /// @return
        /// </summary>
        public DataTable LayDanhSachNhanVien()
        {
            // TODO implement here
            return employeeDao.LayDanhSach();

        }

        private void btnThongTinNhanVien_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)dtgDanhSachNhanVien.SelectedValue;
            try
            {
                lbMaNhanVien.Content = drv["employee_id"].ToString();
                txtTenNhanVien.Text = drv["employee_name"].ToString();
                cbGioiTinh.SelectedValue = drv["gender"].ToString();
                dtpNgaySinh.SelectedDate = DateTime.Parse(drv["birthday"].ToString());
                txtIdentifyCard.Text = drv["identify_card"].ToString();
                txtDiaChi.Text = drv["address"].ToString();
                txtEmail.Text = drv["email"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnXoaNhanVien_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)dtgDanhSachNhanVien.SelectedValue;
            try
            {
                int employeeId = int.Parse(drv["employee_id"].ToString());
                employeeDao.Delete(employeeId);
                LayDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnThemNhanVien_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string employeeName = txtTenNhanVien.Text;
                string gender = cbGioiTinh.Text;
                DateTime? birthday = dtpNgaySinh.SelectedDate;
                string identifyCard = txtIdentifyCard.Text;
                string address = txtDiaChi.Text;
                string email = txtEmail.Text;
                employeeDao.Insert(employeeName, gender, birthday, identifyCard, address, email);
                LayDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSuaNhanVien_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int employeeId = int.Parse((string)lbMaNhanVien.Content);
                string employeeName = txtTenNhanVien.Text;
                string gender = cbGioiTinh.Text;
                DateTime? birthday = dtpNgaySinh.SelectedDate;
                string identifyCard = txtIdentifyCard.Text;
                string address = txtDiaChi.Text;
                string email = txtEmail.Text;
                employeeDao.Update(employeeId, employeeName, gender, birthday, identifyCard, address, email);
                LayDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSuaUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User user = new User();
                user.Username = txbUsername.Text;
                user.DefaultTablespace = cbDefaultTablespace.SelectedValue.ToString();
                user.TemporaryTablespace = cbTempTablespace.SelectedValue.ToString();
                user.Profile = cbProfile.SelectedValue.ToString();
                user.AccountStatus = cbAccountStatus.SelectedValue.ToString();
                user.Password = txbPassword.Text;
                userDao.Update(user);
                LayDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnThemUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User user = new User();
                user.Username = txbUsername.Text;
                user.DefaultTablespace = cbDefaultTablespace.SelectedValue.ToString();
                user.TemporaryTablespace = cbTempTablespace.SelectedValue.ToString();
                user.Profile = cbProfile.SelectedValue.ToString();
                user.AccountStatus = cbAccountStatus.SelectedValue.ToString();
                user.Password = txbPassword.Text;
                userDao.Insert(user);
                LayDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnThongTinUser_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)dtgDanhSachTaiKhoan.SelectedValue;
            try
            {
                txbUsername.Text = drv["USERNAME"].ToString();

                cbTempTablespace.SelectedValue = drv["TEMPORARY_TABLESPACE"];
                cbDefaultTablespace.SelectedValue = drv["DEFAULT_TABLESPACE"];
                cbProfile.SelectedValue = drv["PROFILE"];
                cbAccountStatus.SelectedValue = drv["ACCOUNT_STATUS"];
                txbFilterQuota.Text = drv["USERNAME"].ToString();
                dtgDanhSachSdtCuaNhanVien.ItemsSource = tablespaceDao.LayDanhSachByUsername(txbFilterQuota.Text).DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnXoaUser_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)dtgDanhSachTaiKhoan.SelectedValue;
            string username = drv["USERNAME"].ToString();
            MessageBoxResult result = MessageBox.Show($"Bạn có chắc muốn xóa user {username}?", "Xác nhận xóa", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                userDao.Delete(username);
                LayDanhSach();
            }
        }

        private void btnThemQuota_Click(object sender, RoutedEventArgs e)
        {

                Quota quota = new Quota();
                quota.Username = txbUsername2.Text;
                quota.TablespaceName = cbTablespace.SelectedValue.ToString();
                quota.MaxQuota = txbQuota.Text.Equals("")? -1: int.Parse(txbQuota.Text);
                tablespaceDao.Update(quota);
                LayDanhSach();

        }

        private void btnThongTinQuota_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)dtgDanhSachSdtCuaNhanVien.SelectedValue;
            try
            {
                txbUsername2.Text = drv["USERNAME"].ToString();
                cbTablespace.SelectedValue = drv["TABLESPACE_NAME"];
                txbQuota.Text = drv["MAX_QUOTA_MB"].ToString()== "Unlimited" ? "": drv["MAX_QUOTA_MB"].ToString();
                //txtSDT.Text = drv["phone_number"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnXoaQuota_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)dtgDanhSachSdtCuaNhanVien.SelectedValue;
            Quota quota = new Quota();
            quota.Username = drv["USERNAME"].ToString();
            quota.TablespaceName = drv["TABLESPACE_NAME"].ToString();
            quota.MaxQuota = 0;
            tablespaceDao.Update(quota);
            LayDanhSach();
    
        }

        private void btnTimKiemTheoTenNhanVien_Click(object sender, RoutedEventArgs e)
        {
            string employeeName = txtLocTheoTenNhanVien.Text;
            dtgDanhSachNhanVien.ItemsSource = employeeDao.TimKiemTheoHoTen(employeeName).DefaultView;
        }

        private void btnFilterQuota_Click(object sender, RoutedEventArgs e)
        {
            string username = txbFilterQuota.Text;
            dtgDanhSachSdtCuaNhanVien.ItemsSource = tablespaceDao.LayDanhSachLikeUsername(username).DefaultView;

        }
        private void btnFilterUser_Click(object sender, RoutedEventArgs e)
        {
            string username = txbFilterUser.Text;
            dtgDanhSachTaiKhoan.ItemsSource = userDao.LayDanhSachLikeUsername(username).DefaultView;

        }

        /// <summary>
        /// @return
        /// </summary>
        public Boolean KiemTraTen()
        {
            // TODO implement here
            return false;
        }

        /// <summary>
        /// @return
        /// </summary>
        public Boolean KiemTraNgaySinh()
        {
            // TODO implement here
            return false;
        }

        /// <summary>
        /// @return
        /// </summary>
        public Boolean KiemTraGioiTinh()
        {
            // TODO implement here
            return false;
        }

        /// <summary>
        /// @return
        /// </summary>
        public Boolean KiemTraDiaChi()
        {
            // TODO implement here
            return false;
        }

        /// <summary>
        /// @return
        /// </summary>
        public Boolean KiemTraCCCD()
        {
            // TODO implement here
            return false;
        }

        /// <summary>
        /// @return
        /// </summary>
        public Boolean KiemTraEmail()
        {
            // TODO implement here
            return false;
        }
    }
}