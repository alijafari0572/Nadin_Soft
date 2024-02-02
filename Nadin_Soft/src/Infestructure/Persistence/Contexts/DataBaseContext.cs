using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    public class DataBaseContext : IdentityDbContext<ApplicationUser>
    {
        public DataBaseContext(DbContextOptions databaseOperations)
       : base(databaseOperations)
        {
        }
        #region Entities
        DbSet<ApplicationUser> Users_tbl { get; set; }
        DbSet<Product> Products_tbl { get; set; }
        #endregion
    }
}
