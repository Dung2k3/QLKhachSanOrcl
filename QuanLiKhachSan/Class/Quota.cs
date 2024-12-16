using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiKhachSan.Class
{
    internal class Quota
    {
        private string username;
        private string tablespaceName;
        private int maxQuota;

        public Quota(string username, string tablespaceName, int maxQuota)
        {
            this.username = username;
            this.tablespaceName = tablespaceName;
            this.maxQuota = maxQuota;
        }

        public Quota()
        {
        }

        public string Username { get => username; set => username = value; }
        public string TablespaceName { get => tablespaceName; set => tablespaceName = value; }
        public int MaxQuota { get => maxQuota; set => maxQuota = value; }
    }
}
