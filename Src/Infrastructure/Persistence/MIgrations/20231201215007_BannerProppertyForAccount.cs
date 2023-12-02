using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.MIgrations
{
    /// <inheritdoc />
    public partial class BannerProppertyForAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MuteNotificationOptionUsers_MuteNotificationOptions_MuteNoti~",
                table: "MuteNotificationOptionUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_MuteNotificationOptionUsers_Users_UserId",
                table: "MuteNotificationOptionUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MuteNotificationOptionUsers",
                table: "MuteNotificationOptionUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MuteNotificationOptions",
                table: "MuteNotificationOptions");

            migrationBuilder.RenameTable(
                name: "MuteNotificationOptionUsers",
                newName: "MuteNotificationOptionUser");

            migrationBuilder.RenameTable(
                name: "MuteNotificationOptions",
                newName: "MuteNotificationOption");

            migrationBuilder.RenameIndex(
                name: "IX_MuteNotificationOptionUsers_UserId",
                table: "MuteNotificationOptionUser",
                newName: "IX_MuteNotificationOptionUser_UserId");

            migrationBuilder.AddColumn<int>(
                name: "BannerId",
                table: "Accounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MuteNotificationOptionUser",
                table: "MuteNotificationOptionUser",
                columns: new[] { "MuteNotificationOptionId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MuteNotificationOption",
                table: "MuteNotificationOption",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_BannerId",
                table: "Accounts",
                column: "BannerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_BaseMediaItem_BannerId",
                table: "Accounts",
                column: "BannerId",
                principalTable: "BaseMediaItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MuteNotificationOptionUser_MuteNotificationOption_MuteNotifi~",
                table: "MuteNotificationOptionUser",
                column: "MuteNotificationOptionId",
                principalTable: "MuteNotificationOption",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MuteNotificationOptionUser_Users_UserId",
                table: "MuteNotificationOptionUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_BaseMediaItem_BannerId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_MuteNotificationOptionUser_MuteNotificationOption_MuteNotifi~",
                table: "MuteNotificationOptionUser");

            migrationBuilder.DropForeignKey(
                name: "FK_MuteNotificationOptionUser_Users_UserId",
                table: "MuteNotificationOptionUser");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_BannerId",
                table: "Accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MuteNotificationOptionUser",
                table: "MuteNotificationOptionUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MuteNotificationOption",
                table: "MuteNotificationOption");

            migrationBuilder.DropColumn(
                name: "BannerId",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "MuteNotificationOptionUser",
                newName: "MuteNotificationOptionUsers");

            migrationBuilder.RenameTable(
                name: "MuteNotificationOption",
                newName: "MuteNotificationOptions");

            migrationBuilder.RenameIndex(
                name: "IX_MuteNotificationOptionUser_UserId",
                table: "MuteNotificationOptionUsers",
                newName: "IX_MuteNotificationOptionUsers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MuteNotificationOptionUsers",
                table: "MuteNotificationOptionUsers",
                columns: new[] { "MuteNotificationOptionId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MuteNotificationOptions",
                table: "MuteNotificationOptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MuteNotificationOptionUsers_MuteNotificationOptions_MuteNoti~",
                table: "MuteNotificationOptionUsers",
                column: "MuteNotificationOptionId",
                principalTable: "MuteNotificationOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MuteNotificationOptionUsers_Users_UserId",
                table: "MuteNotificationOptionUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
