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
    [Authorize]
    public class ProizvodiController : ApiController
    {
        IProizvodRepository _repository { get; set; }

        public ProizvodiController(IProizvodRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Proizvod> Get()
        {
            return _repository.GetAll();
        }

        public IHttpActionResult Get(int id)
        {
            Proizvod proizvod = _repository.GetById(id);
            if (proizvod!=null)
            {
                return Ok(proizvod);
            }
            return NotFound();
        }

        public IHttpActionResult Post(Proizvod proizvod)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _repository.Add(proizvod);
            return CreatedAtRoute("DefaultApi", new { id = proizvod.Id }, proizvod);
        }

        public IHttpActionResult Put(int id,Proizvod proizvod)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id!=proizvod.Id)
            {

                return BadRequest();
            }

            try
            {
                _repository.Update(proizvod);
            }
            catch 
            {
                throw;
            }
            return Ok(proizvod);
        }

        public IHttpActionResult Delete(int id)
        {
            Proizvod proizvod = _repository.GetById(id);

            if (proizvod==null)
            {
                return NotFound();
            }
            _repository.Delete(proizvod);
            return Ok();
        }

        public IEnumerable<Proizvod> GetByKategorija(string kategorija)
        {
            return _repository.GetByKategorija(kategorija);
        }

        public IEnumerable<Proizvod> GetByCena(decimal cena)
        {
            return _repository.GetByCena(cena);
        }
    }

}
