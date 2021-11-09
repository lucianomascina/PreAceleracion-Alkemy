using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PreAceleracionOctubre.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreAceleracionOctubre.Context
{
    public class UserContext : IdentityDbContext<User>
    {
        private const string Shema = "users";
        
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema(Shema);
        }
    }
}
