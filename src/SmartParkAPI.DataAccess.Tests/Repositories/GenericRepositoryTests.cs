using System.Linq;
using Autofac.Extras.Moq;
using Microsoft.EntityFrameworkCore;
using SharpTestsEx;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Interfaces;
using SmartParkAPI.DataAccess.Repositories;
using SmartParkAPI.DataAccess.Tests.Base;
using SmartParkAPI.Model;
using Xunit;

namespace SmartParkAPI.DataAccess.Tests.Repositories
{
    public class GenericRepositoryTests : DataAccessTestBase
    {
        private  IUnitOfWork _uow;
        private  IPriceTresholdRepository _repository;

        private readonly AutoMock _mock = AutoMock.GetLoose();

        public GenericRepositoryTests()
        {
            InitContext();
            _repository.Add(GetPriceTreshold());
            _repository.Add(GetPriceTreshold());
            _repository.Add(GetPriceTreshold());
            _uow.Commit();
        }

     

        [Fact]
        public void AddEntity_ThenResultIsValid()
        {
            //Before
            var preResult = _repository.GetAll().ToList();

            //Act
            _repository.Add(GetPriceTreshold());
            _uow.Commit();
            var result = _repository.GetAll().ToList();

            //Then
            result.Count.Should().Be.EqualTo(preResult.Count + 1);
        }

        [Fact]
        public void UpdateEntity_ThenResultIsValid()
        {
            //Before
            var entites = _repository.GetAll().ToList();
            var lastEntity = entites.Last();

            //Act
            lastEntity.MinCharges = 999;
            _repository.Edit(lastEntity);
            _uow.Commit();

            //Then
            var result = _repository.GetAll().ToList();
            result.Count.Should().Be.EqualTo(entites.Count);
            lastEntity.MinCharges.Should().Be.EqualTo(999);
        }

        [Fact]
        public void DeleteLastEntity_ThenResultIsValid()
        {
            InitContext();
            //Before
            var entites = _repository.GetAll().ToList();
            var lastEntity = entites.Last();

            //Act
            _repository.Delete(lastEntity);
            _uow.Commit();

            //Then
            var result = _repository.GetAll().ToList();
            result.Count.Should().Be.EqualTo(entites.Count - 1);
        }

        private void InitContext()
        {
            _mock.Mock<IDatabaseFactory>().Setup(x => x.Get()).Returns(GetContext());
            _repository = _mock.Create<PriceTresholdRepository>();
            _uow = _mock.Create<UnitOfWork>();
        }

        private ParkingAthContext GetContext()
        {
            var context = new ParkingAthContext(true);
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context;
        }
    }
}
