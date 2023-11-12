using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.MIgrations
{
    /// <inheritdoc />
    public partial class NewMediaItemTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaItems");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Accounts");

            migrationBuilder.AddColumn<int>(
                name: "AvatarId",
                table: "Accounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BaseMediaItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AuthorId = table.Column<int>(type: "int", nullable: true),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseMediaItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseMediaItem_Accounts_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BaseToastWithContentToastMediaItem",
                columns: table => new
                {
                    BaseToastWithContentId = table.Column<int>(type: "int", nullable: false),
                    MediaItemsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseToastWithContentToastMediaItem", x => new { x.BaseToastWithContentId, x.MediaItemsId });
                    table.ForeignKey(
                        name: "FK_BaseToastWithContentToastMediaItem_BaseMediaItem_MediaItemsId",
                        column: x => x.MediaItemsId,
                        principalTable: "BaseMediaItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseToastWithContentToastMediaItem_BaseToasts_BaseToastWithC~",
                        column: x => x.BaseToastWithContentId,
                        principalTable: "BaseToasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AvatarId",
                table: "Accounts",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMediaItem_AuthorId",
                table: "BaseMediaItem",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseToastWithContentToastMediaItem_MediaItemsId",
                table: "BaseToastWithContentToastMediaItem",
                column: "MediaItemsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_BaseMediaItem_AvatarId",
                table: "Accounts",
                column: "AvatarId",
                principalTable: "BaseMediaItem",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_BaseMediaItem_AvatarId",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "BaseToastWithContentToastMediaItem");

            migrationBuilder.DropTable(
                name: "BaseMediaItem");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_AvatarId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Accounts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MediaItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AuthorId = table.Column<int>(type: "int", nullable: true),
                    BaseToastWithContentId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaItems_Accounts_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MediaItems_BaseToasts_BaseToastWithContentId",
                        column: x => x.BaseToastWithContentId,
                        principalTable: "BaseToasts",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MediaItems_AuthorId",
                table: "MediaItems",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaItems_BaseToastWithContentId",
                table: "MediaItems",
                column: "BaseToastWithContentId");
        }
    }
}
