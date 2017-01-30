using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_EF.Models
{
     public class ATM_Log
    {
        public int Id { get; set; }
        public Acct ActiveAcct { get; set; }
        public DateTime Time { get; set; }
        public string Adjustment { get; set; }
        public double AdjValue { get; set; }
        public Acct RecipientAcct { get; set; }
        public double LogBalance { get; set; }

        
               
    }
}
