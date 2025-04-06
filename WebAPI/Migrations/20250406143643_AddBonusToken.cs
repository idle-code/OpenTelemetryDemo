using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddBonusToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BonusToken",
                schema: "TheButton",
                columns: table => new
                {
                    Token = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CounterId = table.Column<string>(type: "character varying(64)", nullable: false),
                    ValidUntil = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusToken", x => new { x.CounterId, x.Token });
                    table.ForeignKey(
                        name: "FK_BonusToken_NamedCounters_CounterId",
                        column: x => x.CounterId,
                        principalSchema: "TheButton",
                        principalTable: "NamedCounters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BonusToken",
                schema: "TheButton");
        }
    }
}
