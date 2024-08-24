using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CityInfo.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "It is small peaceful semi urban area located in the morang district.", "Gothgaun" },
                    { 2, "It is the beautiful place located in the satdobato, lalitpur.", "Nakhipot" },
                    { 3, "It is city located in the Sunsari district and important business hub of eastern Nepal.", "Itahari" }
                });

            migrationBuilder.InsertData(
                table: "PointOfInterests",
                columns: new[] { "Id", "CityId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 1, "The PU is situated here.", "Purbanchal University" },
                    { 2, 1, "The oldest government school in the gothgaun.", "Panchayat School" },
                    { 3, 2, "Kirat Yakthung Chumlung is a social organization of the Limbu indigenous ethnic group of Nepal.", "Kirat Yakthung Chumlung" },
                    { 4, 2, "Ullens School is a private school in Nepal that offers holistic education from Kindergarten to IBDP.", "Ullens School" },
                    { 5, 2, "It is the biggest goods store in the Satdobato.", "Bhatbhateni Store" },
                    { 6, 3, "It is the oldest department store in the Itahari.", "Gorkha Department" },
                    { 7, 3, "It is a distinguished educational institution nestled in the heart of Itahari Sunsari.", "Namuna College" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
