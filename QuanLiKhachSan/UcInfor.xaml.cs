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
    public partial class UcInfor : UserControl
    {
        UserDAO userDao = new UserDAO();
        RoleDAO roleDao = new RoleDAO();
        RolePrivilegesDAO rolePrivilegesDao = new RolePrivilegesDAO();
        TableSpaceDAO tablespaceDao = new TableSpaceDAO();
        ProfileDAO profileDao = new ProfileDAO();
        public UcInfor()
        {
            InitializeComponent();
        }
        public void LayDanhSach()
        {
            User user = userDao.GetCurrentUser();
            lbUsername.Content = user.Username;
            lbDefaultTablespace.Content = user.DefaultTablespace;
            lbTempTablespace.Content = user.TemporaryTablespace;
            lbLockDate.Content = user.LockDate;
            lbAccountStatus.Content = user.AccountStatus;
            lbCreatedDate.Content = user.Created;
            dtgUserRole.ItemsSource = roleDao.DSRoleCurrentUser().DefaultView;
            dtgUserPrivileges.ItemsSource = rolePrivilegesDao.DSUserPrivilegesCurrentUser().DefaultView;
            dtgRolePrivileges.ItemsSource = rolePrivilegesDao.DSRolePrivilegesCurrentUser().DefaultView;
        }

        private void btnFilterRolePrivileges_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)dtgUserRole.SelectedValue;
            string role = drv["GRANTED_ROLE"].ToString();

            dtgRolePrivileges.ItemsSource = rolePrivilegesDao.DSRolePrivilegesCurrentUserByRole(role).DefaultView;
        }
    } 
}
