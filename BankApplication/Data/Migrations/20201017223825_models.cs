using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankApplication.Data.Migrations
{
    public partial class models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankAccountTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Commission = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CreditTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Rates = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    Commission = table.Column<decimal>(type: "decimal(18, 6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    Bid = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    Ask = table.Column<decimal>(type: "decimal(18, 6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    PESEL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Balance = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    AvailableFounds = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    Lock = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    BankAccountNumber = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    BankAccountTypeID = table.Column<int>(nullable: false),
                    CurrencyID = table.Column<int>(nullable: false),
                    ProfileID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BankAccounts_BankAccountTypes_BankAccountTypeID",
                        column: x => x.BankAccountTypeID,
                        principalTable: "BankAccountTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Profiles_ProfileID",
                        column: x => x.ProfileID,
                        principalTable: "Profiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreditApplications",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditAmount = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    TotalRepayment = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    MonthRepayment = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    NumberOfMonths = table.Column<int>(nullable: false),
                    DateOfSubmission = table.Column<DateTime>(nullable: false),
                    State = table.Column<bool>(nullable: true),
                    ScannedDocument = table.Column<byte[]>(nullable: true),
                    TypeID = table.Column<int>(nullable: false),
                    ProfileID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditApplications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CreditApplications_Profiles_ProfileID",
                        column: x => x.ProfileID,
                        principalTable: "Profiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreditApplications_CreditTypes_TypeID",
                        column: x => x.TypeID,
                        principalTable: "CreditTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Credits",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditAmount = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    TotalRepayment = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    CurrentRepayment = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    MonthRepayment = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    NumberOfMonths = table.Column<int>(nullable: false),
                    NumberOfMonthsToEnd = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    LastPayment = table.Column<DateTime>(nullable: true),
                    IsPaidOff = table.Column<bool>(nullable: false),
                    TypeIDID = table.Column<int>(nullable: true),
                    TypeID1 = table.Column<int>(nullable: true),
                    ProfileID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credits", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Credits_Profiles_ProfileID",
                        column: x => x.ProfileID,
                        principalTable: "Profiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Credits_CreditTypes_TypeID1",
                        column: x => x.TypeID1,
                        principalTable: "CreditTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Credits_CreditTypes_TypeIDID",
                        column: x => x.TypeIDID,
                        principalTable: "CreditTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValueTo = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    ValueFrom = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    BalanceAfterTransactionUserFrom = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    BalanceAfterTransactionUserTo = table.Column<decimal>(type: "decimal(18, 6)", nullable: false),
                    FromBankAccountNumber = table.Column<string>(nullable: true),
                    ToBankAccountNumber = table.Column<string>(nullable: false),
                    SenderName = table.Column<string>(nullable: true),
                    ReceiverName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    TransactionTypeID = table.Column<int>(nullable: false),
                    CurrencyToID = table.Column<int>(nullable: true),
                    CurrencyFromID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Transactions_Currencies_CurrencyFromID",
                        column: x => x.CurrencyFromID,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Currencies_CurrencyToID",
                        column: x => x.CurrencyToID,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_TransactionTypes_TransactionTypeID",
                        column: x => x.TransactionTypeID,
                        principalTable: "TransactionTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_BankAccountTypeID",
                table: "BankAccounts",
                column: "BankAccountTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_CurrencyID",
                table: "BankAccounts",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_ProfileID",
                table: "BankAccounts",
                column: "ProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_CreditApplications_ProfileID",
                table: "CreditApplications",
                column: "ProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_CreditApplications_TypeID",
                table: "CreditApplications",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Credits_ProfileID",
                table: "Credits",
                column: "ProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_Credits_TypeID1",
                table: "Credits",
                column: "TypeID1");

            migrationBuilder.CreateIndex(
                name: "IX_Credits_TypeIDID",
                table: "Credits",
                column: "TypeIDID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CurrencyFromID",
                table: "Transactions",
                column: "CurrencyFromID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CurrencyToID",
                table: "Transactions",
                column: "CurrencyToID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionTypeID",
                table: "Transactions",
                column: "TransactionTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "CreditApplications");

            migrationBuilder.DropTable(
                name: "Credits");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "BankAccountTypes");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "CreditTypes");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "TransactionTypes");
        }
    }
}
