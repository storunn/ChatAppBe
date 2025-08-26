using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppBe.Migrations
{
    /// <inheritdoc />
    public partial class GroupMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // HATALI: "s" tablosu yok -> Drop/Rename işlemlerini KALDIRDIK.
            // Sadece yanlış adlandırılmış FK'yi kaldırıp doğru isimle ekleyeceğiz.

            // 1) Eski (yanlış isimli) FK'yi kaldır
            migrationBuilder.DropForeignKey(
                name: "FK_s_Groups_GroupId",
                table: "GroupMembers");

            // (Users FK'si zaten doğru isimde; bunu drop/add yapmaya gerek yok.)
            // Eğer önceki migration bunu da düşürüp ekliyorsa sorun değil ama minimal değişiklik daha güvenli.

            // 2) İstenen kolon değişiklikleri
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Msg",
                table: "GroupMessages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // 3) Doğru isimle FK'yi tekrar ekle
            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Up'ta eklediğimiz doğru isimli FK'yi kaldır
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers");

            // Kolonları eski haline döndür
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Msg",
                table: "GroupMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            // Eski (yanlış isimli) FK'yi geri ekle (Down için)
            migrationBuilder.AddForeignKey(
                name: "FK_s_Groups_GroupId",
                table: "GroupMembers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}