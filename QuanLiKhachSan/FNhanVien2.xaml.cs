using QuanLiKhachSan.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        NhanVienDAO employeeDao = new NhanVienDAO();
        PhoneNumberOfEmployeeDao phoneDao = new PhoneNumberOfEmployeeDao();
        TableSpaceDAO tablespaceDao = new TableSpaceDAO();
        ProfileDAO profileDao = new ProfileDAO();
        public UcNhanVien2()
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSuaTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //string username = txtTenTaiKhoan.Text;
                //string password = txtMatKhau.Text;
                //accountDao.Update( username, password);
                LayDanhSach();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnThemTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //string username = txtTenTaiKhoan.Text;
                //string password = txtMatKhau.Text;
                //int employeeId = int.Parse(cbNhanVienCuaTaiKhoan.Text.Split("|")[0]);
                //string roles = cbVaiTro.Text;
                //if(roles == "Admin")
                //{
                    //roles = "sysadmin";
                //}
                //accountDao.Insert(username, password, employeeId, roles);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnXoaUser_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)dtgDanhSachTaiKhoan.SelectedValue;
            try
            {
                txbUsername.Text = drv["USERNAME"].ToString();

                cbTempTablespace.SelectedValue = drv["TEMPORARY_TABLESPACE"];
                cbDefaultTablespace.SelectedValue = drv["DEFAULT_TABLESPACE"];
                cbProfile.SelectedValue = drv["PROFILE"];
                cbAccountStatus.SelectedValue = drv["ACCOUNT_STATUS"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnThemSdtNhanVien_Click(object sender, RoutedEventArgs e)
        {
            if (cbNhanVienCuaSDT.SelectedValue != null)
            {
                string phoneNumber = txtSDT.Text;
                int employeeId = int.Parse(cbNhanVienCuaSDT.SelectedValue.ToString().Split("|")[0]);
                phoneDao.Insert(phoneNumber, employeeId);
                LayDanhSach();
            }
        }

        private void btnThongTinSdtNhanVien_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)dtgDanhSachSdtCuaNhanVien.SelectedValue;
            try
            {
                cbNhanVienCuaSDT.SelectedValue = drv["employee_id"].ToString() + "|" + drv["employee_name"].ToString();
                txtSDT.Text = drv["phone_number"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnXoaSdtNhanVien_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)dtgDanhSachSdtCuaNhanVien.SelectedValue;
            int employeeId = (int)drv["employee_id"];
            string phoneNumber = (string)drv["phone_number"];
            phoneDao.Delete(phoneNumber, employeeId);
            LayDanhSach();
        }

        private void btnTimKiemTheoTenNhanVien_Click(object sender, RoutedEventArgs e)
        {
            string employeeName = txtLocTheoTenNhanVien.Text;
            dtgDanhSachNhanVien.ItemsSource = employeeDao.TimKiemTheoHoTen(employeeName).DefaultView;
        }

        private void btnTimKiemSdtTheoTenNhanVien_Click(object sender, RoutedEventArgs e)
        {
            string employeeName = txtLocSdtTheoTenNhanVien.Text;
            dtgDanhSachSdtCuaNhanVien.ItemsSource = phoneDao.SearchByEmployeeName(employeeName).DefaultView;
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
