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
    public partial class UcNhanVien2 : UserControl
    {
        UserDAO userDao = new UserDAO();
        TableSpaceDAO tablespaceDao = new TableSpaceDAO();
        ProfileDAO profileDao = new ProfileDAO();
        public UcNhanVien2()
        {
            InitializeComponent();

        }

        public void LayDanhSach()
        {
            UserPrivilege up = DbConnectionOrcl.userPrivilege;

            if (up.SelectUser)
            {
                dtgDanhSachTaiKhoan.ItemsSource = userDao.LayDanhSach().DefaultView;
            }
            if (up.SelectQuota)
            {
                dtgDanhSachSdtCuaNhanVien.ItemsSource = tablespaceDao.LayDanhSach().DefaultView;
            }
            cbProfile.ItemsSource = profileDao.ListProfile();
            cbTablespace.ItemsSource = tablespaceDao.ListDefaultTableSpace();
            cbTempTablespace.SelectedIndex = 0;
            cbAccountStatus.SelectedIndex = 0;
            cbAccountStatus.ItemsSource = new[] { "OPEN", "LOCKED" }.ToList();
            txbUsername.Text = "";
            txbPassword.Text = "";
            cbTempTablespace.ItemsSource = tablespaceDao.ListTempTableSpace();
            cbDefaultTablespace.ItemsSource = tablespaceDao.ListDefaultTableSpace();
            cbDefaultTablespace.SelectedIndex = 0;
            cbProfile.SelectedIndex = 0;
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
                if (DbConnectionOrcl.userPrivilege.SelectQuota)
                {
                    dtgDanhSachSdtCuaNhanVien.ItemsSource = tablespaceDao.LayDanhSachByUsername(txbFilterQuota.Text).DefaultView;
                }
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
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete user {username}?", "Confirm delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                userDao.Delete(username);
                LayDanhSach();
            }
        }

        private void btnXoaUser2_Click(object sender, RoutedEventArgs e)
        {
            if (txbUsername.Text.Equals(""))
            {
                MessageBox.Show("Invalid Username", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string username = txbUsername.Text;
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete user {username}?", "Confirm delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                userDao.Delete(username);
                LayDanhSach();
            }
        }

        private void btnThemQuota_Click(object sender, RoutedEventArgs e)
        {
            if (txbUsername2.Text.Equals(""))
            {
                MessageBox.Show("Invalid Username", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (cbTablespace.SelectedValue == null)
            {
                MessageBox.Show("Invalid Tablespace", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Quota quota = new Quota();
            quota.Username = txbUsername2.Text;
            quota.TablespaceName = cbTablespace.SelectedValue.ToString();
            quota.MaxQuota = txbQuota.Text.Equals("") ? -1 : int.Parse(txbQuota.Text);
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
    }
}
