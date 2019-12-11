using EvidencijaProizvoda.Interface;
using EvidencijaProizvoda.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace EvidencijaProizvoda.Repository
{
    public class KategorijaProizvodaRepository : IDisposable, IKategorijaProizvodaRepository
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

        public void Add(KategorijaProizvoda kategorijaProizvoda)
        {
            db.KategorijeProizvoda.Add(kategorijaProizvoda);
            db.SaveChanges();
        }

        public void Delete(KategorijaProizvoda kategorijaProizvoda)
        {
            db.KategorijeProizvoda.Remove(kategorijaProizvoda);
            db.SaveChanges();
        }

        public IEnumerable<KategorijaProizvoda> GetAll()
        {
            return db.KategorijeProizvoda;
        }

        public KategorijaProizvoda GetById(int id)
        {
            return db.KategorijeProizvoda.Find(id);
        }

        public void Put(KategorijaProizvoda kategorijaProizvoda)
        {
            db.Entry(kategorijaProizvoda).State = System.Data.Entity.EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public IEnumerable<KategorijaPoCeni> Najmanji()
        {
            return (from kp in db.KategorijeProizvoda
                            join p in db.Proizvodi
                            on kp.Id equals p.KategorijaProizvodaId
                            group p by kp.Naziv into KategorijaNaziv
                            orderby KategorijaNaziv.Sum(p=>p.Cena)
                            select new KategorijaPoCeni()
                            {
                                Naziv = KategorijaNaziv.Key,
                                UkupnaCena = KategorijaNaziv.Sum(p => p.Cena)

                            }).Take(2);
        }

        public IEnumerable<KategorijaPoCeni> Statistika()
        {
            return (from kp in db.KategorijeProizvoda
                              join p in db.Proizvodi
                              on kp.Id equals p.KategorijaProizvodaId
                              group p by kp.Naziv into KategorijaNaziv
                              orderby KategorijaNaziv.Sum(p=>p.Cena) descending
                              select new KategorijaPoCeni()
                              {
                                  Naziv = KategorijaNaziv.Key,
                                  UkupnaCena = KategorijaNaziv.Sum(p => p.Cena)
                              }).Take(2);
        }
    }
}