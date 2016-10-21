using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartParkAPI.Business.Providers;
using SmartParkAPI.Contracts.DTO;
using SmartParkAPI.Contracts.DTO.PriceTreshold;
using SmartParkAPI.Contracts.DTO.Token;
using SmartParkAPI.Contracts.DTO.User;
using SmartParkAPI.Model;
using SmartParkAPI.Resolver.Mappings;
using SmartParkAPI.Shared.Enums;
using SmartParkAPI.Shared.Helpers;

namespace SmartParkAPI.Business.Tests.Base
{
    public abstract class BusinessTestBase
    {
        protected const string TestLayoutRegister = @"<div><h1>Email layout {{TekstLayoutPlz}}</h1><p>{{UserName}}</p><br/><br/><div>{{BodyHtml}}</div></div>";
        protected const string EmailBodyRegister = @"<div><h3>Email boooody Register</h3></div>";
        protected const string FullEmailRegister = @"<div><h1>Email layout Hahahahahahaha</h1><p>Tomek Kamiński</p><br/><br/><div><div><h3>Email boooody Register</h3></div></div></div>";

        protected const string EmailBodyChangeReset = @"<div><h3>Email boooody ChangeReset</h3></div>";
        protected const string FullEmailChangeReset = @"<div><h1>Email layout Hahahahahahaha</h1><p>Tomek Kamiński</p><br/><br/><div><div><h3>Email boooody ChangeReset</h3></div></div></div>";

        protected const string BasicUserPassword = "Password123";

        protected readonly Dictionary<string, string> CustomParameters = new Dictionary<string, string>
        {
            { "UserName", "Tomek Kamiński" },
            { "TekstLayoutPlz", "Hahahahahahaha" }
        };

        protected IMapper Mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new GateUsageBackendMappings());
            cfg.AddProfile(new WeatherBackendMappings());
            cfg.AddProfile(new MessageBackendMappings());
            cfg.AddProfile(new OrderBackendMappings());
            cfg.AddProfile(new UserBackendMappings());
            cfg.AddProfile(new TokenBackendMappings());
            cfg.AddProfile(new PriceTresholdBackendMappings());
        }).CreateMapper();

        protected static PriceTresholdBaseDto GetPriceTreshold()
        {
            return new PriceTresholdBaseDto
            {
                MinCharges = 5,
                PricePerCharge = 5.5m
            };
        }

        protected static TokenBaseDto GetTokenDto(TokenType type)
        {
            return new TokenBaseDto
            {
                SecureToken = Guid.NewGuid(),
                TokenType = type,
                ValidTo = TokenValidityTimeProvider.GetValidToDate(type)
            };
        }

        protected ParkingAthContext GetContext()
        {
            var context = new ParkingAthContext(true);
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context;
        }

        protected MessageDto GetBaseMessageDto(EmailType type)
        {
            return new MessageDto
            {
                Type = type,
                From = "parkingath@gmail.com",
                To = "tkaminski93@gmail.com",
                MessageParameters = JsonConvert.SerializeObject(CustomParameters),
                DisplayFrom = "adsadssad"
            };
        }


        protected static UserBaseDto GetUserBaseDto()
        {
            var passwordHash = new PasswordHasher();
            var result = passwordHash.CreateHash(BasicUserPassword);

            return new UserBaseDto
            {
                Email = Guid.NewGuid().ToString("N")+"@wp312312addas.pl",
                Charges = 300,
                IsAdmin = false,
                LastName = "Kamiński",
                LockedOut = false,
                LockedTo = null,
                UnsuccessfulLoginAttempts = 0,
                Name = "Tomasz",
                PasswordHash = result.Hash,
                PasswordSalt = result.Salt
            };
        }
    }
}
