using System.Data;
using Biblioteca.We.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.We.Repositorios
{
    public class LibroRepositorio
    {
        private readonly string _cadenaConexion;

        public LibroRepositorio(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("BibliotecaDB")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'BibliotecaDB'.");
        }

        private SqlConnection CrearConexion() => new SqlConnection(_cadenaConexion);

        public async Task<IEnumerable<Libro>> ListarAsync()
        {
            using var conexion = CrearConexion();
            return await conexion.QueryAsync<Libro>(
                "sp_Libros_Listar",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Libro>> BuscarPorTituloAsync(string titulo)
        {
            using var conexion = CrearConexion();
            return await conexion.QueryAsync<Libro>(
                "sp_Libros_BuscarPorTitulo",
                new { Titulo = titulo },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Libro?> ObtenerPorIdAsync(int libroId)
        {
            using var conexion = CrearConexion();
            var libros = await conexion.QueryAsync<Libro>(
                "sp_Libros_ObtenerPorId",
                new { LibroId = libroId },
                commandType: CommandType.StoredProcedure);
            return libros.FirstOrDefault();
        }

        public async Task InsertarAsync(Libro libro)
        {
            using var conexion = CrearConexion();
            await conexion.ExecuteAsync(
                "sp_Libros_Insertar",
                new { libro.Titulo, libro.ISBN, libro.AutorId, libro.Ejemplares },
                commandType: CommandType.StoredProcedure);
        }

        public async Task ActualizarAsync(Libro libro)
        {
            using var conexion = CrearConexion();
            await conexion.ExecuteAsync(
                "sp_Libros_Actualizar",
                new { libro.LibroId, libro.Titulo, libro.ISBN, libro.AutorId, libro.Ejemplares },
                commandType: CommandType.StoredProcedure);
        }

        public async Task EliminarAsync(int libroId)
        {
            using var conexion = CrearConexion();
            await conexion.ExecuteAsync(
                "sp_Libros_Eliminar",
                new { LibroId = libroId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Autor>> ListarAutoresAsync()
        {
            using var conexion = CrearConexion();
            return await conexion.QueryAsync<Autor>(
                "sp_Autores_ListarActivos",
                commandType: CommandType.StoredProcedure);
        }
    }
}