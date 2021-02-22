using BLL_Core.Enums.Images;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Core.Domain
{
    public class Photo
    {
        public int Size { get; set; }
        public string Key { get; set; }
        public string CommunityId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public Func<Stream> Data { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public ImageSize Type { get; set; }
    }
}
