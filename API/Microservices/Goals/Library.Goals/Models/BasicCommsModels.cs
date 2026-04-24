using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Goals.Models
{
    public class BasicCommsModels<T>
    {
        public BasicModel? Basic { get; set; }
        public T? Options { get; set; }
    }
}
