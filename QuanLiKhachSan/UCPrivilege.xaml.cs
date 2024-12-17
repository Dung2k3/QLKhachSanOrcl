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
    public partial class UcPrivilege : UserControl
    {
        UserDAO userDao = new UserDAO();
        NhanVienDAO employeeDao = new NhanVienDAO();
        PhoneNumberOfEmployeeDao phoneDao = new PhoneNumberOfEmployeeDao();
        TableSpaceDAO tablespaceDao = new TableSpaceDAO();
        ProfileDAO profileDao = new ProfileDAO();
        public UcPrivilege()
        {
            InitializeComponent();
        }

        public void LayDanhSach()
        {
            InitializeRolePrivilegesTab();
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
                string targetUserRole = cbTargetUserRole.SelectedValue?.ToString();
                string privilegeType = (cbPrivilegeType.SelectedItem as ComboBoxItem)?.Content?.ToString();
                string privilegeAction = (cbPrivilegeAction.SelectedItem as ComboBoxItem)?.Content?.ToString();
                //MessageBox.Show(targetUserRole + " " + privilegeAction +  ' ' + privilegeType);
                //if (string.IsNullOrEmpty(targetUserRole) ||
                //    string.IsNullOrEmpty(privilegeType) ||
                //    string.IsNullOrEmpty(privilegeAction))
                //{
                //    MessageBox.Show("Please select all required fields.");
                //    return;
                //}
                if (privilegeType.Contains("System", StringComparison.OrdinalIgnoreCase))
                {
                    string systemPrivilege = (cbSystemPrivileges.SelectedItem as ComboBoxItem)?.Content?.ToString();
                    //MessageBox.Show(privilegeType + " " + privilegeAction + " " + systemPrivilege);

                    if (privilegeAction.Contains("Grant", StringComparison.OrdinalIgnoreCase))
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
                else if (privilegeType.Contains("Object", StringComparison.OrdinalIgnoreCase))
                {
                    string objectPrivilege = (cbObjectPrivileges.SelectedItem as ComboBoxItem)?.Content?.ToString();
                    string specificObject = txtSpecificObject.Text;
                    MessageBox.Show(objectPrivilege + " " + specificObject);

                    if (privilegeAction.Contains("Grant", StringComparison.OrdinalIgnoreCase))
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
                privilegesView.RowFilter = $"USERNAME LIKE '{selectedUserRole}'";
                dtgPrivilegeManagement.ItemsSource = privilegesView;

                // Filter Role Management DataGrid
                DataTable rolesTable = rolePrivilegesDao.GetRoles();
                DataView rolesView = rolesTable.DefaultView;
                rolesView.RowFilter = $"AssignedUsers LIKE '%{selectedUserRole}%'";
                dtgRoleManagement.ItemsSource = rolesView;

                // Filter User Information DataGrid
                DataTable userInfoTable = rolePrivilegesDao.GetUserInformation();
                DataView userInfoView = userInfoTable.DefaultView;
                userInfoView.RowFilter = $"USERNAME LIKE '{selectedUserRole}' OR ROLES = '{selectedUserRole}'";
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
