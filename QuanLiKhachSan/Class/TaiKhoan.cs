using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiKhachSan.Class
{
    public class TaiKhoan: Model
    {
        private string tenTaiKhoan;
        private string matKhau;

        public string TenTaiKhoan { get => tenTaiKhoan; set { tenTaiKhoan = value; OnPropertyChanged(); } }
        public string MatKhau { get => matKhau; set { matKhau = value; OnPropertyChanged(); } }

        public TaiKhoan() { }
    }
}
