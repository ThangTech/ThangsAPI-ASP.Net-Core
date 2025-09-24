using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ThangAPI.Migrations
{
    /// <inheritdoc />
    public partial class Seeddatafordifficultitesandregions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("245e8287-3b85-41bb-9303-6c2f52ff26eb"), "Normal" },
                    { new Guid("360a04ba-90b0-4d45-90f3-77e89eb77f79"), "Hard" },
                    { new Guid("bdff050c-6d1e-4693-ac1a-ee180519a28d"), "Easy" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageURL" },
                values: new object[,]
                {
                    { new Guid("242c94d0-f607-4c35-acd8-b5d48394d8de"), "MAN", "Manchester", "manchester.jpg" },
                    { new Guid("2b36212b-ce30-46e5-bc7d-9aae18decdc2"), "MUN", "Munich", "munich.jpg" },
                    { new Guid("4c4044e0-59da-4070-b4be-4ee6c7dcefed"), "LON", "London", "london.jpg" },
                    { new Guid("8779c4b0-b32d-4b32-8cd7-1a219467a256"), "HAN", "Ha noiu", "hanoi.jpg" },
                    { new Guid("d154e851-3c7f-4e9f-b6ff-1030701ec74f"), "BER", "Berlin", "berlin.jpg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("245e8287-3b85-41bb-9303-6c2f52ff26eb"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("360a04ba-90b0-4d45-90f3-77e89eb77f79"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("bdff050c-6d1e-4693-ac1a-ee180519a28d"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("242c94d0-f607-4c35-acd8-b5d48394d8de"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("2b36212b-ce30-46e5-bc7d-9aae18decdc2"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("4c4044e0-59da-4070-b4be-4ee6c7dcefed"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("8779c4b0-b32d-4b32-8cd7-1a219467a256"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("d154e851-3c7f-4e9f-b6ff-1030701ec74f"));
        }
    }
}
