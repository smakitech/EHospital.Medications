using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EHospital.Medications.BusinessLogic.Services;
using EHospital.Medications.Model;

namespace EHospital.Medications.Tests
{
    /// <summary>
    /// Test class for testing DrugService class.
    /// </summary>
    [TestClass]
    public class DrugServiceTests
    {
        /// <summary>
        /// The repository mock.
        /// </summary>
        private Mock<IRepository<Drug>> mockRepository;

        /// <summary>
        /// The unit of work mock.
        /// </summary>
        private Mock<IUnitOfWork> mockUnitOfWork;

        /// <summary>
        /// The drugs list for testing.
        /// </summary>
        private List<Drug> drugsList;

        /// <summary>
        /// The service under testing.
        /// </summary>
        private DrugService service;

        /// <summary>
        /// Initialize required objects before run of every test method.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockRepository = new Mock<IRepository<Drug>>();
            this.mockUnitOfWork = new Mock<IUnitOfWork>();
            this.mockUnitOfWork.Setup(u => u.Drugs).Returns(this.mockRepository.Object);
            this.drugsList = this.GetDrugsList();
        }

        /// <summary>
        /// Checks whether DrugService method GetByIdAsync works correctly
        /// when valid, existed identifier of drug is passing.
        /// </summary>
        /// <param name="id">The drug identifier.</param>
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        public void GetByIdAsync_ReturnsDrugCorrectly(int id)
        {
            // Arrange
            this.mockUnitOfWork.Setup(u => u.Drugs.GetAsync(id)).ReturnsAsync(this.drugsList[id - 1]);
            this.service = new DrugService(this.mockUnitOfWork.Object);

            // Act
            Drug actual = this.service.GetByIdAsync(id).Result;

            // Assert
            Assert.AreEqual(this.drugsList[id - 1], actual);
        }

        /// <summary>
        /// Checks whether DrugService method GetByIdAsync throws <see cref="ArgumentException"/>
        /// in case invalid, non-existed identifier of drug is passing.
        /// </summary>
        /// <param name="id">The drug identifier.</param>
        /// <returns>Task object.</returns>
        [TestMethod]
        [DataRow(0)]
        [DataRow(100)]
        [DataRow(-7)]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetByIdAsync_ThrowsArgumentException(int id)
        {
            // Arrange
            this.mockUnitOfWork.Setup(u => u.Drugs.GetAsync(id)).ReturnsAsync(default(Drug));
            this.service = new DrugService(this.mockUnitOfWork.Object);

            // Act
            Drug actual = await this.service.GetByIdAsync(id);
        }

        /// <summary>
        /// Checks whether DrugService method GetAllByNameAsync throws <see cref="NoContentException"/>
        /// no any match by name has been passed.
        /// </summary>
        /// <param name="name">
        /// Input to search for drug.
        /// </param>
        /// <returns>Task object.</returns>
        [TestMethod]
        [DataRow("Nise")]
        [DataRow("nise")]
        [ExpectedException(typeof(NoContentException))]
        public async Task GetAllByNameAsync_ThrowsNoContentException(string name)
        {
            // Arrange
            this.mockUnitOfWork.Setup(u => u.Drugs.GetAllAsync(It.IsAny<Expression<Func<Drug, bool>>>()))
                .ReturnsAsync(this.drugsList.Where(d => d.IsDeleted == false && d.Name.StartsWith(name)).AsQueryable());
            this.service = new DrugService(this.mockUnitOfWork.Object);

            // Act
            IEnumerable<Drug> actual = await this.service.GetAllByNameAsync(name);
        }

        /// <summary>
        /// Checks whether DrugService method GetAllByNameAsync
        /// finds and returns set of drugs with name which starts from name
        /// specified by parameter of testing method.
        /// </summary>
        /// <param name="name">
        /// Input to search for drug.
        /// </param>
        /// <param name="expected">
        /// Amount of expected matches.
        /// </param>
        /// <returns>
        /// Task object.
        /// </returns>
        [TestMethod]
        [DataRow("alt", 2)]
        [DataRow("Alt", 2)]
        [DataRow("ALT", 2)]
        [DataRow("Althea", 2)]
        [DataRow("AltHEa", 2)]
        [DataRow("Althea root", 2)]
        [DataRow("a", 3)]
        [DataRow("A", 3)]
        public async Task GetAllByNameAsync_ReturnsAllFoundDrugsCorrectly(string name, int expected)
        {
            // Arrange
            this.mockUnitOfWork.Setup(u => u.Drugs.GetAllAsync(It.IsAny<Expression<Func<Drug, bool>>>()))
                .ReturnsAsync(this.drugsList.Where(d => d.IsDeleted == false
                && d.Name.ToLower().StartsWith(name.ToLower())).AsQueryable());
            this.service = new DrugService(this.mockUnitOfWork.Object);
            // Amount of actual matches.
            int actual = 0;

            // Act
            IEnumerable<Drug> drugs = await this.service.GetAllByNameAsync(name);
            actual = drugs.Count();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Checks whether DrugService method GetAllByNameAsync
        /// finds and returns set of drugs with name which starts from name
        /// specified by parameter of testing method in descending order by name.
        /// </summary>
        /// <param name="name">
        /// Input to search for drug.
        /// </param>
        [TestMethod]
        [DataRow("a")]
        [DataRow("A")]
        public void GetAllByNameAsync_ReturnsAllFoundDrugsInDescendingOrder(string name)
        {
            // Arrange
            List<Drug> expected = this.drugsList.Where(d => d.IsDeleted == false && d.Name.ToLower().StartsWith(name.ToLower())).ToList();
            expected.OrderBy(d => d.Name);
            this.mockUnitOfWork.Setup(u => u.Drugs.GetAllAsync(It.IsAny<Expression<Func<Drug, bool>>>())).ReturnsAsync(expected.AsQueryable());
            this.service = new DrugService(this.mockUnitOfWork.Object);
            List<Drug> actual;

            // Act
            actual = this.service.GetAllByNameAsync(name).Result.ToList();

            // Assert
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);
            Assert.AreEqual(expected[2], actual[2]);
        }

        /// <summary>
        /// Checks whether DrugService method GetAllAsync
        /// returns correctly all the drugs stored in data source
        /// and have non-deleted status.
        /// </summary>
        /// <returns>
        /// Task object.
        /// </returns>
        [TestMethod]
        public async Task GetAllAsync_ReturnsAllDrugsCorrectly()
        {
            // Arrange
            // TODO: Get rid of dependency to number
            // TODO: IsDeleted isn't checked before
            int expected = 4;
            int actual;
            this.mockUnitOfWork.Setup(u => u.Drugs.GetAllAsync(It.IsAny<Expression<Func<Drug, bool>>>()))
                .ReturnsAsync(this.drugsList.Where(d => d.IsDeleted == false).AsQueryable());
            this.service = new DrugService(this.mockUnitOfWork.Object);

            // Act
            IEnumerable<Drug> drugs = await this.service.GetAllAsync();
            actual = drugs.Count();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Checks whether DrugService method GetAllAsync
        /// returns all the drugs stored in data source
        /// and have non-deleted status in descending order.
        /// </summary>
        [TestMethod]
        public void GetAllAsync_ReturnsAllDrugsInDescendingOrder()
        {
            // Arrange
            List<Drug> expected = this.drugsList.Where(d => d.IsDeleted == false).ToList();
            this.mockUnitOfWork.Setup(u => u.Drugs.GetAllAsync(It.IsAny<Expression<Func<Drug, bool>>>())).ReturnsAsync(expected.AsQueryable());
            this.service = new DrugService(this.mockUnitOfWork.Object);
            expected.OrderBy(d => d.Name);
            List<Drug> actual;

            // Act
            actual = this.service.GetAllAsync().Result.ToList();

            // Assert
            Assert.AreEqual(expected[0], actual[0]);
            //for (int index = 0; index < expected.Count; index++)
            //{
            //    Assert.AreEqual(expected[index], actual[index]);
            //}
        }

        /// <summary>
        /// Gets the drugs list for testing.
        /// </summary>
        /// <returns>List of drugs.</returns>
        private List<Drug> GetDrugsList()
        {
            List<Drug> drugsList = new List<Drug>()
            {
                new Drug()
                {
                    Id = 1,
                    Name = "Septefrilum",
                    Type = "Pill",
                    Dose = 0.2,
                    DoseUnit = "mg",
                    Direction = "Oral",
                    Instruction = "Septefrilum instruction.",
                    IsDeleted = true
                },
                new Drug()
                {
                    Id = 2,
                    Name = "Quixx",
                    Type = "Spray",
                    Dose = 30,
                    DoseUnit = "ml",
                    Direction = "Nasal",
                    Instruction = "Qyixx instruction.",
                    IsDeleted = false
                },
                new Drug()
                {
                    Id = 3,
                    Name = "Althea root",
                    Type = "Syrup",
                    Dose = 100,
                    DoseUnit = "ml",
                    Direction = "Oral",
                    Instruction = "Althea root instruction.",
                    IsDeleted = false
                },
                new Drug()
                {
                    Id = 4,
                    Name = "Althea root",
                    Type = "Pill",
                    Dose = 0.12,
                    DoseUnit = "g",
                    Direction = "Oral",
                    Instruction = "Althea root instruction.",
                    IsDeleted = false
                },
                new Drug()
                {
                    Id = 5,
                    Name = "Ambroxol",
                    Type = "Pill",
                    Dose = 30,
                    DoseUnit = "mg",
                    Direction = "Oral",
                    Instruction = "Ambroxol root instruction.",
                    IsDeleted = false
                }
            };

            return drugsList;
        }
    }
}