using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.MIgrations
{
    /// <inheritdoc />
    public partial class FollowerSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FollowerId",
                table: "BaseNotifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Follows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FollowFromId = table.Column<int>(type: "int", nullable: false),
                    FollowToId = table.Column<int>(type: "int", nullable: false),
                    DateOfFollow = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Follows_Accounts_FollowFromId",
                        column: x => x.FollowFromId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Follows_Accounts_FollowToId",
                        column: x => x.FollowToId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BaseNotifications_FollowerId",
                table: "BaseNotifications",
                column: "FollowerId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_FollowFromId",
                table: "Follows",
                column: "FollowFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_FollowToId",
                table: "Follows",
                column: "FollowToId");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseNotifications_Accounts_FollowerId",
                table: "BaseNotifications",
                column: "FollowerId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseNotifications_Accounts_FollowerId",
                table: "BaseNotifications");

            migrationBuilder.DropTable(
                name: "Follows");

            migrationBuilder.DropIndex(
                name: "IX_BaseNotifications_FollowerId",
                table: "BaseNotifications");

            migrationBuilder.DropColumn(
                name: "FollowerId",
                table: "BaseNotifications");
        }
    }
}
