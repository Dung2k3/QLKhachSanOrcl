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
        NhanVienDAO employeeDao = new NhanVienDAO();
        PhoneNumberOfEmployeeDao phoneDao = new PhoneNumberOfEmployeeDao();
        TableSpaceDAO tablespaceDao = new TableSpaceDAO();
        ProfileDAO profileDao = new ProfileDAO();
        public UcNhanVien2()
        {
            InitializeComponent();

            InitializeRolePrivilegesTab();

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

        RolePrivilegesDAO rolePrivilegesDao = new RolePrivilegesDAO();

        // Method to populate DataGrids in Role & Privileges tab
        private void PopulateRolePrivilegesTabs()
        {
            try
            {
                dtgPrivilegeManagement.ItemsSource = rolePrivilegesDao.GetPrivileges().DefaultView;

                dtgRoleManagement.ItemsSource = rolePrivilegesDao.GetRoles().DefaultView;

                dtgProfileManagement.ItemsSource = rolePrivilegesDao.GetProfiles().DefaultView;

                dtgUserInformation.ItemsSource = rolePrivilegesDao.GetUserInformation().DefaultView;

                cbTargetUserRole.ItemsSource = rolePrivilegesDao.GetTargetUserRoles();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error populating Role & Privileges tabs: {ex.Message}");
            }
        }

        // Event handler for Granting Privileges
        private void btnGrantPrivilege_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string targetUserRole = cbTargetUserRole.SelectedItem?.ToString();
                string privilegeType = cbPrivilegeType.SelectedItem?.ToString();
                string privilegeAction = cbPrivilegeAction.SelectedItem?.ToString();

                // For System Privileges
                if (privilegeType == "System Privileges")
                {
                    string systemPrivilege = cbSystemPrivileges.SelectedItem?.ToString();

                    if (privilegeAction == "Grant")
                    {
                        rolePrivilegesDao.GrantSystemPrivilege(cbTargetUserRole.Text, systemPrivilege,
                            chkAllowReGrantPrivilege.IsChecked == true);
                    }
                    else // Revoke
                    {
                        rolePrivilegesDao.RevokeSystemPrivilege(cbTargetUserRole.Text, systemPrivilege);
                    }
                }
                // For Object Privileges
                else if (privilegeType == "Object Privileges")
                {
                    string objectPrivilege = cbObjectPrivileges.SelectedItem?.ToString();
                    string specificObject = txtSpecificObject.Text;

                    if (privilegeAction == "Grant")
                    {
                        rolePrivilegesDao.GrantObjectPrivilege(cbTargetUserRole.Text, specificObject,
                            objectPrivilege, chkAllowReGrantPrivilege.IsChecked == true);
                    }
                    else // Revoke
                    {
                        rolePrivilegesDao.RevokeObjectPrivilege(cbTargetUserRole.Text, specificObject, objectPrivilege);
                    }
                }

                // Refresh DataGrids after operation
                PopulateRolePrivilegesTabs();
                MessageBox.Show("Privilege operation completed successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error granting/revoking privilege: {ex.Message}");
            }
        }

        // Event handler for Revoking Individual Privileges
        private void btnRevokePrivilege_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedPrivilege = dtgPrivilegeManagement.SelectedItem as DataRowView;

                if (selectedPrivilege != null)
                {
                    string username = selectedPrivilege["Username"].ToString();
                    string privilegeType = selectedPrivilege["PrivilegeType"].ToString();
                    string specificPrivilege = selectedPrivilege["SpecificPrivilege"].ToString();
                    string obj = selectedPrivilege["Object"].ToString();

                    if (privilegeType == "System Privileges")
                    {
                        rolePrivilegesDao.RevokeSystemPrivilege(username, specificPrivilege);
                    }
                    else // Object Privileges
                    {
                        rolePrivilegesDao.RevokeObjectPrivilege(username, obj, specificPrivilege);
                    }

                    // Refresh DataGrids
                    PopulateRolePrivilegesTabs();
                    MessageBox.Show("Privilege revoked successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error revoking privilege: {ex.Message}");
            }
        }

        // Additional helper methods for Role & Privileges tab
        private void InitializeRolePrivilegesTab()
        {
            // Populate combo boxes with initial data
            cbPrivilegeType.SelectedIndex = 0;
            cbPrivilegeAction.SelectedIndex = 0;
            var userRoles = rolePrivilegesDao.GetTargetUserRoles().ToList();
            userRoles.Insert(0, "All"); // Add "All" as the first item
            cbTargetUserRole.ItemsSource = userRoles;
            cbTargetUserRole.SelectedIndex = 0; // Default to "All"
            // Initial population of DataGrids
            PopulateRolePrivilegesTabs();
        }
        private void cbTargetUserRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedUserRole = cbTargetUserRole.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedUserRole))
            {
                // If no user/role selected, show all data
                PopulateRolePrivilegesTabs();
                return;
            }

            try
            {
                // Filter Privilege Management DataGrid
                DataTable privilegesTable = rolePrivilegesDao.GetPrivileges();
                DataView privilegesView = privilegesTable.DefaultView;
                privilegesView.RowFilter = $"Username LIKE '{selectedUserRole}'";
                dtgPrivilegeManagement.ItemsSource = privilegesView;

                // Filter Role Management DataGrid
                DataTable rolesTable = rolePrivilegesDao.GetRoles();
                DataView rolesView = rolesTable.DefaultView;
                rolesView.RowFilter = $"AssignedUsers LIKE '%{selectedUserRole}%'";
                dtgRoleManagement.ItemsSource = rolesView;

                // Filter User Information DataGrid
                DataTable userInfoTable = rolePrivilegesDao.GetUserInformation();
                DataView userInfoView = userInfoTable.DefaultView;
                userInfoView.RowFilter = $"Username LIKE '{selectedUserRole}' OR Roles LIKE '%{selectedUserRole}%'";
                dtgUserInformation.ItemsSource = userInfoView;

                // Profile Management can be filtered similarly
                DataTable profilesTable = rolePrivilegesDao.GetProfiles();
                DataView profilesView = profilesTable.DefaultView;
                profilesView.RowFilter = $"AssignedUsers LIKE '%{selectedUserRole}%'";
                dtgProfileManagement.ItemsSource = profilesView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering Role & Privileges tabs: {ex.Message}");
            }
        }
    }
}
