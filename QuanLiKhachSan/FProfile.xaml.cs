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
using static System.Collections.Specialized.BitVector32;

namespace QuanLiKhachSan
{
    /// <summary>
    /// Interaction logic for UcNhanVien.xaml
    /// </summary>
    public partial class UcProfile : UserControl
    {
        PhoneNumberOfEmployeeDao phoneDao = new PhoneNumberOfEmployeeDao();
        TableSpaceDAO tablespaceDao = new TableSpaceDAO();
        ProfileDAO profileDao = new ProfileDAO();
        public UcProfile()
        {
            InitializeComponent();
        }

        public void LayDanhSach()
        {
            dtgDanhSachProfile.ItemsSource = profileDao.LayDanhSach().DefaultView;
        }

        /// <summary>
        /// @return
        /// </summary>
        private void btnSuaTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string profilename = txbProfileName.Text;
                string session = "";
                string connecttime = "";
                string IDLE = "";
                // SESSIONS_PER_USER
                if (cbSession.SelectedIndex == 0)
                {
                    session = "UNLIMITED";
                    txbSession.Clear();
                }
                else if (cbSession.SelectedIndex == 1)
                {
                    session = "DEFAULT";
                    txbSession.Clear();
                }
                else
                {
                    session = txbSession.Text.ToString();
                }
                // CONNECT_TIME
                if (cbConnectTime.SelectedIndex == 0)
                {
                    connecttime = "UNLIMITED";
                    txbConnectTime.Clear();
                }
                else if (cbConnectTime.SelectedIndex == 1)
                {
                    connecttime = "DEFAULT";
                    txbConnectTime.Clear();
                }
                else
                {
                    connecttime = txbConnectTime.Text.ToString();
                }
                // IDLE_TIME
                if (cbIDLE.SelectedIndex == 0)
                {
                    IDLE = "UNLIMITED";
                    txbIDLE.Clear();
                }
                else if (cbIDLE.SelectedIndex == 1)
                {
                    IDLE = "DEFAULT";
                    txbIDLE.Clear();
                }
                else
                {
                    IDLE = txbIDLE.Text.ToString();
                }
                profileDao.AlterProfile(profilename, session, connecttime, IDLE); 
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
                
                string profilename = txbProfileName.Text;
                string session = "";
                string connecttime = "";
                string IDLE = "";
                // SESSIONS_PER_USER
                if (cbSession.SelectedIndex == 0)
                {
                    session = "UNLIMITED";
                }
                else if (cbSession.SelectedIndex == 1)
                {
                    session = "DEFAULT";
                }
                else
                {
                    session = txbSession.Text.ToString();
                }
                // CONNECT_TIME
                if (cbConnectTime.SelectedIndex == 0)
                {
                    connecttime = "UNLIMITED";
                }
                else if (cbSession.SelectedIndex == 1)
                {
                    connecttime = "DEFAULT";
                }
                else
                {
                    connecttime = txbConnectTime.Text.ToString();
                }
                // IDLE_TIME
                if (cbIDLE.SelectedIndex == 0)
                {
                    IDLE = "UNLIMITED";
                }
                else if (cbIDLE.SelectedIndex == 1)
                {
                    IDLE = "DEFAULT";
                }
                else
                {
                    IDLE = txbIDLE.Text.ToString();
                }
                profileDao.CreateNewProfile(profilename, session, connecttime, IDLE);
                LayDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnThongTinUser_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)dtgDanhSachProfile.SelectedValue;
            dtgUserProfile.ItemsSource = profileDao.LayDanhSachUserTheoProfile((string)drv["PROFILE"]).DefaultView;
            try
            {
                txbProfileName.Text = drv["PROFILE"].ToString();
                // SESSIONS_PER_USER
                if (drv["SESSIONS_PER_USER"].ToString() == "UNLIMITED")
                {
                    cbSession.SelectedIndex = 0;
                    txbSession.Clear();
                }
                else if (drv["SESSIONS_PER_USER"].ToString() == "DEFAULT")
                {
                    cbSession.SelectedIndex = 1;
                    txbSession.Clear();
                }
                else
                {
                    cbSession.SelectedIndex = 2;
                    txbSession.Text = drv["SESSIONS_PER_USER"].ToString();
                }
                // CONNECT_TIME
                if (drv["CONNECT_TIME"].ToString() == "UNLIMITED")
                {
                    cbConnectTime.SelectedIndex = 0;
                    txbConnectTime.Clear();

                }
                else if (drv["CONNECT_TIME"].ToString() == "DEFAULT")
                {
                    cbConnectTime.SelectedIndex = 1;
                    txbConnectTime.Clear();
                }
                else
                {
                    cbConnectTime.SelectedIndex = 2;
                    txbConnectTime.Text = drv["CONNECT_TIME"].ToString();
                }
                // IDLE_TIME
                if (drv["IDLE_TIME"].ToString() == "UNLIMITED")
                {
                    cbIDLE.SelectedIndex = 0;
                    txbIDLE.Clear();
                }
                else if (drv["IDLE_TIME"].ToString() == "DEFAULT")
                {
                    cbIDLE.SelectedIndex = 1;
                    txbIDLE.Clear();
                }
                else
                {
                    cbIDLE.SelectedIndex = 2;
                    txbIDLE.Text = drv["IDLE_TIME"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnXoaUser_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)dtgDanhSachProfile.SelectedValue;
            try
            {
                
                string profilename = (string)drv["PROFILE"];
                profileDao.DropProfile(profilename);
                LayDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
