using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTorrent_GUI
{
    public class DownloadingFileItem
    {
        public string Filename { get; set; }
        public float Size { get; set; }
        public DateTime StartedTime { get; set; }
        public DateTime EndedTime { get; set; }
        public float Percentage { get; set; }
    }
}
