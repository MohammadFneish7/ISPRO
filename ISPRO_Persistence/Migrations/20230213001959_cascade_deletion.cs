using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISPRO.Persistence.Migrations
{
    public partial class cascade_deletion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrePaidCards_Subscriptions_SubscriptionId",
                table: "PrePaidCards");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Subscriptions_SubscriptionId",
                table: "UserAccounts");

            migrationBuilder.AddForeignKey(
                name: "FK_PrePaidCards_Subscriptions_SubscriptionId",
                table: "PrePaidCards",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Subscriptions_SubscriptionId",
                table: "UserAccounts",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrePaidCards_Subscriptions_SubscriptionId",
                table: "PrePaidCards");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Subscriptions_SubscriptionId",
                table: "UserAccounts");

            migrationBuilder.AddForeignKey(
                name: "FK_PrePaidCards_Subscriptions_SubscriptionId",
                table: "PrePaidCards",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Subscriptions_SubscriptionId",
                table: "UserAccounts",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id");
        }
    }
}
