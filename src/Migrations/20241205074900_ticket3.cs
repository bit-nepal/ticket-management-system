using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tms.Migrations
{
    /// <inheritdoc />
    public partial class ticket3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketNoEnd",
                table: "DailyRevenues",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TicketNoStart",
                table: "DailyRevenues",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketNoEnd",
                table: "DailyRevenues");

            migrationBuilder.DropColumn(
                name: "TicketNoStart",
                table: "DailyRevenues");
        }
    }
}
