
using Microsoft.EntityFrameworkCore;

namespace ApiGazPro.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {            
        }

        //Agregar los modelos
        //public DbSet<Usuario> Usuario { get; set; }
    }
}
