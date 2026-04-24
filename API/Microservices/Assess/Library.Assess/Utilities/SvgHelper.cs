using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Utilities
{
    public static class SvgHelper
    {      

        public static MemoryStream GetSvgStreamFromString(string svgContent)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(svgContent));
        }
    }
}
