using System.Data;
using Biblioteca.We.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.We.Repositorios
{
    public class PrestamoRepositorio
    {
        private readonly string _cadenaConexion;

        public PrestamoRepositorio(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("BibliotecaDB")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'BibliotecaDB'.");
        }

        private SqlConnection CrearConexion() => new SqlConnection(_cadenaConexion);

        public async Task<IEnumerable<PrestamoReporte>> ReportePorFechasAsync(DateTime desde, DateTime hasta)
        {
            using var conexion = CrearConexion();
            return await conexion.QueryAsync<PrestamoReporte>(
                "sp_Prestamos_ReportePorFechas",
                new { Desde = desde.Date, Hasta = hasta.Date },
                commandType: CommandType.StoredProcedure);
        }
    }
}