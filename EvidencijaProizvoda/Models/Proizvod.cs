using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EvidencijaProizvoda.Models
{
    public class Proizvod
    {
        public int Id { get; set; }

        [Required]
        public string Naziv { get; set; }

        [Required]
        [Range(minimum:1.00,maximum:1000.00,ErrorMessage ="Cena mora biti izmedju 1 i 1000")]
        public decimal Cena { get; set; }

        [ForeignKey("KategorijaProizvoda")]
        public int KategorijaProizvodaId { get; set; }
        public KategorijaProizvoda KategorijaProizvoda { get; set; }
    }
}