using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsManager.Models
{
    public class Deposit
    {
        public int Id { get; set; }
        public float Value { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Bank { get; set; }
    }
}
