USE BibliotecaDB;
GO

/* =========================================================
   LIBROS
   ========================================================= */

-- Listado de libros activos con el nombre del autor
CREATE OR ALTER PROCEDURE sp_Libros_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT  l.LibroId, l.Titulo, l.ISBN, l.AutorId,
            a.Nombre AS AutorNombre, l.Ejemplares, l.Activo
    FROM Libros l
    INNER JOIN Autores a ON a.AutorId = l.AutorId
    WHERE l.Activo = 1
    ORDER BY l.Titulo;
END
GO

-- Búsqueda de libros activos por título
CREATE OR ALTER PROCEDURE sp_Libros_BuscarPorTitulo
    @Titulo VARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT  l.LibroId, l.Titulo, l.ISBN, l.AutorId,
            a.Nombre AS AutorNombre, l.Ejemplares, l.Activo
    FROM Libros l
    INNER JOIN Autores a ON a.AutorId = l.AutorId
    WHERE l.Activo = 1
      AND l.Titulo LIKE '%' + @Titulo + '%'
    ORDER BY l.Titulo;
END
GO

-- Obtener un libro por Id
CREATE OR ALTER PROCEDURE sp_Libros_ObtenerPorId
    @LibroId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT  l.LibroId, l.Titulo, l.ISBN, l.AutorId,
            a.Nombre AS AutorNombre, l.Ejemplares, l.Activo
    FROM Libros l
    INNER JOIN Autores a ON a.AutorId = l.AutorId
    WHERE l.LibroId = @LibroId
      AND l.Activo = 1;
END
GO

-- Insertar libro
CREATE OR ALTER PROCEDURE sp_Libros_Insertar
    @Titulo     VARCHAR(200),
    @ISBN       VARCHAR(20),
    @AutorId    INT,
    @Ejemplares INT
AS
BEGIN
    INSERT INTO Libros (Titulo, ISBN, AutorId, Ejemplares, Activo)
    VALUES (@Titulo, @ISBN, @AutorId, @Ejemplares, 1);
END
GO

-- Actualizar libro
CREATE OR ALTER PROCEDURE sp_Libros_Actualizar
    @LibroId    INT,
    @Titulo     VARCHAR(200),
    @ISBN       VARCHAR(20),
    @AutorId    INT,
    @Ejemplares INT
AS
BEGIN
    UPDATE Libros
    SET Titulo     = @Titulo,
        ISBN       = @ISBN,
        AutorId    = @AutorId,
        Ejemplares = @Ejemplares
    WHERE LibroId = @LibroId;
END
GO

-- Eliminación LÓGICA (nunca DELETE físico)
CREATE OR ALTER PROCEDURE sp_Libros_Eliminar
    @LibroId INT
AS
BEGIN
    UPDATE Libros
    SET Activo = 0
    WHERE LibroId = @LibroId;
END
GO

/* =========================================================
   AUTORES (para la lista desplegable)
   ========================================================= */
CREATE OR ALTER PROCEDURE sp_Autores_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT AutorId, Nombre
    FROM Autores
    WHERE Activo = 1
    ORDER BY Nombre;
END
GO

/* =========================================================
   SOCIOS
   ========================================================= */
CREATE OR ALTER PROCEDURE sp_Socios_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT SocioId, DNI, Nombre, Email, Activo
    FROM Socios
    WHERE Activo = 1
    ORDER BY Nombre;
END
GO

-- Devuelve 1 si se insertó, 0 si el DNI ya existe
CREATE OR ALTER PROCEDURE sp_Socios_Insertar
    @DNI    VARCHAR(8),
    @Nombre VARCHAR(100),
    @Email  VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Socios WHERE DNI = @DNI)
    BEGIN
        SELECT 0 AS Resultado;
        RETURN;
    END

    INSERT INTO Socios (DNI, Nombre, Email, Activo)
    VALUES (@DNI, @Nombre, @Email, 1);

    SELECT 1 AS Resultado;
END
GO

/* =========================================================
   REPORTE DE PRÉSTAMOS POR INTERVALO DE FECHAS
   ========================================================= */
CREATE OR ALTER PROCEDURE sp_Prestamos_ReportePorFechas
    @Desde DATE,
    @Hasta DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT  p.PrestamoId,
            s.Nombre AS Socio,
            s.DNI,
            STRING_AGG(l.Titulo, ', ') AS Libros,
            p.FechaPrestamo,
            p.FechaLimite,
            p.Estado
    FROM Prestamos p
    INNER JOIN Socios s          ON s.SocioId    = p.SocioId
    INNER JOIN DetallePrestamo d ON d.PrestamoId = p.PrestamoId
    INNER JOIN Libros l          ON l.LibroId    = d.LibroId
    WHERE p.FechaPrestamo BETWEEN @Desde AND @Hasta
    GROUP BY p.PrestamoId, s.Nombre, s.DNI, p.FechaPrestamo, p.FechaLimite, p.Estado
    ORDER BY p.FechaPrestamo;
END
GO