namespace EvidencijaProizvoda.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EvidencijaProizvoda.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EvidencijaProizvoda.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.KategorijeProizvoda.AddOrUpdate(
                new Models.KategorijaProizvoda() { Id = 1, Naziv = "Obuca" },
                new Models.KategorijaProizvoda() { Id = 2, Naziv = "Odeca" },
                new Models.KategorijaProizvoda() { Id = 3, Naziv = "Hrana" }
                );

            context.Proizvodi.AddOrUpdate(
                new Models.Proizvod() { Id=1,Naziv="Patike",Cena=1000,KategorijaProizvodaId=1},
                new Models.Proizvod() { Id=2,Naziv="Majica",Cena=300,KategorijaProizvodaId=2},
                new Models.Proizvod() { Id=3,Naziv="Mleko",Cena=100,KategorijaProizvodaId=3}
                );
        }
    }
}
