using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddManagerConstraintsFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Managers_ManagerId",
                table: "Hotels");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_Email",
                table: "Managers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Managers_PersonalNumber",
                table: "Managers",
                column: "PersonalNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Managers_ManagerId",
                table: "Hotels",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Managers_ManagerId",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Managers_Email",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Managers_PersonalNumber",
                table: "Managers");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Managers_ManagerId",
                table: "Hotels",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
