﻿using ImageMagick;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Birder2.Services
{
    public class StreamService : IStreamService
    {
        public async Task<byte[]> GetByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public byte[] ResizePhoto(byte[] resizeArray)
        {
            
            using (MagickImage image = new MagickImage(resizeArray))
            {
                MagickGeometry size = new MagickGeometry(30, 30);

                size.IgnoreAspectRatio = true;

                image.Resize(size);
                image.Write(@"C:\Users\rcros\Desktop\NewSize.png");
                var x = image.ToByteArray();
                return x;
            }
            //return ;
        }
    }
}


//public async Task<byte[]> GetByteArray(IFormFile file)
//{
//    using (var memoryStream = new MemoryStream())
//    {
//        await file.CopyToAsync(memoryStream);
//        return memoryStream.ToArray();
//    }
//}


