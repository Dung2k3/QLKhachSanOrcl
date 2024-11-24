using QuanLiKhachSan.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiKhachSan.Class
{
    public class DichVu: Model
    {
        private int maDichVu;
        private string tenDichVu;
        private float giaTien;


        public DichVu() { }

        public DichVu(int maDichVu, string tenDichVu) { 
            this.MaDichVu = maDichVu;
            this.TenDichVu = tenDichVu;
        }

        public Boolean KiemTraDichVu()
        {
            try
            {
                if (this.maDichVu <= 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public int MaDichVu { get => maDichVu; set => maDichVu = value; }
        public string TenDichVu { get => tenDichVu; set => tenDichVu = value; }
        public float GiaTien { get => giaTien; set => giaTien = value; }
    }
}
