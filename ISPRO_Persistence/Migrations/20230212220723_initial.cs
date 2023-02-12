using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISPRO.Persistence.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminAccounts",
                columns: table => new
                {
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    UserType = table.Column<string>(type: "TEXT", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    Mobile = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminAccounts", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "ManagerAccounts",
                columns: table => new
                {
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    UserType = table.Column<string>(type: "TEXT", nullable: false),
                    MaxAllowedProjects = table.Column<int>(type: "INTEGER", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    Mobile = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerAccounts", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    ProjectManagerUsername = table.Column<string>(type: "TEXT", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Projects_ManagerAccounts_ProjectManagerUsername",
                        column: x => x.ProjectManagerUsername,
                        principalTable: "ManagerAccounts",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Bandwidth = table.Column<int>(type: "INTEGER", nullable: false),
                    Quota = table.Column<int>(type: "INTEGER", nullable: false),
                    ProjectName = table.Column<string>(type: "TEXT", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Projects_ProjectName",
                        column: x => x.ProjectName,
                        principalTable: "Projects",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ProjectName = table.Column<string>(type: "TEXT", nullable: false),
                    SubscriptionId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserType = table.Column<string>(type: "TEXT", nullable: false),
                    ValidityDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    Mobile = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Username);
                    table.ForeignKey(
                        name: "FK_UserAccounts_Projects_ProjectName",
                        column: x => x.ProjectName,
                        principalTable: "Projects",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccounts_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CashPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserAccountName = table.Column<string>(type: "TEXT", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RechargePeriod = table.Column<int>(type: "INTEGER", nullable: false),
                    Ammount = table.Column<double>(type: "REAL", nullable: false),
                    Currency = table.Column<int>(type: "INTEGER", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashPayments_UserAccounts_UserAccountName",
                        column: x => x.UserAccountName,
                        principalTable: "UserAccounts",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrePaidCards",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    SubscriptionId = table.Column<int>(type: "INTEGER", nullable: false),
                    ConsumerName = table.Column<string>(type: "TEXT", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ConsumptionDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RechargePeriod = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrePaidCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrePaidCards_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrePaidCards_UserAccounts_ConsumerName",
                        column: x => x.ConsumerName,
                        principalTable: "UserAccounts",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CashPayments_UserAccountName",
                table: "CashPayments",
                column: "UserAccountName");

            migrationBuilder.CreateIndex(
                name: "IX_PrePaidCards_ConsumerName",
                table: "PrePaidCards",
                column: "ConsumerName");

            migrationBuilder.CreateIndex(
                name: "IX_PrePaidCards_SubscriptionId",
                table: "PrePaidCards",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectManagerUsername",
                table: "Projects",
                column: "ProjectManagerUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_ProjectName",
                table: "Subscriptions",
                column: "ProjectName");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_ProjectName",
                table: "UserAccounts",
                column: "ProjectName");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_SubscriptionId",
                table: "UserAccounts",
                column: "SubscriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminAccounts");

            migrationBuilder.DropTable(
                name: "CashPayments");

            migrationBuilder.DropTable(
                name: "PrePaidCards");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "ManagerAccounts");
        }
    }
}
