using Microsoft.EntityFrameworkCore;
using restapi3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restapi3.Entities
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }
        public DbSet<GuestResponse> Responses { get; set; }
    }
}
