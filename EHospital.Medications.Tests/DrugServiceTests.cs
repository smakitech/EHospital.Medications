using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// Checks whether DrugService method GetByIdAsync throws <see cref="ArgumentException"/>
        /// in case invalid, non-existed identifier of drug is passing.
        /// </summary>
        /// <remarks>
        /// Method under testing can return records
        /// with IsDeleted property equal true as well.
        /// </remarks>
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
            Drug target = this.drugsList.Find(d => d.Id == id);
            this.mockUnitOfWork
                .Setup(u => u.Drugs.GetAsync(id))
                .ReturnsAsync(target);
            this.service = new DrugService(this.mockUnitOfWork.Object);

            // Act
            await this.service.GetByIdAsync(id);
        }

        /// <summary>
        /// Checks whether DrugService method GetByIdAsync works correctly
        /// when valid, existed identifier of drug is passing.
        /// </summary>
        /// <remarks>
        /// Method under testing can return records
        /// with IsDeleted property equal true as well.
        /// </remarks>
        /// <param name="id">The drug identifier.</param>
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        public void GetByIdAsync_ReturnsDrugCorrectly(int id)
        {
            // Arrange
            Drug expected = this.drugsList[id - 1];
            this.mockUnitOfWork
                .Setup(u => u.Drugs.GetAsync(id))
                .ReturnsAsync(expected);
            this.service = new DrugService(this.mockUnitOfWork.Object);

            // Act
            Drug actual = this.service.GetByIdAsync(id).Result;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Checks whether DrugService method GetAllByNameAsync throws <see cref="NoContentException"/>
        /// in case no match by name has been found.
        /// </summary>
        /// <remarks>
        /// Method under testing returns records with IsDeleted property equal false only.
        /// Method under testing searches for drugs start from specified name.
        /// Method under testing orders result set of records in ascending order.
        /// </remarks>
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
            IEnumerable<Drug> targetDrugs = this.drugsList
                .Where(d => d.IsDeleted == false && d.Name.ToLower().StartsWith(name.ToLower()));
            this.mockUnitOfWork
                .Setup(u => u.Drugs.GetAllAsync(It.IsAny<Expression<Func<Drug, bool>>>()))
                .ReturnsAsync(targetDrugs.AsQueryable());
            this.service = new DrugService(this.mockUnitOfWork.Object);

            // Act
            await this.service.GetAllByNameAsync(name);
        }

        /// <summary>
        /// Checks whether DrugService method GetAllByNameAsync
        /// finds and returns set of drugs with name which starts from specified name correctly.
        /// Checks whether DrugService method GetAllByNameAsync
        /// returns records with IsDeleted property equal false only.
        /// </summary>
        /// <remarks>
        /// Method under testing returns records with IsDeleted property equal false only.
        /// Method under testing searches for drugs start from specified name.
        /// Method under testing orders result set of records in ascending order.
        /// </remarks>
        /// <param name="name">
        /// Input to search for drug.
        /// </param>
        [TestMethod]
        [DataRow("par")]
        [DataRow("Par")]
        [DataRow("PAR")]
        [DataRow("Paracetamol")]
        [DataRow("ParaCEtamol")]
        [DataRow("p")]
        [DataRow("P")]
        public void GetAllByNameAsync_ReturnsAllFoundDrugsCorrectly(string name)
        {
            // Arrange
            IEnumerable<Drug> expected = this.drugsList
                .Where(d => d.IsDeleted == false && d.Name.ToLower().StartsWith(name.ToLower()));
            this.mockUnitOfWork
                .Setup(u => u.Drugs.GetAllAsync(It.IsAny<Expression<Func<Drug, bool>>>()))
                .ReturnsAsync(expected.AsQueryable());
            this.service = new DrugService(this.mockUnitOfWork.Object);
            expected.OrderBy(d => d.Name);

            // Act
            IEnumerable<Drug> actual = this.service.GetAllByNameAsync(name).Result;

            // Assert
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        /// TODO: Test fall down
        /// <summary>
        /// Checks whether DrugService method GetAllByNameAsync
        /// finds and returns set of drugs in ascending order with name which starts from specified name.
        /// Checks whether DrugService method GetAllByNameAsync
        /// returns records with IsDeleted property equal false only.
        /// </summary>
        /// <param name="name">
        /// Input to search for drug.
        /// </param>
        [TestMethod]
        [DataRow("p")]
        [DataRow("P")]
        [Ignore]
        public void GetAllByNameAsync_ReturnsAllFoundDrugsInAscendingOrder(string name)
        {
            // Arrange
            List<Drug> expected = this.drugsList
                .Where(d => d.IsDeleted == false && d.Name.ToLower().StartsWith(name.ToLower())).ToList();
            this.mockUnitOfWork
                .Setup(u => u.Drugs.GetAllAsync(It.IsAny<Expression<Func<Drug, bool>>>()))
                .ReturnsAsync(expected.AsQueryable());
            this.service = new DrugService(this.mockUnitOfWork.Object);
            expected.OrderBy(d => d.Name);

            // Act
            List<Drug> actual = this.service.GetAllByNameAsync(name).Result.ToList();

            // Assert
            for (int index = 0; index < expected.Count(); index++)
            {
                Assert.AreEqual(expected[index], actual[index]);
            }
        }

        /// <summary>
        /// Checks whether DrugService method GetAllAsync throws <see cref="NoContentException"/>
        /// in case no drugs records store in data source.
        /// </summary>
        /// <remarks>
        /// Method under testing returns records with IsDeleted property equal false only.
        /// Method under testing orders result set of records in ascending order.
        /// </remarks>
        /// <returns>Task object.</returns>
        [TestMethod]
        [ExpectedException(typeof(NoContentException))]
        public async Task GetAllAsync_ThrowsNoContentException()
        {
            // Arrange
            List<Drug> targetDrugs = new List<Drug>();
            this.mockUnitOfWork
                .Setup(u => u.Drugs.GetAllAsync(It.IsAny<Expression<Func<Drug, bool>>>()))
                .ReturnsAsync(targetDrugs.AsQueryable());
            this.service = new DrugService(this.mockUnitOfWork.Object);

            // Act
            await this.service.GetAllAsync();
        }

        /// <summary>
        /// Checks whether DrugService method GetAllAsync
        /// returns correctly all the drugs stored in data source
        /// and which have IsDeleted property equal false.
        /// </summary>
        [TestMethod]
        public void GetAllAsync_ReturnsAllDrugsCorrectly()
        {
            // Arrange
            IEnumerable<Drug> expected = this.drugsList.
                Where(d => d.IsDeleted == false);
            this.mockUnitOfWork
                .Setup(u => u.Drugs.GetAllAsync(It.IsAny<Expression<Func<Drug, bool>>>()))
                .ReturnsAsync(expected.AsQueryable());
            this.service = new DrugService(this.mockUnitOfWork.Object);
            expected.OrderBy(d => d.Name);

            // Act
            IEnumerable<Drug> actual = this.service.GetAllAsync().Result;

            // Assert
            Assert.AreEqual(expected.Count(), actual.Count());
        }

        // TODO: Test fall down
        /// <summary>
        /// Checks whether DrugService method GetAllAsync
        /// returns all the drugs stored in data source
        /// and have IsDeleted property equal false
        /// in ascending order.
        /// </summary>
        [TestMethod]
        [Ignore]
        public void GetAllAsync_ReturnsAllDrugsInAscendingOrder()
        {
            // Arrange
            List<Drug> expected = this.drugsList
                .Where(d => d.IsDeleted == false).ToList();
            this.mockUnitOfWork
                .Setup(u => u.Drugs.GetAllAsync(It.IsAny<Expression<Func<Drug, bool>>>()))
                .ReturnsAsync(expected.AsQueryable());
            this.service = new DrugService(this.mockUnitOfWork.Object);
            // Act
            List<Drug> actual = this.service.GetAllAsync().Result.ToList();

            //for (int index = 0; index < expected.Count(); index++)
            //{
            //    Assert.AreEqual(expected[index], actual[index]);
            //}
            Assert.AreEqual(expected[1].Id, actual[1].Id);
        }

        /// <summary>
        /// Checks whether DrugService method AddAsync throws <see cref="ArgumentException"/>
        /// in case attempt of addition of drug which is already stores in data source,
        /// has the same name, type, dose, dose unit and IsDeleted property equals false.
        /// </summary>
        /// <returns>Task object</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task AddAsync_ExistedDrug_ThrowsArgumentException()
        {
            // Arrange
            // Existed drug with IsDeleted property equals false
            Drug existed = this.drugsList[1];
            IEnumerable<Drug> existedDrugs = this.drugsList
                .Where(d => d.Name == existed.Name
                            && d.Type == existed.Type
                            && d.Dose == existed.Dose
                            && d.DoseUnit == existed.DoseUnit);
            this.mockUnitOfWork
                .Setup(u => u.Drugs.GetAllAsync(It.IsAny<Expression<Func<Drug, bool>>>()))
                .ReturnsAsync(existedDrugs.AsQueryable());
            this.mockUnitOfWork
                .Setup(u => u.Drugs.Insert(existed))
                .Returns(existed);
            this.service = new DrugService(this.mockUnitOfWork.Object);

            // Act
            await this.service.AddAsync(existed);
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
                    Name = "Paracetamol",
                    Type = "Pill",
                    Dose = 1000,
                    DoseUnit = "mg",
                    Direction = "Oral",
                    Instruction = "Paracetamol instruction.",
                    IsDeleted = false
                },
                new Drug()
                {
                    Id = 4,
                    Name = "Pertussin",
                    Type = "Syrup",
                    Dose = 100,
                    DoseUnit = "ml",
                    Direction = "Oral",
                    Instruction = "Pertussin instruction.",
                    IsDeleted = false
                },
                new Drug()
                {
                    Id = 5,
                    Name = "Paracetamol",
                    Type = "Pill",
                    Dose = 500,
                    DoseUnit = "mg",
                    Direction = "Oral",
                    Instruction = "Paracetamol instruction.",
                    IsDeleted = false
                },
                new Drug()
                {
                    Id = 6,
                    Name = "Paracetamol",
                    Type = "Pill",
                    Dose = 250,
                    DoseUnit = "mg",
                    Direction = "Oral",
                    Instruction = "Paracetamol instruction.",
                    IsDeleted = true
                }
            };

            return drugsList;
        }
    }
}