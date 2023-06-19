using Microsoft.EntityFrameworkCore;
using VeterinaryClinic.Data.Entities;

namespace VeterinaryClinic.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {


        }
    }
}
