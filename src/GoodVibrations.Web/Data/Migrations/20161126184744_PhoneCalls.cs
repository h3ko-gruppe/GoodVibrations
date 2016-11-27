using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GoodVibrations.Web.Data.Migrations
{
    public partial class PhoneCalls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhoneCall",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CurrentLocation = table.Column<string>(nullable: true),
                    FromPhoneNumber = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: false),
                    ToPhoneNumber = table.Column<string>(nullable: false),
                    Token = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneCall", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhoneCall_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhoneCall_UserId",
                table: "PhoneCall",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhoneCall");
        }
    }
}
