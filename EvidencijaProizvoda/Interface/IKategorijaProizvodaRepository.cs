using EvidencijaProizvoda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvidencijaProizvoda.Interface
{
    public interface IKategorijaProizvodaRepository
    {
        IEnumerable<KategorijaProizvoda> GetAll();
        KategorijaProizvoda GetById(int id);
        void Add(KategorijaProizvoda kategorijaProizvoda);
        void Delete(KategorijaProizvoda kategorijaProizvoda);
        void Put(KategorijaProizvoda kategorijaProizvoda);
        IEnumerable<KategorijaPoCeni> Statistika();
        IEnumerable<KategorijaPoCeni> Najmanji();
    }
}