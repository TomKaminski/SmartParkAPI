using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SmartParkAPI.Model.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceTreshold",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MinCharges = table.Column<int>(nullable: false),
                    PricePerCharge = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceTreshold", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    SecureToken = table.Column<Guid>(nullable: false),
                    TokenType = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ValidTo = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProfilePhoto = table.Column<byte[]>(nullable: true),
                    ProfilePhotoId = table.Column<Guid>(nullable: true),
                    ShrinkedSidebar = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Weather",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Clouds = table.Column<int>(nullable: false),
                    DateOfRead = table.Column<DateTime>(nullable: false),
                    Humidity = table.Column<double>(nullable: false),
                    Pressure = table.Column<double>(nullable: false),
                    Temperature = table.Column<double>(nullable: false),
                    ValidToDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weather", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Charges = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    LockedOut = table.Column<bool>(nullable: false),
                    LockedTo = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PasswordChangeTokenId = table.Column<long>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PasswordSalt = table.Column<string>(nullable: true),
                    UnsuccessfulLoginAttempts = table.Column<int>(nullable: false),
                    UserPreferencesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Token_PasswordChangeTokenId",
                        column: x => x.PasswordChangeTokenId,
                        principalTable: "Token",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_UserPreferences_UserPreferencesId",
                        column: x => x.UserPreferencesId,
                        principalTable: "UserPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeatherInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WeatherConditionId = table.Column<int>(nullable: false),
                    WeatherDescription = table.Column<string>(nullable: true),
                    WeatherId = table.Column<Guid>(nullable: false),
                    WeatherMain = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherInfo_Weather_WeatherId",
                        column: x => x.WeatherId,
                        principalTable: "Weather",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GateUsage",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateOfUse = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GateUsage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GateUsage_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BCC = table.Column<string>(nullable: true),
                    CC = table.Column<string>(nullable: true),
                    DisplayFrom = table.Column<string>(nullable: true),
                    From = table.Column<string>(nullable: true),
                    MessageParameters = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ViewInBrowserTokenId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_Token_ViewInBrowserTokenId",
                        column: x => x.ViewInBrowserTokenId,
                        principalTable: "Token",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    ExtOrderId = table.Column<Guid>(nullable: false),
                    NumOfCharges = table.Column<int>(nullable: false),
                    OrderPlace = table.Column<int>(nullable: false),
                    OrderState = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    PriceTresholdId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_PriceTreshold_PriceTresholdId",
                        column: x => x.PriceTresholdId,
                        principalTable: "PriceTreshold",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PortalMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    HiddenForReceiver = table.Column<bool>(nullable: false),
                    HiddenForSender = table.Column<bool>(nullable: false),
                    IsDisplayed = table.Column<bool>(nullable: false),
                    IsNotification = table.Column<bool>(nullable: false),
                    PortalMessageType = table.Column<int>(nullable: false),
                    PreviousMessageId = table.Column<Guid>(nullable: true),
                    ReceiverUserId = table.Column<int>(nullable: false),
                    Starter = table.Column<bool>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    ToAdmin = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortalMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortalMessage_PortalMessage_PreviousMessageId",
                        column: x => x.PreviousMessageId,
                        principalTable: "PortalMessage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PortalMessage_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GateUsage_UserId",
                table: "GateUsage",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserId",
                table: "Message",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ViewInBrowserTokenId",
                table: "Message",
                column: "ViewInBrowserTokenId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PriceTresholdId",
                table: "Order",
                column: "PriceTresholdId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PortalMessage_PreviousMessageId",
                table: "PortalMessage",
                column: "PreviousMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_PortalMessage_UserId",
                table: "PortalMessage",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_PasswordChangeTokenId",
                table: "User",
                column: "PasswordChangeTokenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserPreferencesId",
                table: "User",
                column: "UserPreferencesId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeatherInfo_WeatherId",
                table: "WeatherInfo",
                column: "WeatherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GateUsage");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "PortalMessage");

            migrationBuilder.DropTable(
                name: "WeatherInfo");

            migrationBuilder.DropTable(
                name: "PriceTreshold");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Weather");

            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.DropTable(
                name: "UserPreferences");
        }
    }
}
