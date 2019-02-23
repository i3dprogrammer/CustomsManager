using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsManager.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Index(IsUnique = true)]
        public int Code { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Deposit> Deposits { get; set; }
        public virtual ICollection<Operation> Operations { get; set; }

        private float _totalOperationsCost()
        {
            float total = 0.0f;
            if(Operations != null)
            {
                Operations.ToList().ForEach(x => total += x.TotalCost());
            }
            return total;
        }

        private float _totalDeposits()
        {
            float total = 0.0f;
            if(Deposits != null)
            {
                Deposits.ToList().ForEach(x => total += x.Value);
            }
            return total;
        }

        public float TotalDeposits
        {
            get
            {
                return _totalDeposits();
            }
        }

        public float TotalCost
        {
            get
            {
                return _totalOperationsCost();
            }
        }

        public float TotalCredit
        {
            get
            {
                return TotalDeposits - TotalCost;
            }
        }
    }
}
