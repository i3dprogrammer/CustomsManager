using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsManager.Models
{
    public class PDF
    {
        public int Id { get; set; }
        public virtual byte[] SectionData { get; set; }
    }
}
