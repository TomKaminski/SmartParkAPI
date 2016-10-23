using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SmartParkAPI.Model;

namespace SmartParkAPI.Model.Migrations
{
    [DbContext(typeof(ParkingAthContext))]
    [Migration("20161023192523_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SmartParkAPI.Model.Concrete.GateUsage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateOfUse");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("GateUsage");
                });

            modelBuilder.Entity("SmartParkAPI.Model.Concrete.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BCC");

                    b.Property<string>("CC");

                    b.Property<string>("DisplayFrom");

                    b.Property<string>("From");

                    b.Property<string>("MessageParameters");

                    b.Property<string>("Title");

                    b.Property<string>("To");

                    b.Property<int>("Type");

                    b.Property<int>("UserId");

                    b.Property<long>("ViewInBrowserTokenId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("ViewInBrowserTokenId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("SmartParkAPI.Model.Concrete.Order", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<Guid>("ExtOrderId");

                    b.Property<int>("NumOfCharges");

                    b.Property<int>("OrderPlace");

                    b.Property<int>("OrderState");

                    b.Property<decimal>("Price");

                    b.Property<int>("PriceTresholdId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PriceTresholdId");

                    b.HasIndex("UserId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("SmartParkAPI.Model.Concrete.PortalMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<bool>("HiddenForReceiver");

                    b.Property<bool>("HiddenForSender");

                    b.Property<bool>("IsDisplayed");

                    b.Property<bool>("IsNotification");

                    b.Property<int>("PortalMessageType");

                    b.Property<Guid?>("PreviousMessageId");

                    b.Property<int>("ReceiverUserId");

                    b.Property<bool>("Starter");

                    b.Property<string>("Text");

                    b.Property<string>("Title");

                    b.Property<bool>("ToAdmin");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PreviousMessageId");

                    b.HasIndex("UserId");

                    b.ToTable("PortalMessage");
                });

            modelBuilder.Entity("SmartParkAPI.Model.Concrete.PriceTreshold", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("MinCharges");

                    b.Property<decimal>("PricePerCharge");

                    b.HasKey("Id");

                    b.ToTable("PriceTreshold");
                });

            modelBuilder.Entity("SmartParkAPI.Model.Concrete.Token", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid>("SecureToken");

                    b.Property<int>("TokenType");

                    b.Property<int>("UserId");

                    b.Property<DateTime?>("ValidTo");

                    b.HasKey("Id");

                    b.ToTable("Token");
                });

            modelBuilder.Entity("SmartParkAPI.Model.Concrete.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Charges");

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Email");

                    b.Property<bool>("IsAdmin");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockedOut");

                    b.Property<DateTime?>("LockedTo");

                    b.Property<string>("Name");

                    b.Property<long?>("PasswordChangeTokenId");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PasswordSalt");

                    b.Property<int>("UnsuccessfulLoginAttempts");

                    b.Property<int>("UserPreferencesId");

                    b.HasKey("Id");

                    b.HasIndex("PasswordChangeTokenId")
                        .IsUnique();

                    b.HasIndex("UserPreferencesId")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("SmartParkAPI.Model.Concrete.UserPreferences", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("ProfilePhoto");

                    b.Property<Guid?>("ProfilePhotoId");

                    b.Property<bool>("ShrinkedSidebar");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserPreferences");
                });

            modelBuilder.Entity("SmartParkAPI.Model.Concrete.Weather", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Clouds");

                    b.Property<DateTime>("DateOfRead");

                    b.Property<double>("Humidity");

                    b.Property<double>("Pressure");

                    b.Property<double>("Temperature");

                    b.Property<DateTime>("ValidToDate");

                    b.HasKey("Id");

                    b.ToTable("Weather");
                });

            modelBuilder.Entity("SmartParkAPI.Model.Concrete.WeatherInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("WeatherConditionId");

                    b.Property<string>("WeatherDescription");

                    b.Property<Guid>("WeatherId");

                    b.Property<string>("WeatherMain");

                    b.HasKey("Id");

                    b.HasIndex("WeatherId");

                    b.ToTable("WeatherInfo");
                });

            modelBuilder.Entity("SmartParkAPI.Model.Concrete.GateUsage", b =>
                {
                    b.HasOne("SmartParkAPI.Model.Concrete.User", "User")
                        .WithMany("GateUsages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartParkAPI.Model.Concrete.Message", b =>
                {
                    b.HasOne("SmartParkAPI.Model.Concrete.User", "User")
                        .WithMany("UserMessages")
                        .HasForeignKey("UserId");

                    b.HasOne("SmartParkAPI.Model.Concrete.Token", "ViewInBrowserToken")
                        .WithMany()
                        .HasForeignKey("ViewInBrowserTokenId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartParkAPI.Model.Concrete.Order", b =>
                {
                    b.HasOne("SmartParkAPI.Model.Concrete.PriceTreshold", "PriceTreshold")
                        .WithMany("Orders")
                        .HasForeignKey("PriceTresholdId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartParkAPI.Model.Concrete.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartParkAPI.Model.Concrete.PortalMessage", b =>
                {
                    b.HasOne("SmartParkAPI.Model.Concrete.PortalMessage", "PreviousMessage")
                        .WithMany()
                        .HasForeignKey("PreviousMessageId");

                    b.HasOne("SmartParkAPI.Model.Concrete.User")
                        .WithMany("UserPortalMessages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartParkAPI.Model.Concrete.User", b =>
                {
                    b.HasOne("SmartParkAPI.Model.Concrete.Token", "PasswordChangeToken")
                        .WithOne("User")
                        .HasForeignKey("SmartParkAPI.Model.Concrete.User", "PasswordChangeTokenId");

                    b.HasOne("SmartParkAPI.Model.Concrete.UserPreferences", "UserPreferences")
                        .WithOne("User")
                        .HasForeignKey("SmartParkAPI.Model.Concrete.User", "UserPreferencesId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartParkAPI.Model.Concrete.WeatherInfo", b =>
                {
                    b.HasOne("SmartParkAPI.Model.Concrete.Weather", "Weather")
                        .WithMany("WeatherInfo")
                        .HasForeignKey("WeatherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
