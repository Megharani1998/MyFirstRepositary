using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using projectModels;


namespace MyContactManagerData
{
    public class MyContactDBManagerContext : DbContext 
    {

        
        private static IConfigurationRoot _configuration;


        public DbSet<State> States { get; set; }
        public DbSet<Contacts> Contacts { get; set; }


        public MyContactDBManagerContext()
        {
            //purposefully empty : necessary for Scaffold
        }

      
       
            public MyContactDBManagerContext(DbContextOptions<MyContactDBManagerContext> options) : base(options)
            {
            }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder().
               SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                _configuration = builder.Build();


                var cnstr = _configuration.GetConnectionString("MyContactManager");

                optionsBuilder.UseSqlServer(cnstr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<State>(x =>
            {
                x.HasData(
                    new State() { Id = 1, Name = "Solapur", Abbreviation = "SP" },
                    new State() { Id = 2, Name = "pune", Abbreviation = "PU" },
                    new State() { Id = 3, Name = "akola", Abbreviation = "AK" },
                    new State() { Id = 4, Name = "kurnul", Abbreviation = "KR" },
                    new State() { Id = 5, Name = "maipal", Abbreviation = "MP" },
                    new State() { Id = 6, Name = "UttarPradesh", Abbreviation = "UP" },
                    new State() { Id = 7, Name = "Maharashtra", Abbreviation = "MH" },
                    new State() { Id = 8, Name = "gujrat", Abbreviation = "GR" },
                    new State() { Id = 9, Name = "Bangal", Abbreviation = "BG" },
                    new State() { Id = 10, Name = "Rajsthan", Abbreviation = "RS" },
                    new State() { Id = 11, Name = "Manipur", Abbreviation = "MPR" },
                    new State() { Id = 12, Name = "Ratnagiri", Abbreviation = "RG" },
                    new State() { Id = 13, Name = "Goa", Abbreviation = "GO" }

                    );
            });
        }


    }
}
