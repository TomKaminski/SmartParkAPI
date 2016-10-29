using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartParkAPI.Model.Migrations
{
    public partial class RemovedCanAccessProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanAccess",
                table: "UserDevice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanAccess",
                table: "UserDevice",
                nullable: false,
                defaultValue: false);
        }
    }
}
