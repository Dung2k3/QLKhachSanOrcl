using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiKhachSan.Class
{
    public class HinhAnhModel: Model
    {
        private byte[] hinhAnh;
        public HinhAnhModel() { }
        public HinhAnhModel(byte[] hinhAnh)
        {
            this.hinhAnh = hinhAnh;
        }

        public byte[] HinhAnh { get => hinhAnh; set { hinhAnh = value; OnPropertyChanged(); } }
    }
}
