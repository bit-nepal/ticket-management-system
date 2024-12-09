using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tms.Migrations
{
    /// <inheritdoc />
    public partial class ticket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TicketNo = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NepaliDate_Year = table.Column<int>(type: "INTEGER", nullable: false),
                    NepaliDate_Month = table.Column<int>(type: "INTEGER", nullable: false),
                    NepaliDate_Day = table.Column<int>(type: "INTEGER", nullable: false),
                    BarCodeData = table.Column<string>(type: "TEXT", nullable: false),
                    Nationality = table.Column<int>(type: "INTEGER", nullable: false),
                    PersonType = table.Column<int>(type: "INTEGER", nullable: false),
                    NoOfPeople = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalPrice = table.Column<int>(type: "INTEGER", nullable: false),
                    IsGroupVisit = table.Column<bool>(type: "INTEGER", nullable: false),
                    CustomText = table.Column<string>(type: "TEXT", nullable: true),
                    GroupSize = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.id);
                });

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
                name: "AddOn",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AddOnType = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalPrice = table.Column<int>(type: "INTEGER", nullable: false),
                    IsSelected = table.Column<bool>(type: "INTEGER", nullable: false),
                    TicketId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddOn", x => x.id);
                    table.ForeignKey(
                        name: "FK_AddOn_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "id");
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
                    TicketNoStart = table.Column<int>(type: "INTEGER", nullable: true),
                    TicketNoEnd = table.Column<int>(type: "INTEGER", nullable: true),
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
                name: "IX_AddOn_TicketId",
                table: "AddOn",
                column: "TicketId");

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
                name: "AddOn");

            migrationBuilder.DropTable(
                name: "RevenueCells");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "DailyRevenues");

            migrationBuilder.DropTable(
                name: "MonthlyRevenues");

            migrationBuilder.DropTable(
                name: "YearlyRevenues");
        }
    }
}
