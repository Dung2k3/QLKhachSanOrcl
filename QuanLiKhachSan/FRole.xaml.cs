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
        private UserDAO userDao = new UserDAO(); // Assuming a UserDAO exists
        private PrivilegeDAO privilegeDao = new PrivilegeDAO();

        public FRoleManagement()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                // Load role data into DataGrid
                dtgRoles.ItemsSource = roleDao.GetRoleList().DefaultView;

                dtgAssignedRoles.ItemsSource = roleDao.GetRoleUserList().DefaultView;


                // Load users and roles into ComboBoxes
                cbUser.ItemsSource = userDao.GetUsers();
                cbRole.ItemsSource = roleDao.GetRoles();
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
            //try
            //{
            //    DataRowView drv = (DataRowView)dtgAssignedRoles.SelectedItem;
            //    if (drv != null)
            //    {
            //        txtRoleName.Text = drv["Role_Name"].ToString();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Please select a role to view details.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private void btnUpdateRole_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string rolename = txtRoleName.Text;
                string password = txtPassword.Password;

                if (string.IsNullOrEmpty(rolename) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please fill in both Role Name and Password.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                roleDao.UpdateRole(rolename, password);
                LoadData();
                MessageBox.Show("Role updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating role: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddRole_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string rolename = txtRoleName.Text;
                string password = txtPassword.Password;

                if (string.IsNullOrEmpty(rolename) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please fill in both Role Name and Password.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                roleDao.AddRole(rolename, password);
                LoadData();
                MessageBox.Show("Role added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding role: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    MessageBox.Show("Role deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
                string userId = cbUser.SelectedValue?.ToString();
                string roleId = cbRole.SelectedValue?.ToString();

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleId))
                {
                    MessageBox.Show("Please select both a user and a role.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                roleDao.AssignRoleToUser(userId, roleId);
                LoadData();
                MessageBox.Show("Role assigned successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
                DataRowView selectedRole = (DataRowView)dtgAssignedRoles.SelectedItem;
                if (selectedRole != null)
                {
                    string userId = selectedRole["UserId"].ToString();
                    string roleId = selectedRole["RoleId"].ToString();
                    roleDao.RevokeRoleFromUser(userId, roleId);
                    LoadData();
                    MessageBox.Show("Role revoked successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Please select an assigned role to revoke.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
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
                    // Optional: Load user-specific data if needed
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
                    // Optional: Load role-specific data if needed
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error handling role selection: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}