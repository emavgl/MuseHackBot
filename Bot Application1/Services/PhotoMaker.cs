using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Bot_Application1.Services
{
    public static class PhotoMaker
    {
        public static async Task<string> mergeImages(string img1, string text)
        {
            string tmpFile1 = HttpContext.Current.Server.MapPath("~/Data/" + "tmp" + Guid.NewGuid().ToString() + ".jpeg");
            string tmpFile2 = HttpContext.Current.Server.MapPath("~/Data/" + "tmp" + Guid.NewGuid().ToString() + ".jpeg");
            string outFile = HttpContext.Current.Server.MapPath("~/Data/" + "tmp" + Guid.NewGuid().ToString() + ".jpeg");

            HttpClient httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync(img1);
            byte[] photoBytes = await responseMessage.Content.ReadAsByteArrayAsync();
            httpClient.Dispose();

            //RESIZE INPUT FILE

            ISupportedImageFormat format = new JpegFormat { Quality = 95 };
            Size size = new Size(682, 0);
            using (MemoryStream inStream = new MemoryStream(photoBytes))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        // Load, resize, set the format and quality and save an image.
                        imageFactory.Load(inStream)
                                    .Resize(size)
                                    .Format(format)
                                    .Save(outStream);
                    }
                    // Do something with the stream.
                    Image img = System.Drawing.Image.FromStream(outStream);

                    img.Save(tmpFile1, ImageFormat.Jpeg);
                }
            }

            //ADD PHOTO TO HOMO SAPIENS
            int quality = 95;
            byte[] imgB2 = File.ReadAllBytes(tmpFile1);
            byte[] imgB1 = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Assets/template.jpg"));

            using (var inStream1 = new MemoryStream(imgB1))
            {
                using (var inStream2 = new MemoryStream(imgB2))
                {
                    using (var outStream = new MemoryStream())
                    {
                        // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                        using (var imageFactory = new ImageFactory(preserveExifData: true))
                        {
                            imageFactory.Load(inStream1).Overlay(new ImageLayer()
                            {
                                Image = System.Drawing.Image.FromStream(inStream2),
                                Position = new Point(0, 0)
                            }
                            )
                                 .Format(format)
                                .Quality(quality)
                                .Save(outStream);

                        }
                        // Do something with the stream.
                        Image img = System.Drawing.Image.FromStream(outStream);

                        img.Save(tmpFile2, ImageFormat.Jpeg);
                    }
                }
            }

            //ADD TEXT

            byte[] tmpFileB = File.ReadAllBytes(tmpFile2);
            using (var inStream = new MemoryStream(tmpFileB))
            {
                using (var outStream = new MemoryStream())
                {
                    // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                    using (var imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        // Do your magic here
                        imageFactory.Load(inStream)
                            .Watermark(new TextLayer()
                            {
                                DropShadow = true,
                                Text = text,
                                Position = new Point(400, 400),
                                FontSize = 350,
                                FontColor = Color.White
                            })
                            .Resize(size)
                            .Format(format)
                            .Quality(quality)
                            .Save(outStream);
                    }
                    // Do something with the stream.
                    Image img = System.Drawing.Image.FromStream(outStream);
                    img.Save(outFile, ImageFormat.Jpeg);
                }
            }
            return outFile;
        }
    }
}