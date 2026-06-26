using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cashback.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAffiliateLinkSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AffiliateLinks_SubId",
                table: "AffiliateLinks");

            migrationBuilder.DropColumn(
                name: "Merchant",
                table: "AffiliateLinks");

            migrationBuilder.RenameColumn(
                name: "SubId",
                table: "AffiliateLinks",
                newName: "Sub1");

            migrationBuilder.CreateIndex(
                name: "IX_AffiliateLinks_Sub1",
                table: "AffiliateLinks",
                column: "Sub1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AffiliateLinks_Sub1",
                table: "AffiliateLinks");

            migrationBuilder.RenameColumn(
                name: "Sub1",
                table: "AffiliateLinks",
                newName: "SubId");

            migrationBuilder.AddColumn<int>(
                name: "Merchant",
                table: "AffiliateLinks",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AffiliateLinks_SubId",
                table: "AffiliateLinks",
                column: "SubId",
                unique: true);
        }
    }
}
