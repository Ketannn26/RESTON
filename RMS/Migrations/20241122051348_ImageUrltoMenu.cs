﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Migrations
{
    /// <inheritdoc />
    public partial class ImageUrltoMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Menus");
        }
    }
}
