using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Migrations
{
    /// <inheritdoc />
    public partial class GenericIDChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Loan_Id",
                table: "Loans",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Borrower_Id",
                table: "Borrowers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Book_Id",
                table: "Books",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Author_Id",
                table: "Authors",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Loans",
                newName: "Loan_Id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Borrowers",
                newName: "Borrower_Id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Books",
                newName: "Book_Id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Authors",
                newName: "Author_Id");
        }
    }
}
