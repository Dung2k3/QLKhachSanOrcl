using System.Data.OracleClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Oracle.ManagedDataAccess.Client;
using QuanLiKhachSan.Class;

namespace QuanLiKhachSan.DAO
{
    class TableSpaceDAO
    {
        public DataTable LayDanhSach()
        {
            string sql = "SELECT tablespace_name, username, bytes / 1024 / 1024 AS quota_mb, " +
                "          blocks, CASE " +
                "              WHEN max_bytes > 0 THEN TO_CHAR(max_bytes / 1024 / 1024) " +
                "              ELSE 'Unlimited' " +
                "          END  AS max_quota_mb " +
                "         FROM dba_ts_quotas ";
            return DbConnectionOrcl.ExecuteTable(sql);
        }
        public DataTable LayDanhSachLikeUsername(string username)
        {
            string sql = "SELECT tablespace_name, username, bytes / 1024 / 1024 AS quota_mb, " +
                "          blocks, CASE " +
                "              WHEN max_bytes > 0 THEN TO_CHAR(max_bytes / 1024 / 1024) " +
                "              ELSE 'Unlimited' " +
                "          END  AS max_quota_mb " +
                "         FROM dba_ts_quotas " +
               $"         WHERE username LIKE '%{username}%'";
            return DbConnectionOrcl.ExecuteTable(sql);
        }

        public DataTable LayDanhSachByUsername(string username)
        {
            string sql = "SELECT tablespace_name, username, bytes / 1024 / 1024 AS quota_mb, " +
                "          blocks, CASE " +
                "              WHEN max_bytes > 0 THEN TO_CHAR(max_bytes / 1024 / 1024) " +
                "              ELSE 'Unlimited' " +
                "          END  AS max_quota_mb " +
                "         FROM dba_ts_quotas " +
               $"         WHERE username = '{username}'";
            return DbConnectionOrcl.ExecuteTable(sql);
        }

        public List<String> ListTempTableSpace() 
        {
            string sql = "SELECT tablespace_name " +
                " FROM dba_tablespaces " +
                " WHERE CONTENTS = 'TEMPORARY' ";
            return DbConnectionOrcl.ExecuteListString(sql);
        }

        public List<String> ListDefaultTableSpace()
        {
            string sql = "SELECT tablespace_name " +
                " FROM dba_tablespaces " +
                " WHERE CONTENTS = 'PERMANENT' ";
            return DbConnectionOrcl.ExecuteListString(sql);
        }

        public void Update(Quota quota)
        {
            string size = quota.MaxQuota >= 0 ? quota.MaxQuota.ToString() + "M" : "UNLIMITED";
            string sql = $"ALTER USER {quota.Username} QUOTA {size} ON {quota.TablespaceName} ";
            string mess = $"Update quota user {quota.Username} complete";
            DbConnectionOrcl.ExecuteNonQuery(sql, mess);
        }
    }
}
