using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoAnalyzer.Prediction.Domain.Migrations
{
    /// <inheritdoc />
    public partial class HistoryPrediction2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Grade = table.Column<double>(type: "double precision", nullable: true),
                    isGenerated = table.Column<bool>(type: "boolean", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PredictionHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CoinId = table.Column<string>(type: "text", nullable: false),
                    PricePoints = table.Column<string>(type: "text", nullable: false),
                    UserEmail = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredictionHistories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_News_Text",
                table: "News",
                column: "Text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "PredictionHistories");
        }
    }
}
