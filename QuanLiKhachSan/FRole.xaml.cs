using QuanLiKhachSan.DAO;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace QuanLiKhachSan
{
    /// <summary>
    /// Interaction logic for FRoleManagement.xaml
    /// </summary>
    public partial class FRoleManagement : UserControl
    {
        private RoleDAO roleDao = new RoleDAO();

        public FRoleManagement()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                dtgRoles.ItemsSource = roleDao.GetRoleList().DefaultView;

                dtgAssignedRoles.ItemsSource = roleDao.GetRoleUserList().DefaultView;

                cbUser.ItemsSource = roleDao.GetGrantees()
                               .AsEnumerable()
                               .Select(row => new { UserName = row["GranteeName"].ToString() })
                               .ToList();

                cbRole.ItemsSource = roleDao.GetRoles()
                                            .Select(role => new { RoleName = role })
                                            .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnInfoRole_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView drv = (DataRowView)dtgRoles.SelectedItem;
                if (drv != null)
                {
                    txtRoleName.Text = drv["Role_Name"].ToString();
                }
                else
                {
                    MessageBox.Show("Please select a role to view details.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnInfoRoleUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedRow = dtgAssignedRoles.SelectedItem as DataRowView;
                if (selectedRow == null)
                {
                    MessageBox.Show("Please select a valid row.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                string userName = selectedRow["USER_NAME"]?.ToString();
                string roleName = selectedRow["ROLE_NAME"]?.ToString();
                string adminOption = selectedRow["ADMIN_OPTION"]?.ToString();

                cbUser.SelectedValue = userName; 
                cbRole.SelectedValue = roleName;
                checkAllowReGrantPrivilege.IsChecked = adminOption == "YES";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading role-user information: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddRole_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string rolename = txtRoleName.Text;
                string password = txtPassword.Password;

                if (string.IsNullOrEmpty(rolename))
                {
                    MessageBox.Show("Please fill in Role Name.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                roleDao.AddRole(rolename, password);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding role: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnUpdateRole_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string rolename = txtRoleName.Text;
                string password = txtPassword.Password;

                if (string.IsNullOrEmpty(rolename))
                {
                    MessageBox.Show("Please fill in Role Name.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                roleDao.UpdateRole(rolename, password);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating role: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDeleteRole_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView drv = (DataRowView)dtgRoles.SelectedItem;
                if (drv != null)
                {
                    string rolename = drv["Role_Name"].ToString();
                    roleDao.DeleteRole(rolename);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Please select a role to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting role: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnAssignRole_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string user = cbUser.SelectedValue?.ToString();
                string role = cbRole.SelectedValue?.ToString();
                bool allowReGrant = checkAllowReGrantPrivilege.IsChecked ?? false;

                if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(role))
                {
                    MessageBox.Show("Please select both a user and a role.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                roleDao.AssignRoleToUser(user, role, allowReGrant);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error assigning role: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnRevokeRole_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string user = cbUser.SelectedValue?.ToString();
                string role = cbRole.SelectedValue?.ToString();

                bool revokeOnlyAdminOption = checkAllowReGrantPrivilege.IsChecked ?? false;

                if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(role))
                {
                    MessageBox.Show("Please select both a user and a role.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                roleDao.RevokeRoleFromUser(user, role, revokeOnlyAdminOption);
                LoadData();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error revoking role: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cbUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbUser.SelectedValue != null)
                {
                    string selectedUserId = cbUser.SelectedValue.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error handling user selection: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbRole.SelectedValue != null)
                {
                    string selectedRoleId = cbRole.SelectedValue.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error handling role selection: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}