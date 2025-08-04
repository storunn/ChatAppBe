using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppBe.Migrations
{
    /// <inheritdoc />
    public partial class SentAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Microsoft.EntityFrameworkCore.Migrations.Operations.Builders.OperationBuilder<Microsoft.EntityFrameworkCore.Migrations.Operations.AddColumnOperation> operationBuilder = migrationBuilder.AddColumn<DateTime>(
        name: "SentAt",
        table: "Messages",
        type: "datetime2",
        nullable: false,
        defaultValueSql: "GETDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
