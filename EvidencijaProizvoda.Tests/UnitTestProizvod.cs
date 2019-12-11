using EvidencijaProizvoda.Controllers;
using EvidencijaProizvoda.Interface;
using EvidencijaProizvoda.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace EvidencijaProizvoda.Tests
{
    [TestClass]
    public class UnitTestProizvod
    {
        [TestMethod]
        public void GetReturnsProductWithSameId()
        {
            // Arrange
            var mockRepository = new Mock<IProizvodRepository>();
            mockRepository.Setup(x => x.GetById(42)).Returns(new Proizvod { Id = 42 });

            var controller = new ProizvodiController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Get(42);
            var contentResult = actionResult as OkNegotiatedContentResult<Proizvod>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(42, contentResult.Content.Id);
        }

        // --------------------------------------------------------------------------------------

        [TestMethod]
        public void GetReturnsNotFound()
        {
            // Arrange
            var mockRepository = new Mock<IProizvodRepository>();
            var controller = new ProizvodiController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Get(10);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteReturnsNotFound()
        {
            // Arrange 
            var mockRepository = new Mock<IProizvodRepository>();
            var controller = new ProizvodiController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Delete(10);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        // --------------------------------------------------------------------------------------

        [TestMethod]
        public void DeleteReturnsOk()
        {
            // Arrange
            var mockRepository = new Mock<IProizvodRepository>();
            mockRepository.Setup(x => x.GetById(10)).Returns(new Proizvod { Id = 10 });
            var controller = new ProizvodiController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Delete(10);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }

        // --------------------------------------------------------------------------------------

        [TestMethod]
        public void PutReturnsBadRequest()
        {
            // Arrange
            var mockRepository = new Mock<IProizvodRepository>();
            var controller = new ProizvodiController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Put(10, new Proizvod { Id = 9, Naziv = "Proizvod" });

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        // -------------------------------------------------------------------------------------

        [TestMethod]
        public void PostMethodSetsLocationHeader()
        {
            // Arrange
            var mockRepository = new Mock<IProizvodRepository>();
            var controller = new ProizvodiController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Post(new Proizvod { Id = 10, Naziv = "Proizovd" });
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Proizvod>;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual(10, createdResult.RouteValues["id"]);
        }

        // ------------------------------------------------------------------------------------------

        [TestMethod]
        public void GetReturnsMultipleObjects()
        {
            // Arrange
            List<Proizvod> proizvodi = new List<Proizvod>();
            proizvodi.Add(new Proizvod() { Id = 1, Naziv = "Proizvod" });
            proizvodi.Add(new Proizvod() { Id = 2, Naziv = "Proizvod2" });

            var mockRepository = new Mock<IProizvodRepository>();
            mockRepository.Setup(x => x.GetAll()).Returns(proizvodi.AsEnumerable());
            var controller = new ProizvodiController(mockRepository.Object);

            // Act
            IEnumerable<Proizvod> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(proizvodi.Count, result.ToList().Count);
            Assert.AreEqual(proizvodi.ElementAt(0), result.ElementAt(0));
            Assert.AreEqual(proizvodi.ElementAt(1), result.ElementAt(1));
        }

        [TestMethod]
        public void GetReturnsByNazivKategorije()
        {
            // Arrange
            KategorijaProizvoda kp = new KategorijaProizvoda() { Id = 1, Naziv = "Testna Kategorija" };

            List<Proizvod> proizvodi = new List<Proizvod>();
            proizvodi.Add(new Proizvod() { Id = 1, Naziv = "Proizvod", KategorijaProizvodaId = kp.Id });
            proizvodi.Add(new Proizvod() { Id = 2, Naziv = "Proizvod2", KategorijaProizvodaId = kp.Id });

            var mockRepository = new Mock<IProizvodRepository>();
            mockRepository.Setup(x => x.GetByKategorija(kp.Naziv)).Returns(proizvodi.AsEnumerable().Where(p => p.KategorijaProizvodaId == kp.Id));
            var controller = new ProizvodiController(mockRepository.Object);

            // Act
            IEnumerable<Proizvod> result = controller.GetByKategorija(kp.Naziv);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(proizvodi.Count, result.ToList().Count);
            Assert.AreEqual(proizvodi.ElementAt(0), result.ElementAt(0));
            Assert.AreEqual(proizvodi.ElementAt(1), result.ElementAt(1));
        }

        [TestMethod]
        public void GetReturnsByCenaManja()
        {
            // Arrange
            List<Proizvod> proizvodi = new List<Proizvod>();
            proizvodi.Add(new Proizvod() { Id = 1, Naziv = "Proizvod", Cena=300});
            proizvodi.Add(new Proizvod() { Id = 2, Naziv = "Proizvod2", Cena=200});

            var mockRepository = new Mock<IProizvodRepository>();
            mockRepository.Setup(x => x.GetByCena(300)).Returns(proizvodi.AsEnumerable().Where(p => p.Cena < 300));
            var controller = new ProizvodiController(mockRepository.Object);

            // Act
            IEnumerable<Proizvod> result = controller.GetByCena(300);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(proizvodi.Count, result.ToList().Count);
            Assert.AreEqual(proizvodi.ElementAt(1), result.ElementAt(0));
        }

    }
}
