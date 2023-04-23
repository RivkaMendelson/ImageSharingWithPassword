using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharing_Data
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Passcode { get; set; }
        public int Views { get; set; }
    }
}
