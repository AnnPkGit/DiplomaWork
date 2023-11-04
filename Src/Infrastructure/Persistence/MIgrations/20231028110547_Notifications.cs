using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.MIgrations
{
    /// <inheritdoc />
    public partial class Notifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ToAccountId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Viewed = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    QuoteId = table.Column<int>(type: "int", nullable: true),
                    ReToastId = table.Column<int>(type: "int", nullable: true),
                    ReactionId = table.Column<int>(type: "int", nullable: true),
                    ReplyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseNotifications_Accounts_ToAccountId",
                        column: x => x.ToAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseNotifications_BaseToasts_QuoteId",
                        column: x => x.QuoteId,
                        principalTable: "BaseToasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseNotifications_BaseToasts_ReToastId",
                        column: x => x.ReToastId,
                        principalTable: "BaseToasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseNotifications_BaseToasts_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "BaseToasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseNotifications_Reactions_ReactionId",
                        column: x => x.ReactionId,
                        principalTable: "Reactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BaseNotifications_QuoteId",
                table: "BaseNotifications",
                column: "QuoteId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseNotifications_ReactionId",
                table: "BaseNotifications",
                column: "ReactionId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseNotifications_ReplyId",
                table: "BaseNotifications",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseNotifications_ReToastId",
                table: "BaseNotifications",
                column: "ReToastId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseNotifications_ToAccountId",
                table: "BaseNotifications",
                column: "ToAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseNotifications");
        }
    }
}
