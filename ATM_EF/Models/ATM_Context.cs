using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_EF.Models
{
    public class ATM_Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Acct> Accounts { get; set; }
        public DbSet<ATM_Log> ATM_Logs { get; set; }

    }
}
