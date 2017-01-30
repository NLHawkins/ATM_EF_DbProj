using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_EF.Models
{
    public class Acct
    {
        public int Id { get; set; }
        public string AcctName { get; set; }
        public double CurrentBalance { get; set; }
        public User AcctUser { get; set; }
        

        //public virtual User User { get; set; }
    }
}
