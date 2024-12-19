using QuanLiKhachSan.DAO;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace QuanLiKhachSan
{
    /// <summary>
    /// Interaction logic for UcUserInfo.xaml
    /// </summary>
    public partial class UcUserInfo : UserControl
    {
        UserDetailDAO userDetailDao = new UserDetailDAO();

        public UcUserInfo()
        {
            InitializeComponent();
            LayDanhSach();
        }

        public void LayDanhSach()
        {
            dtgUserDetails.ItemsSource = userDetailDao.GetLists().DefaultView;
        }

        private void btnInforUser_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)dtgUserDetails.SelectedValue;
            try
            {
                txtUsername.Text = drv["USERNAME"].ToString();
                txtFullName.Text = drv["FULL_NAME"].ToString();
                txtAddress.Text = drv["ADDRESS"].ToString();
                txtPhoneNumber.Text = drv["PHONE_NUMBER"].ToString();
                txtEmail.Text = drv["EMAIL"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)dtgUserDetails.SelectedValue;
            try
            {
                string username = drv["USERNAME"].ToString();
                userDetailDao.DeleteUserDetail(username);
                LayDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = txtUsername.Text;
                string fullName = txtFullName.Text;
                string address = txtAddress.Text;
                string phoneNumber = txtPhoneNumber.Text;
                string email = txtEmail.Text;
                userDetailDao.InsertUserDetail(username, fullName, address, phoneNumber, email);
                LayDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = txtUsername.Text;
                string fullName = txtFullName.Text;
                string address = txtAddress.Text;
                string phoneNumber = txtPhoneNumber.Text;
                string email = txtEmail.Text;
                userDetailDao.UpdateUserDetail(username, fullName, address, phoneNumber, email);
                LayDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}