using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tms.Migrations
{
    /// <inheritdoc />
    public partial class Ticket4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NepaliDate",
                table: "Tickets");

            migrationBuilder.AddColumn<int>(
                name: "NepaliDate_Day",
                table: "Tickets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NepaliDate_Month",
                table: "Tickets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NepaliDate_Year",
                table: "Tickets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NepaliDate_Day",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "NepaliDate_Month",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "NepaliDate_Year",
                table: "Tickets");

            migrationBuilder.AddColumn<string>(
                name: "NepaliDate",
                table: "Tickets",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
