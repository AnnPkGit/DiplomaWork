using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.MIgrations
{
    /// <inheritdoc />
    public partial class Add_Toasts_Infrastructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeactivatedById",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeactivatedById",
                table: "Accounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MediaItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItems", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Toasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    Context = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    QuoteId = table.Column<int>(type: "int", nullable: true),
                    Deactivated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeactivatedById = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toasts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Toasts_Accounts_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Toasts_Toasts_QuoteId",
                        column: x => x.QuoteId,
                        principalTable: "Toasts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Toasts_Users_DeactivatedById",
                        column: x => x.DeactivatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MediaItemId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reactions_MediaItems_MediaItemId",
                        column: x => x.MediaItemId,
                        principalTable: "MediaItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ReToasts",
                columns: table => new
                {
                    ToastId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReToasts", x => new { x.ToastId, x.AccountId });
                    table.ForeignKey(
                        name: "FK_ReToasts_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReToasts_Toasts_ToastId",
                        column: x => x.ToastId,
                        principalTable: "Toasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ToastMediaItem",
                columns: table => new
                {
                    ToastId = table.Column<int>(type: "int", nullable: false),
                    MediaItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToastMediaItem", x => new { x.ToastId, x.MediaItemId });
                    table.ForeignKey(
                        name: "FK_ToastMediaItem_MediaItems_MediaItemId",
                        column: x => x.MediaItemId,
                        principalTable: "MediaItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToastMediaItem_Toasts_ToastId",
                        column: x => x.ToastId,
                        principalTable: "Toasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ToastReaction",
                columns: table => new
                {
                    ToastId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ReactionId = table.Column<int>(type: "int", nullable: false),
                    Reacted = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToastReaction", x => new { x.ToastId, x.AccountId });
                    table.ForeignKey(
                        name: "FK_ToastReaction_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToastReaction_Reactions_ReactionId",
                        column: x => x.ReactionId,
                        principalTable: "Reactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToastReaction_Toasts_ToastId",
                        column: x => x.ToastId,
                        principalTable: "Toasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DeactivatedById",
                table: "Users",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_DeactivatedById",
                table: "Accounts",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_MediaItemId",
                table: "Reactions",
                column: "MediaItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReToasts_AccountId",
                table: "ReToasts",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ToastMediaItem_MediaItemId",
                table: "ToastMediaItem",
                column: "MediaItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ToastReaction_AccountId",
                table: "ToastReaction",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ToastReaction_ReactionId",
                table: "ToastReaction",
                column: "ReactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Toasts_AuthorId",
                table: "Toasts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Toasts_DeactivatedById",
                table: "Toasts",
                column: "DeactivatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Toasts_QuoteId",
                table: "Toasts",
                column: "QuoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Users_DeactivatedById",
                table: "Accounts",
                column: "DeactivatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_DeactivatedById",
                table: "Users",
                column: "DeactivatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Users_DeactivatedById",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_DeactivatedById",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ReToasts");

            migrationBuilder.DropTable(
                name: "ToastMediaItem");

            migrationBuilder.DropTable(
                name: "ToastReaction");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "Toasts");

            migrationBuilder.DropTable(
                name: "MediaItems");

            migrationBuilder.DropIndex(
                name: "IX_Users_DeactivatedById",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_DeactivatedById",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "DeactivatedById",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeactivatedById",
                table: "Accounts");
        }
    }
}
