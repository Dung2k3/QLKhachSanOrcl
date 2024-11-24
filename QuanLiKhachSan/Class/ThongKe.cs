using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;

namespace QuanLiKhachSan.Class
{
    public class ThongKe: Model
    {
        private DateTime ngayBatDau;
        private DateTime ngayKetThuc;
        private double tongDoanhThu;
        private SeriesCollection seriesCollection;
        private List<string> labels;
        public ThongKe()
        {
            ngayBatDau = DateTime.Now.AddYears(-1);
            ngayKetThuc = DateTime.Now;
        }
        public DateTime NgayBatDau { get => ngayBatDau; set { ngayBatDau = value; OnPropertyChanged(); } }
        public DateTime NgayKetThuc { get => ngayKetThuc; set { ngayKetThuc = value; OnPropertyChanged(); } }
        public SeriesCollection SeriesCollection { get => seriesCollection; set { seriesCollection = value; OnPropertyChanged(); } }
        public List<string> Labels { get => labels; set { labels = value; OnPropertyChanged(); } }
        public double TongDoanhThu { get => tongDoanhThu; set { tongDoanhThu = value; OnPropertyChanged(); } }
    }
}
