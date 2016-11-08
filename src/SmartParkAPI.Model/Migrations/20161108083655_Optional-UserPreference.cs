using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartParkAPI.Model.Migrations
{
    public partial class OptionalUserPreference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_UserPreferences_UserPreferencesId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_UserPreferencesId",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "UserPreferencesId1",
                table: "User",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserPreferences",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserPreferencesId1",
                table: "User",
                column: "UserPreferencesId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_UserPreferences_UserPreferencesId1",
                table: "User",
                column: "UserPreferencesId1",
                principalTable: "UserPreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_UserPreferences_UserPreferencesId1",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_UserPreferencesId1",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserPreferencesId1",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserPreferences",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserPreferencesId",
                table: "User",
                column: "UserPreferencesId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_UserPreferences_UserPreferencesId",
                table: "User",
                column: "UserPreferencesId",
                principalTable: "UserPreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
