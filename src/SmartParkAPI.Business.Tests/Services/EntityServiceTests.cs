using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using SharpTestsEx;
using SmartParkAPI.Business.Services;
using SmartParkAPI.Business.Tests.Base;
using SmartParkAPI.Contracts.DTO.PriceTreshold;
using SmartParkAPI.DataAccess;
using SmartParkAPI.DataAccess.Common;
using SmartParkAPI.DataAccess.Repositories;
using Xunit;

namespace SmartParkAPI.Business.Tests.Services
{
    public class EntityServiceTests : BusinessTestBase
    {
        private PriceTresholdService _sut;
        private readonly AutoMock _mock = AutoMock.GetLoose();

        public EntityServiceTests()
        {
            InitContext();
            _sut.Create(GetPriceTreshold());
            _sut.Create(GetPriceTreshold());
            _sut.Create(GetPriceTreshold());
        }

        [Fact]
        public void AddEntity_ThenResultIsValid()
        {
            //Before
            var preResult = _sut.GetAll().Result.ToList();

            //Act
            _sut.Create(GetPriceTreshold());

            var result = _sut.GetAll();

            //Then
            result.IsValid.Should().Be.True();
            result.Result.ToList().Count.Should().Be.EqualTo(preResult.Count + 1);
        }

        [Fact]
        public void AddManyEntity_ThenResultIsValid()
        {
            //Before
            var preResult = _sut.GetAll().Result.ToList();

            //Act
            _sut.CreateMany(new List<PriceTresholdBaseDto> {GetPriceTreshold(), GetPriceTreshold(), GetPriceTreshold()});

            var result = _sut.GetAll();

            //Then
            result.IsValid.Should().Be.True();
            result.Result.ToList().Count.Should().Be.EqualTo(preResult.Count + 3);
        }

        [Fact]
        public async void AddEntity_ThenResultIsValid_Async()
        {
            //Before
            var preResult = _sut.GetAll().Result.ToList();

            //Act
            await _sut.CreateAsync(GetPriceTreshold());

            var result = _sut.GetAll();

            //Then
            result.IsValid.Should().Be.True();
            result.Result.ToList().Count.Should().Be.EqualTo(preResult.Count + 1);
        }


        [Fact]
        public async void AddManyEntity_ThenResultIsValid_Async()
        {
            //Before
            var preResult = _sut.GetAll().Result.ToList();

            //Act
            await
                _sut.CreateManyAsync(new List<PriceTresholdBaseDto>
                {
                    GetPriceTreshold(),
                    GetPriceTreshold(),
                    GetPriceTreshold()
                });

            var result = _sut.GetAll();

            //Then
            result.IsValid.Should().Be.True();
            result.Result.ToList().Count.Should().Be.EqualTo(preResult.Count + 3);
        }

        [Fact]
        public void UpdateEntity_ThenResultIsValid()
        {
            InitContext();
            //Before
            var entites = _sut.GetAll().Result.ToList();
            var lastEntity = entites.Last();

            //Act
            lastEntity.MinCharges = 999;
            var editResult = _sut.Edit(lastEntity);

            //Then
            var result = _sut.GetAll();
            result.IsValid.Should().Be.True();
            editResult.IsValid.Should().Be.True();
            result.Result.ToList().Count.Should().Be.EqualTo(entites.Count);
            lastEntity.MinCharges.Should().Be.EqualTo(999);
        }

        [Fact]
        public void UpdateManyEntity_ThenResultIsValid()
        {
            InitContext();
            //Before
            var entites = _sut.GetAll().Result.ToList();
            var lastEntity = entites.Last();
            var firstEntity = entites.First();

            //Act
            lastEntity.MinCharges = 999;
            firstEntity.MinCharges = 123456;
            var editResult = _sut.EditMany(new List<PriceTresholdBaseDto> {lastEntity, firstEntity});

            //Then
            var result = _sut.GetAll();
            result.IsValid.Should().Be.True();
            result.Result.ToList().Count.Should().Be.EqualTo(entites.Count);
            editResult.IsValid.Should().Be.True();
            lastEntity.MinCharges.Should().Be.EqualTo(999);
        }

        [Fact]
        public async void UpdateEntity_ThenResultIsValid_Async()
        {
            InitContext();
            //Before
            var entites = _sut.GetAll().Result.ToList();
            var lastEntity = entites.Last();

            //Act
            lastEntity.MinCharges = 999;
            await _sut.EditAsync(lastEntity);

            //Then
            var result = _sut.GetAll();
            result.IsValid.Should().Be.True();
            result.Result.ToList().Count.Should().Be.EqualTo(entites.Count);
            lastEntity.MinCharges.Should().Be.EqualTo(999);
        }

        [Fact]
        public async void UpdateManyEntity_ThenResultIsValid_Async()
        {
            InitContext();
            //Before
            var entites = _sut.GetAll().Result.ToList();
            var lastEntity = entites.Last();
            var firstEntity = entites.First();

            //Act
            lastEntity.MinCharges = 999;
            firstEntity.MinCharges = 123456;
            var editResult = await _sut.EditManyAsync(new List<PriceTresholdBaseDto> {lastEntity, firstEntity});

            //Then
            var result = _sut.GetAll();
            result.IsValid.Should().Be.True();
            result.Result.ToList().Count.Should().Be.EqualTo(entites.Count);
            editResult.IsValid.Should().Be.True();
            lastEntity.MinCharges.Should().Be.EqualTo(999);
        }

        [Fact]
        public void DeleteLastEntity_ThenResultIsValid()
        {
            InitContext();
            //Before
            var entites = _sut.GetAll().Result.ToList();
            var lastEntity = entites.Last();

            //Act
            _sut.Delete(lastEntity);

            //Then
            var result = _sut.GetAll().Result.ToList();
            result.Count.Should().Be.EqualTo(entites.Count - 1);
        }

        [Fact]
        public async void DeleteLastEntity_ThenResultIsValid_Async()
        {
            InitContext();
            //Before
            var entites = _sut.GetAll().Result.ToList();
            var lastEntity = entites.Last();

            //Act
            await _sut.DeleteAsync(lastEntity);

            //Then
            var result = _sut.GetAll().Result.ToList();
            result.Count.Should().Be.EqualTo(entites.Count - 1);
        }

        [Fact]
        public void DeleteManyEntity_ThenResultIsValid()
        {
            InitContext();
            //Before
            var entites = _sut.GetAll().Result.ToList();
            var lastEntity = entites.Last();
            var firstEntity = entites.First();

            //Act
            _sut.DeleteMany(new List<PriceTresholdBaseDto> {lastEntity, firstEntity});

            //Then
            var result = _sut.GetAll().Result.ToList();
            result.Count.Should().Be.EqualTo(entites.Count - 2);
        }

        [Fact]
        public void DeleteManyEntity_ById_ThenResultIsValid()
        {
            InitContext();
            //Act
            var result = _sut.DeleteMany(new List<int> {1, 2});

            //Then
            result.IsValid.Should().Be.True();
        }

        [Fact]
        public async void DeleteManyEntityById_ThenResultIsValid_Async()
        {
            InitContext();
            //Act
            var result = await _sut.DeleteManyAsync(new List<int> {1, 2});

            //Then
            result.IsValid.Should().Be.True();
        }

        [Fact]
        public async void GetEntityById_ThenResultIsValid_Async()
        {
            //Act
            var result = await _sut.GetAsync(2);

            //Then
            result.Result.Should().Not.Be.Null();
            result.IsValid.Should().Be.True();
        }


        [Fact]
        public async void GetEntityByPredicate_ThenResultIsValid_Async()
        {
            //Act
            var result = await _sut.GetAsync(x => x.MinCharges > 1);

            //Then
            result.Result.Should().Not.Be.Null();
            result.IsValid.Should().Be.True();
        }

        [Fact]
        public void GetEntityByPredicate_ThenResultIsValid()
        {
            //Act
            var result = _sut.Get(x => x.MinCharges > 1);

            //Then
            result.Result.Should().Not.Be.Null();
            result.IsValid.Should().Be.True();
        }

        [Fact]
        public void GetAll_ThenResultIsValid()
        {
            //Act
            var result = _sut.GetAll();

            //Then
            result.Result.Should().Not.Be.Null();
            result.Result.Should().Have.Count.GreaterThan(0);
            result.IsValid.Should().Be.True();
        }

        [Fact]
        public async void GetAll_ThenResultIsValid_Async()
        {
            //Act
            var result = await _sut.GetAllAsync();

            //Then
            result.Result.Should().Not.Be.Null();
            result.Result.Should().Have.Count.GreaterThan(0);
            result.IsValid.Should().Be.True();
        }

        [Fact]
        public void GetAllWithPredicate_ThenResultIsValid()
        {
            //Act
            var result = _sut.GetAll(x => x.MinCharges > 0);

            //Then
            result.Result.Should().Not.Be.Null();
            result.Result.Should().Have.Count.GreaterThan(0);
            result.IsValid.Should().Be.True();
        }

        [Fact]
        public async void GetAllWithPredicate_ThenResultIsValid_Async()
        {
            //Act
            var result = await _sut.GetAllAsync(x => x.MinCharges > 0);

            //Then
            result.Result.Should().Not.Be.Null();
            result.Result.Should().Have.Count.GreaterThan(0);
            result.IsValid.Should().Be.True();
        }


        private void InitContext()
        {
            _mock.Mock<IDatabaseFactory>().Setup(x => x.Get()).Returns(GetContext());
            var repository = _mock.Create<PriceTresholdRepository>();
            var uow = _mock.Create<UnitOfWork>();

            _sut = new PriceTresholdService(uow, repository, Mapper);
        }

    }
}
