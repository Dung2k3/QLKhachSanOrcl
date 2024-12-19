using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiKhachSan.Class
{
    internal class UserPrivilege
    {
        public bool CreateProfile { get; set; } = false;
        public bool AlterProfile { get; set; } = false;
        public bool DropProfile { get; set; } = false;

        public bool CreateRole { get; set; } = false;
        public bool AlterAnyRole { get; set; } = false;
        public bool DropAnyRole { get; set; } = false;
        public bool GrantAnyRole { get; set; } = false;

        public bool CreateSession { get; set; } = false;

        public bool SelectAnyTable { get; set; } = false;

        public bool CreateUser { get; set; } = false;
        public bool AlterUser { get; set; } = false;
        public bool DropUser { get; set; } = false;
        public bool SelectUser { get; set; } = false;
        public bool SelectQuota { get; set; } = false; 
        public bool SelectProfile { get; set; } = false;
        public bool SelectRole{ get; set; } = false;
        public bool SelectPrivs { get; set; } = false;
        public bool SelectUserDetail{ get; set; } = false;


    }
}
