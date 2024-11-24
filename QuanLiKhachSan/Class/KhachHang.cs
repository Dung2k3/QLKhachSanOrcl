using QuanLiKhachSan.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLiKhachSan.Class
{
    public class KhachHang: Model
    {
        private int maKhachHang;
        private string ten;
        private DateTime ngaySinh;
        private string diaChi;
        private string soCCCD;
        private string email;
        private string sdt;


        public KhachHang() { }

        public KhachHang(int maKhachHang, string ten) {
            this.MaKhachHang = maKhachHang;
            this.Ten = ten;
        }

        /// <summary>
        /// @return
        /// </summary>
        public Boolean KiemTraKhachHang()
        {
            try
            {
                if (this.maKhachHang <= 0)
                {
                    return false;
                }
                if (this.ten == null)
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

        public int MaKhachHang { get => maKhachHang; set => maKhachHang = value; }
        public string Ten { get => ten; set => ten = value; }
        public DateTime NgaySinh { get => ngaySinh; set => ngaySinh = value; }
        public string DiaChi { get => diaChi; set => diaChi = value; }
        public string SoCCCD { get => soCCCD; set => soCCCD = value; }
        public string Email { get => email; set => email = value; }
        public string Sdt { get => sdt; set => sdt = value; }
    }
}
