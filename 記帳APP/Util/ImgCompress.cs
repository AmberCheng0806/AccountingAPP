using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 記帳APP.Util
{
    internal static class ImgCompress
    {
        public static Bitmap Compress(Image image, string originPath, string path)
        {
            ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders()
          .FirstOrDefault(x => x.FormatID == ImageFormat.Jpeg.Guid);
            EncoderParameters encoderParams = new EncoderParameters(1);
            FileInfo fileInfo = new FileInfo(originPath);
            long quality = fileInfo.Length < 1000000 ? 50L : 5L;
            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            var memory = new MemoryStream();
            image.Save(memory, codec, encoderParams);
            return new Bitmap(memory);
        }
        public static Bitmap Compress(Image image, int width, int height)
        {
            Bitmap bitmap = new Bitmap(40, 40);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(image, 0, 0, 40, 40);
            }
            return bitmap;
        }
    }
}
