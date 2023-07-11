using APIREST.Models;
using Microsoft.EntityFrameworkCore;

namespace APIREST.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
                
        }

        public DbSet<Usuario> Usuario {get; set;}
        public DbSet<Pedido> Pedido {get; set;}
          

    }
}