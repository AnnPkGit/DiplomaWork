using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.MIgrations
{
    /// <inheritdoc />
    public partial class MuteNotificationsOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseToasts_BaseToasts_QuotedToastId",
                table: "BaseToasts");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseToasts_BaseToasts_ReplyToToastId",
                table: "BaseToasts");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseToasts_BaseToasts_ToastWithContentId",
                table: "BaseToasts");

            migrationBuilder.CreateTable(
                name: "MuteNotificationOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuteNotificationOptions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MuteNotificationOptionUsers",
                columns: table => new
                {
                    MuteNotificationOptionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuteNotificationOptionUsers", x => new { x.MuteNotificationOptionId, x.UserId });
                    table.ForeignKey(
                        name: "FK_MuteNotificationOptionUsers_MuteNotificationOptions_MuteNoti~",
                        column: x => x.MuteNotificationOptionId,
                        principalTable: "MuteNotificationOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MuteNotificationOptionUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "MuteNotificationOptions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "YouDoNotFollow" },
                    { 2, "WhoDoNotFollowYou" },
                    { 3, "WithANewAccount" },
                    { 4, "WhoHaveDefaultProfilePhoto" },
                    { 5, "WhoHaveNotConfirmedTheirEmail" },
                    { 6, "WhoHaveNotConfirmedTheirPhoneNumber" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MuteNotificationOptionUsers_UserId",
                table: "MuteNotificationOptionUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseToasts_BaseToasts_QuotedToastId",
                table: "BaseToasts",
                column: "QuotedToastId",
                principalTable: "BaseToasts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseToasts_BaseToasts_ReplyToToastId",
                table: "BaseToasts",
                column: "ReplyToToastId",
                principalTable: "BaseToasts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseToasts_BaseToasts_ToastWithContentId",
                table: "BaseToasts",
                column: "ToastWithContentId",
                principalTable: "BaseToasts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseToasts_BaseToasts_QuotedToastId",
                table: "BaseToasts");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseToasts_BaseToasts_ReplyToToastId",
                table: "BaseToasts");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseToasts_BaseToasts_ToastWithContentId",
                table: "BaseToasts");

            migrationBuilder.DropTable(
                name: "MuteNotificationOptionUsers");

            migrationBuilder.DropTable(
                name: "MuteNotificationOptions");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseToasts_BaseToasts_QuotedToastId",
                table: "BaseToasts",
                column: "QuotedToastId",
                principalTable: "BaseToasts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseToasts_BaseToasts_ReplyToToastId",
                table: "BaseToasts",
                column: "ReplyToToastId",
                principalTable: "BaseToasts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseToasts_BaseToasts_ToastWithContentId",
                table: "BaseToasts",
                column: "ToastWithContentId",
                principalTable: "BaseToasts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
