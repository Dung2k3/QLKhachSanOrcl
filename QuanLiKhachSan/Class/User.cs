using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiKhachSan.Class
{
    internal class User
    {
        private string username;
        private string password;
        private string defaultTablespace;
        private string temporaryTablespace;
        private string lockDate;
        private DateTime created;
        private string accountStatus;
        private string profile;



        public User(string username, string password, string default_tablespace, string temporary_tablespace, string lock_date, DateTime created, string account_status, string profile)
        {
            this.username = username;
            this.password = password;
            this.defaultTablespace = default_tablespace;
            this.temporaryTablespace = temporary_tablespace;
            this.lockDate = lock_date;
            this.created = created;
            this.accountStatus = account_status;
            this.profile = profile;
        }
        public User()
        {
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string DefaultTablespace { get => defaultTablespace; set => defaultTablespace = value; }
        public string TemporaryTablespace { get => temporaryTablespace; set => temporaryTablespace = value; }
        public string AccountStatus { get => accountStatus; set => accountStatus = value; }
        public string Profile { get => profile; set => profile = value; }


    }
}
