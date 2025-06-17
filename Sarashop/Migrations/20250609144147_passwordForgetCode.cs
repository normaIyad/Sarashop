﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sarashop.Migrations
{
    /// <inheritdoc />
    public partial class passwordForgetCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "passwordResetCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    applecationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExprationCode = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passwordResetCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_passwordResetCodes_AspNetUsers_applecationUserId",
                        column: x => x.applecationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_passwordResetCodes_applecationUserId",
                table: "passwordResetCodes",
                column: "applecationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "passwordResetCodes");
        }
    }
}
