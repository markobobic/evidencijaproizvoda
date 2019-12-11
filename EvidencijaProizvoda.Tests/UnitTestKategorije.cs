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
    public class UnitTestKategorije
    {
        [TestMethod]
        public void GetReturnsProductWithSameId()
        {
            // Arrange
            var mockRepository = new Mock<IKategorijaProizvodaRepository>();
            mockRepository.Setup(x => x.GetById(42)).Returns(new Models.KategorijaProizvoda { Id = 42 });

            var controller = new KategorijeController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Get(42);
            var contentResult = actionResult as OkNegotiatedContentResult<KategorijaProizvoda>;

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
            var mockRepository = new Mock<IKategorijaProizvodaRepository>();
            var controller = new KategorijeController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Get(10);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteReturnsNotFound()
        {
            // Arrange 
            var mockRepository = new Mock<IKategorijaProizvodaRepository>();
            var controller = new KategorijeController(mockRepository.Object);

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
            var mockRepository = new Mock<IKategorijaProizvodaRepository>();
            mockRepository.Setup(x => x.GetById(10)).Returns(new KategorijaProizvoda { Id = 10 });
            var controller = new KategorijeController(mockRepository.Object);

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
            var mockRepository = new Mock<IKategorijaProizvodaRepository>();
            var controller = new KategorijeController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Put(10, new KategorijaProizvoda { Id = 9, Naziv = "Kategorija" });

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        // -------------------------------------------------------------------------------------

        [TestMethod]
        public void PostMethodSetsLocationHeader()
        {
            // Arrange
            var mockRepository = new Mock<IKategorijaProizvodaRepository>();
            var controller = new KategorijeController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Post(new KategorijaProizvoda { Id = 10, Naziv="Kategorija"});
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<KategorijaProizvoda>;

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
            List<KategorijaProizvoda> kategorije = new List<KategorijaProizvoda>();
            kategorije.Add(new KategorijaProizvoda() { Id = 1, Naziv = "Kategorija1" });
            kategorije.Add(new KategorijaProizvoda() { Id = 2, Naziv = "Kategorija2" });

            var mockRepository = new Mock<IKategorijaProizvodaRepository>();
            mockRepository.Setup(x => x.GetAll()).Returns(kategorije.AsEnumerable());
            var controller = new KategorijeController(mockRepository.Object);

            // Act
            IEnumerable<KategorijaProizvoda> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(kategorije.Count, result.ToList().Count);
            Assert.AreEqual(kategorije.ElementAt(0), result.ElementAt(0));
            Assert.AreEqual(kategorije.ElementAt(1), result.ElementAt(1));
        }

        [TestMethod]
        public void GetReturnsStatistika()
        {
            // Arrange
            List<KategorijaPoCeni> kategorije = new List<KategorijaPoCeni>();
            kategorije.Add(new KategorijaPoCeni() { UkupnaCena = 11, Naziv = "Kategorija1" });
            kategorije.Add(new KategorijaPoCeni() { UkupnaCena = 22, Naziv = "Kategorija2" });

            var mockRepository = new Mock<IKategorijaProizvodaRepository>();
            mockRepository.Setup(x => x.Statistika()).Returns(kategorije.AsEnumerable().OrderByDescending(x=>x.UkupnaCena));
            var controller = new KategorijeController(mockRepository.Object);

            // Act
            IEnumerable<KategorijaPoCeni> result = controller.GetStatistika();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(kategorije.Count, result.ToList().Count);
            Assert.AreEqual(kategorije.ElementAt(0), result.ElementAt(1));
            Assert.AreEqual(kategorije.ElementAt(1), result.ElementAt(0));
        }

        [TestMethod]
        public void GetReturnsNajmanji()
        {
            // Arrange
            List<KategorijaPoCeni> kategorije = new List<KategorijaPoCeni>();
            kategorije.Add(new KategorijaPoCeni() { UkupnaCena = 11, Naziv = "Kategorija1" });
            kategorije.Add(new KategorijaPoCeni() { UkupnaCena = 22, Naziv = "Kategorija2" });

            var mockRepository = new Mock<IKategorijaProizvodaRepository>();
            mockRepository.Setup(x => x.Najmanji()).Returns(kategorije.AsEnumerable().OrderBy(x=>x.UkupnaCena));
            var controller = new KategorijeController(mockRepository.Object);

            // Act
            IEnumerable<KategorijaPoCeni> result = controller.GetNajmanji();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(kategorije.Count, result.ToList().Count);
            Assert.AreEqual(kategorije.ElementAt(0), result.ElementAt(0));
            Assert.AreEqual(kategorije.ElementAt(1), result.ElementAt(1));
        }

 

    }
}
