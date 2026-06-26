using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cashback.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWebhookEventIdempotencyIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProviderOrderId",
                table: "WebhookEvents",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebhookEvents_Provider_ProviderOrderId",
                table: "WebhookEvents",
                columns: new[] { "Provider", "ProviderOrderId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WebhookEvents_Provider_ProviderOrderId",
                table: "WebhookEvents");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderOrderId",
                table: "WebhookEvents",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);
        }
    }
}
