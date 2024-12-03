using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Symbol = table.Column<string>(type: "TEXT", nullable: false),
                    ConversionRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    CountryCode = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    ConversionsLimit = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    SubscriptionId = table.Column<int>(type: "INTEGER", nullable: true),
                    SubscribedUntil = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ConversionsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    usedTrial = table.Column<bool>(type: "INTEGER", nullable: false),
                    isDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Conversions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FromCurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    ToCurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Result = table.Column<decimal>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversions_Currencies_FromCurrencyId",
                        column: x => x.FromCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conversions_Currencies_ToCurrencyId",
                        column: x => x.ToCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conversions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "ConversionRate", "CountryCode", "IsDeleted", "Name", "Symbol" },
                values: new object[,]
                {
                    { 1, 1m, "US", false, "Dólar estadounidense", "USD" },
                    { 2, 0.001m, "AR", false, "Peso argentino", "ARS" },
                    { 3, 1.09m, "EU", false, "Euro", "EUR" }
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "Id", "ConversionsLimit", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 10, "Free", 0m },
                    { 2, 100, "Trial", 10m },
                    { 3, null, "Pro", 100m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ConversionsCount", "Name", "Password", "SubscribedUntil", "SubscriptionId", "Username", "isDeleted", "usedTrial" },
                values: new object[] { 1, 0, "Administrador", "admin", new DateTime(2025, 12, 3, 18, 55, 52, 174, DateTimeKind.Utc).AddTicks(6162), 3, "admin", false, false });

            migrationBuilder.CreateIndex(
                name: "IX_Conversions_FromCurrencyId",
                table: "Conversions",
                column: "FromCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversions_ToCurrencyId",
                table: "Conversions",
                column: "ToCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversions_UserId",
                table: "Conversions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SubscriptionId",
                table: "Users",
                column: "SubscriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conversions");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Subscriptions");
        }
    }
}
