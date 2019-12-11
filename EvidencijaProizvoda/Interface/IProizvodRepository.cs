using EvidencijaProizvoda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvidencijaProizvoda.Interface
{
    public interface IProizvodRepository
    {
        IEnumerable<Proizvod> GetAll();
        Proizvod GetById(int id);
        void Add(Proizvod proizvod);
        void Delete(Proizvod proizvod);
        void Update(Proizvod proizvod);
        IEnumerable<Proizvod> GetByKategorija(string kategorija);
        IEnumerable<Proizvod> GetByCena(decimal cena);
    }
}