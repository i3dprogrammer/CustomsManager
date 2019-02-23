using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsManager.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }

        [NotMapped]
        public string FullPath { get; set; }
        public string FileName
        {
            get
            {
                if (FullPath == null || FullPath == "" || !FullPath.Contains("\\"))
                    return "";
                else
                    return FullPath.Split('\\').Last();
            }
        }
        [NotMapped]
        public bool ReceiptExist
        {
            get
            {
                return Receipt != null;
            }
        }
        public virtual PDF Receipt { get; set; }
    }
}
