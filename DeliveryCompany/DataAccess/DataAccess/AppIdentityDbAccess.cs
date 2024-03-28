using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Authentification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess
{
    public class AppIdentityDbAccess : IdentityDbContext<AppUser>
    {
        public AppIdentityDbAccess(DbContextOptions<AppIdentityDbAccess> options) : base(options)
        {
        }
    }
}
