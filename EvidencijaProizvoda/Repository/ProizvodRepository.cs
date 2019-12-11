using EvidencijaProizvoda.Interface;
using EvidencijaProizvoda.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace EvidencijaProizvoda.Repository
{
    public class ProizvodRepository : IDisposable, IProizvodRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Add(Proizvod proizvod)
        {
            db.Proizvodi.Add(proizvod);
            db.SaveChanges();
        }

        public void Delete(Proizvod proizvod)
        {
            db.Proizvodi.Remove(proizvod);
            db.SaveChanges();
        }

        public IEnumerable<Proizvod> GetAll()
        {
            return db.Proizvodi.Include(p => p.KategorijaProizvoda);
        }

        public IEnumerable<Proizvod> GetByCena(decimal cena)
        {
            return db.Proizvodi.Include(p => p.KategorijaProizvoda).Where(p => p.Cena < cena).OrderBy(p => p.Cena);
        }

        public Proizvod GetById(int id)
        {
            return db.Proizvodi.Include(p => p.KategorijaProizvoda).SingleOrDefault(p => p.Id == id);
        }

        public IEnumerable<Proizvod> GetByKategorija(string kategorija)
        {
            return db.Proizvodi.Include(p => p.KategorijaProizvoda).Where(p => p.KategorijaProizvoda.Naziv == kategorija).OrderBy(p => p.KategorijaProizvoda.Naziv);
        }

        public void Update(Proizvod proizvod)
        {
            db.Entry(proizvod).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
    }
}