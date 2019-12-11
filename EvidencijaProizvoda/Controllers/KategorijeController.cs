using EvidencijaProizvoda.Interface;
using EvidencijaProizvoda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EvidencijaProizvoda.Controllers
{
    public class KategorijeController : ApiController
    {
        IKategorijaProizvodaRepository _repository { get; set; }

        public KategorijeController(IKategorijaProizvodaRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<KategorijaProizvoda> Get()
        {
            return _repository.GetAll();
        }

        public IHttpActionResult Get(int id)
        {
            KategorijaProizvoda kategorijaProizvoda = _repository.GetById(id);
            if (kategorijaProizvoda != null)
            {
                return Ok(kategorijaProizvoda);
            }
            return NotFound();
        }

        public IHttpActionResult Post(KategorijaProizvoda kategorijaProizvoda)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _repository.Add(kategorijaProizvoda);
            return CreatedAtRoute("DefaultApi", new { id = kategorijaProizvoda.Id }, kategorijaProizvoda);
        }

        public IHttpActionResult Put(int id, KategorijaProizvoda kategorijaProizvoda)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id!=kategorijaProizvoda.Id)
            {
                return BadRequest();
            }
            try
            {
                _repository.Put(kategorijaProizvoda);
            }
            catch 
            {

                throw;
            }
            return Ok(kategorijaProizvoda);
        }

        public IHttpActionResult Delete(int id)
        {
            KategorijaProizvoda kategorijaProizvoda = _repository.GetById(id);
            if (kategorijaProizvoda!=null)
            {
                _repository.Delete(kategorijaProizvoda);
                return Ok();
            }
            return NotFound();
        }

        [Route("api/statistika")]
        public IEnumerable<KategorijaPoCeni> GetStatistika()
        {
            return _repository.Statistika();
        }

        [Route("api/najmanji")]
        public IEnumerable<KategorijaPoCeni> GetNajmanji()
        {
            return _repository.Najmanji();
        }


    }
}
