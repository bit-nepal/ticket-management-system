using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tms.Migrations
{
    /// <inheritdoc />
    public partial class Ticket2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "YearlyRevenues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Year = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearlyRevenues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyRevenues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Month = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    YearlyRevenueId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyRevenues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyRevenues_YearlyRevenues_YearlyRevenueId",
                        column: x => x.YearlyRevenueId,
                        principalTable: "YearlyRevenues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyRevenues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateAD = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateBS_Year = table.Column<int>(type: "INTEGER", nullable: false),
                    DateBS_Month = table.Column<int>(type: "INTEGER", nullable: false),
                    DateBS_Day = table.Column<int>(type: "INTEGER", nullable: false),
                    MonthlyRevenueId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyRevenues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyRevenues_MonthlyRevenues_MonthlyRevenueId",
                        column: x => x.MonthlyRevenueId,
                        principalTable: "MonthlyRevenues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RevenueCells",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    NoOfPeople = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    DailyRevenueId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevenueCells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RevenueCells_DailyRevenues_DailyRevenueId",
                        column: x => x.DailyRevenueId,
                        principalTable: "DailyRevenues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyRevenues_MonthlyRevenueId",
                table: "DailyRevenues",
                column: "MonthlyRevenueId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyRevenues_YearlyRevenueId",
                table: "MonthlyRevenues",
                column: "YearlyRevenueId");

            migrationBuilder.CreateIndex(
                name: "IX_RevenueCells_DailyRevenueId",
                table: "RevenueCells",
                column: "DailyRevenueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevenueCells");

            migrationBuilder.DropTable(
                name: "DailyRevenues");

            migrationBuilder.DropTable(
                name: "MonthlyRevenues");

            migrationBuilder.DropTable(
                name: "YearlyRevenues");
        }
    }
}
