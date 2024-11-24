using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace QuanLiKhachSan.Class
{
    public class ChuyenHinhAnh: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) 
                return null;
            if (value is System.DBNull) 
                return null;
            byte[] bytes = (byte[])value;
            BitmapImage image = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                stream.Seek(0, SeekOrigin.Begin);
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
            }
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
