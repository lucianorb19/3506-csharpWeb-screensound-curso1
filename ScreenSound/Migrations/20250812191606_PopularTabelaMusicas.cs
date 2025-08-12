using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScreenSound.Migrations
{
    /// <inheritdoc />
    public partial class PopularTabelaMusicas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("Musicas", 
                new string[] {"Nome","AnoLancamento"},
                new object[] {"Paper Cut", 2000});

            migrationBuilder.InsertData("Musicas",
                new string[] { "Nome", "AnoLancamento" },
                new object[] { "From the Inside", 2003 });

            migrationBuilder.InsertData("Musicas",
                new string[] { "Nome", "AnoLancamento" },
                new object[] { "Love of My Life", 1975 });

            migrationBuilder.InsertData("Musicas",
                new string[] { "Nome", "AnoLancamento" },
                new object[] { "Smoke on the Water", 1972 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Musicas");
        }


    }
}
