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
                if(DbConnectionOrcl.userPrivilege.SelectPrivs)
                    dtgPrivilegeManagement.ItemsSource = rolePrivilegesDao.GetPrivileges().DefaultView;
                else
                    dtgPrivilegeManagement.ItemsSource = rolePrivilegesDao.DSUserAndRolePrivilegesCurrentUser().DefaultView;
                //dtgRoleManagement.ItemsSource = rolePrivilegesDao.GetRoles().DefaultView;

                //dtgProfileManagement.ItemsSource = rolePrivilegesDao.GetProfiles().DefaultView;

                //dtgUserInformation.ItemsSource = rolePrivilegesDao.GetUserInformation().DefaultView;

                //cbTargetUserRole.ItemsSource = rolePrivilegesDao.GetTargetUserRoles();

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

                if (privilegeType.Contains("System", StringComparison.OrdinalIgnoreCase))
                {
                    string systemPrivilege = (cbSystemPrivileges.SelectedItem as ComboBoxItem)?.Content?.ToString();

                    if (privilegeAction.Contains("Grant", StringComparison.OrdinalIgnoreCase))
                    {
                        rolePrivilegesDao.GrantSystemPrivilege(cbTargetUserRole.Text, systemPrivilege,
                            chkAllowReGrantPrivilege.IsChecked == true);
                    }
                    else
                    {
                        rolePrivilegesDao.RevokeSystemPrivilege(cbTargetUserRole.Text, systemPrivilege);
                    }
                }
                else if (privilegeType.Contains("Object", StringComparison.OrdinalIgnoreCase))
                {
                    string objectPrivilege = (cbObjectPrivileges.SelectedItem as ComboBoxItem)?.Content?.ToString();
                    string specificObject = txtSpecificObject.Text;
                    string columnName = txtColumnName.Text.Trim();

                    if (privilegeAction.Contains("Grant", StringComparison.OrdinalIgnoreCase))
                    {
                        rolePrivilegesDao.GrantObjectPrivilege(
                            cbTargetUserRole.Text,
                            specificObject,
                            objectPrivilege,
                            chkAllowReGrantPrivilege.IsChecked == true,
                            string.IsNullOrEmpty(columnName) ? null : columnName
                        );
                    }
                    else
                    {
                        rolePrivilegesDao.RevokeObjectPrivilege(
                            cbTargetUserRole.Text,
                            specificObject,
                            objectPrivilege,
                            string.IsNullOrEmpty(columnName) ? null : columnName
                        );
                    }
                }

                PopulateRolePrivilegesTabs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error granting/revoking privilege: {ex.Message}");
            }
        }

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

                    string objectName = obj;
                    string columnName = null;
                    if (obj.Contains("."))
                    {
                        var parts = obj.Split('.');
                        objectName = parts[0];
                        columnName = parts[1];
                    }

                    if (privilegeType == "System Privileges")
                    {
                        rolePrivilegesDao.RevokeSystemPrivilege(username, specificPrivilege);
                    }
                    else // Object or Column Privileges
                    {
                        rolePrivilegesDao.RevokeObjectPrivilege(username, objectName, specificPrivilege, columnName);
                    }

                    PopulateRolePrivilegesTabs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error revoking privilege: {ex.Message}");
            }
        }
        private void InitializeRolePrivilegesTab()
        {
            cbPrivilegeType.SelectedIndex = 0;
            cbPrivilegeAction.SelectedIndex = 0;
            var userRoles = rolePrivilegesDao.GetTargetUserRoles().ToList();
            userRoles.Insert(0, "All");
            cbTargetUserRole.ItemsSource = userRoles;
            cbTargetUserRole.SelectedIndex = 0;
            PopulateRolePrivilegesTabs();
        }
        private void cbPrivilegeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSystemPrivileges == null || cbObjectPrivileges == null ||
                txtSpecificObject == null || txtColumnName == null) return;

            string selectedType = (cbPrivilegeType.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (selectedType == "System Privileges")
            {
                cbSystemPrivileges.IsEnabled = true;
                cbSystemPrivileges.Background = Brushes.White;

                cbObjectPrivileges.IsEnabled = false;
                cbObjectPrivileges.Background = Brushes.Gray;
                txtSpecificObject.IsEnabled = false;
                txtSpecificObject.Visibility = Visibility.Hidden;
                txtColumnName.IsEnabled = false;
                txtColumnName.Visibility = Visibility.Collapsed;

                cbObjectPrivileges.SelectedIndex = -1;
                txtSpecificObject.Text = "";
                txtColumnName.Text = "";
            }
            else
            {
                cbSystemPrivileges.IsEnabled = false;
                cbSystemPrivileges.Background = Brushes.Gray;

                cbObjectPrivileges.IsEnabled = true;
                cbObjectPrivileges.Background = Brushes.White;
                txtSpecificObject.IsEnabled = true;
                txtSpecificObject.Visibility = Visibility.Visible;

                // Only show column name if INSERT or UPDATE is selected
                var selectedPrivilege = (cbObjectPrivileges.SelectedItem as ComboBoxItem)?.Content?.ToString();
                var selectedAction = (cbPrivilegeAction.SelectedItem as ComboBoxItem)?.Content?.ToString();

                if (selectedPrivilege == "INSERT" || selectedPrivilege == "UPDATE" && selectedAction == "Grant")
                {
                    txtColumnName.IsEnabled = true;
                    txtColumnName.Visibility = Visibility.Visible;
                }
                else
                {
                    txtColumnName.IsEnabled = false;
                    txtColumnName.Visibility = Visibility.Collapsed;
                    txtColumnName.Text = "";
                }

                cbSystemPrivileges.SelectedIndex = -1;
            }
        }
        private void cbObjectPrivileges_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtColumnName == null) return;

            var selectedPrivilege = (cbObjectPrivileges.SelectedItem as ComboBoxItem)?.Content?.ToString();
            var selectedAction = (cbPrivilegeAction.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (selectedPrivilege == "INSERT" || selectedPrivilege == "UPDATE" && selectedAction == "Grant")
            {
                txtColumnName.IsEnabled = true;
                txtColumnName.Visibility = Visibility.Visible;
            }
            else
            {
                txtColumnName.IsEnabled = false;
                txtColumnName.Visibility = Visibility.Collapsed;
                txtColumnName.Text = ""; // Clear the text when hidden
            }
        }
        private void cbPrivilegeAction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtColumnName == null || cbObjectPrivileges == null) return;

            var selectedPrivilege = (cbObjectPrivileges.SelectedItem as ComboBoxItem)?.Content?.ToString();
            var selectedAction = (cbPrivilegeAction.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if ((selectedPrivilege == "INSERT" || selectedPrivilege == "UPDATE") && selectedAction == "Grant")
            {
                txtColumnName.IsEnabled = true;
                txtColumnName.Visibility = Visibility.Visible;
            }
            else
            {
                txtColumnName.IsEnabled = false;
                txtColumnName.Visibility = Visibility.Collapsed;
                txtColumnName.Text = "";
            }
        }
        private void cbTargetUserRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedUserRole = cbTargetUserRole.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedUserRole) || selectedUserRole.Equals("All"))
            {
                PopulateRolePrivilegesTabs();
                return;
            }
            try
            {
                DataTable privilegesTable = rolePrivilegesDao.GetPrivileges();
                DataView privilegesView = privilegesTable.DefaultView;
                privilegesView.RowFilter = $"USERNAME LIKE '{selectedUserRole}'";
                dtgPrivilegeManagement.ItemsSource = privilegesView;

                DataTable rolesTable = rolePrivilegesDao.GetRoles();
                DataView rolesView = rolesTable.DefaultView;
                rolesView.RowFilter = $"AssignedUsers LIKE '%{selectedUserRole}%'";
                dtgRoleManagement.ItemsSource = rolesView;

                DataTable userInfoTable = rolePrivilegesDao.GetUserInformation();
                DataView userInfoView = userInfoTable.DefaultView;
                userInfoView.RowFilter = $"USERNAME LIKE '{selectedUserRole}' OR ROLES = '{selectedUserRole}'";
                dtgUserInformation.ItemsSource = userInfoView;

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
