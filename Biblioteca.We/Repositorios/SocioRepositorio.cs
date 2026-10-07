using System.Data;
using Biblioteca.We.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.We.Repositorios
{
    public class SocioRepositorio
    {
        private readonly string _cadenaConexion;

        public SocioRepositorio(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("BibliotecaDB")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'BibliotecaDB'.");
        }

        private SqlConnection CrearConexion() => new SqlConnection(_cadenaConexion);

        public async Task<IEnumerable<Socio>> ListarAsync()
        {
            using var conexion = CrearConexion();
            return await conexion.QueryAsync<Socio>(
                "sp_Socios_ListarActivos",
                commandType: CommandType.StoredProcedure);
        }

        // Devuelve true si se insertó, false si el DNI ya existe
        public async Task<bool> InsertarAsync(Socio socio)
        {
            using var conexion = CrearConexion();
            var resultado = await conexion.QuerySingleAsync<int>(
                "sp_Socios_Insertar",
                new { socio.DNI, socio.Nombre, socio.Email },
                commandType: CommandType.StoredProcedure);
            return resultado == 1;
        }
    }
}