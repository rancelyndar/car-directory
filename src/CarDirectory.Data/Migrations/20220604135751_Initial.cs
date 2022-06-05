using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDirectory.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "car",
                columns: table => new
                {
                    state_number = table.Column<string>(type: "text", nullable: false),
                    model = table.Column<string>(type: "text", nullable: false),
                    color = table.Column<string>(type: "text", nullable: false),
                    release_year = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_car", x => x.state_number);
                });

            migrationBuilder.CreateTable(
                name: "fine",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    state_number = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    receipt_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_payed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_fine", x => x.id);
                    table.ForeignKey(
                        name: "fk_fine_car_car_state_number",
                        column: x => x.state_number,
                        principalTable: "car",
                        principalColumn: "state_number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_fine_state_number",
                table: "fine",
                column: "state_number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fine");

            migrationBuilder.DropTable(
                name: "car");
        }
    }
}
