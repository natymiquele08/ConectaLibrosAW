USE master;
GO

-- 1. Si la base de datos existe, fuerza la desconexión de usuarios y la elimina
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'ObligatorioBD')
BEGIN
    ALTER DATABASE ObligatorioBD SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE ObligatorioBD;
END
GO


CREATE DATABASE ObligatorioBD;
GO


USE ObligatorioBD;
GO

-- Creacion de tablas
CREATE TABLE Autores
(
	CodigoInternoAutor varchar(10) Primary Key,
	NombreA varchar(30) not null,
	FechaNacimiento datetime check(DATEADD(YEAR, 18, FechaNacimiento) <= GETDATE()) not null
)
go

CREATE TABLE Editoriales
(
	NombreEditorial varchar(20) primary key,
	DireccionE varchar(20) not null,
	CiudadE varchar(20) not null, 
	Antiguedad int not null

)
go

CREATE TABLE Libros
( 
	ISBNLibro varchar(13) primary key,
	NombreL varchar(30) not null,
	Resena varchar(100) not null,
	Genero varchar(30) not null,
	StockActual int not null,
	CodigoInternoAutor varchar(10) not null foreign key references Autores(CodigoInternoAutor),
	NombreEditorial varchar(20) not null foreign key references Editoriales (NombreEditorial)

)
go

CREATE TABLE Librerias
(
	RUTLibrerias varchar(12) primary key check (LEN(RUTLibrerias) = 12),
	NombreLibreria varchar(20) not null,
	DireccionL varchar(50) not null, 
	CiudadL varchar(20) not null,
	Telefono varchar(15) not null
)
go

CREATE TABLE Solicitudes
(
	ID int primary key identity(1,1), --autogenerado el ID
	FechaA date not null default GETDATE(), --fecha automatica
	FechaEntrega datetime not null,  
	CantUnidadesSol int check (CantUnidadesSol > 0 ) not null, --la cant de unidades no puede ser negativa (RNE que agregue yo)
	RUTLibrerias varchar(12) not null foreign key references Librerias(RUTLibrerias),
	ISBNLibro varchar(13) not null foreign key references Libros(ISBNLibro)

)
go


--Datos de prueba
-- 1. AUTORES (Mayores de edad, código de hasta 10 caracteres)
INSERT INTO Autores (CodigoInternoAutor, NombreA, FechaNacimiento) VALUES
('AR1002', 'Anne Rice', '19501004'),
('SK1947', 'Stephen King', '19470921'),
('JK1965', 'J.K. Rowling', '19650731'),
('MG1975', 'Mariana Enriquez', '19731208'),
('PR1990', 'Pedro Ramirez', '19900515'); 

-- 2. EDITORIALES
INSERT INTO Editoriales (NombreEditorial, DireccionE, CiudadE, Antiguedad) VALUES
('Editorial En Papel', 'Av. Principal 2569', 'Minas', 35),
('Debolsillo', '18 de Julio 1420', 'Montevideo', 20),
('Salamandra', 'Plaza Indep 810', 'Montevideo', 15),
('Planeta Uruguay', 'Av. Brasil 2901', 'Montevideo', 40);

-- 3. LIBRERÍAS (RUT de 12 dígitos numéricos)
INSERT INTO Librerias (RUTLibrerias, NombreLibreria, DireccionL, CiudadL, Telefono) VALUES
('223335555566', 'Letras Viajeras', 'Calle 27 esq Rodo', 'Pando', '099887744'),
('112233445566', 'Libreria Central', '18 de Julio 1002', 'Montevideo', '098123456'),
('998877665544', 'Libros del Sol', 'Calle Sarandi 450', 'Maldonado', '091987654'),
('123456789012', 'Libreria Nueva', 'Av. Rivera 3200', 'Montevideo', '099112233'); -- Para probar borrado sin solicitudes

-- 4. LIBROS
INSERT INTO Libros (ISBNLibro, NombreL, Resena, Genero, StockActual, CodigoInternoAutor, NombreEditorial) VALUES
('11225588', 'Libro Azul', 'Cuento infantil ilustrado', 'Infantil', 50, 'AR1002', 'Editorial En Papel'),
('978030774', 'El Resplandor', 'Novela de terror psicologico', 'Terror', 15, 'SK1947', 'Debolsillo'),
('978847888', 'Harry Potter 1', 'Un nino descubre que es mago', 'Fantasia', 30, 'JK1965', 'Salamandra'),
('978987738', 'Nuestra Parte Noche', 'Novela de terror y sectas', 'Terror', 10, 'MG1975', 'Planeta Uruguay');

-- 5. SOLICITUDES (Se omite FechaA para que use DEFAULT GETDATE(), FechaEntrega a futuro)
INSERT INTO Solicitudes (FechaEntrega, CantUnidadesSol, RUTLibrerias, ISBNLibro) VALUES
(DATEADD(DAY, 3, GETDATE()), 15, '223335555566', '11225588'),
(DATEADD(DAY, 2, GETDATE()), 5, '112233445566', '978030774'),
(DATEADD(DAY, 5, GETDATE()), 10, '998877665544', '978847888'),
(DATEADD(DAY, 4, GETDATE()), 8, '223335555566', '978987738');
GO

--Procedimientos almacenados
-- A) CRUD AUTOR

--Agregar autor: LISTO
create or alter proc AgregarAutor
@CodigoInternoAutor varchar(10), @NombreA varchar(30), @FechaNacimiento datetime
as
begin
	--validar que no exista ya el autor
	if exists( select * from Autores where CodigoInternoAutor=@CodigoInternoAutor)
		return -1

	insert into Autores(CodigoInternoAutor, NombreA, FechaNacimiento) values (@CodigoInternoAutor,@NombreA,@FechaNacimiento)
	if(@@ERROR !=0)
		return -2

	return 1 --se agrego con exito
end
go

declare @retorno int
exec @retorno = AgregarAutor JV1234, 'Jose', '19991204'

if @retorno = 1
    print 'Autor agregado con éxito.';
else if  @retorno = -1
    print 'El código de autor ya existe.';
else if @retorno = -2
    print 'Ocurrió un problema al insertar el autor';
go

--select * from Autores



--Modificar autor (update)
create or alter proc ModificarAutor
@CodigoInternoAutor varchar(10), @NombreA varchar(30), @FechaNacimiento datetime
as
begin
	if not exists(select * from Autores where CodigoInternoAutor=@CodigoInternoAutor)
		return -1 --no existe el autor a modificar

	if (DATEADD(YEAR, 18, @FechaNacimiento) > GETDATE())
		return -2

	update Autores set NombreA = @NombreA, FechaNacimiento=@FechaNacimiento
	where CodigoInternoAutor =@CodigoInternoAutor
	if(@@ERROR != 0)
		return -3

	return 1 -- modificacion con exito

end
go

declare @retorno int
exec @retorno = ModificarAutor JV1234, 'JUAN', '19991208'
if @retorno = 1
    print 'Autor modificado con éxito.';
else if  @retorno = -1
    print 'No existe el autor a modificar';
else if @retorno = -2
    print 'El autor debe ser mayor de edad.';
else if @retorno = -3
    print 'Ocurrió un problema al modificar el autor';
go



-- Eliminar autor (delete)
create or alter proc EliminarAutor
@CodigoInternoAutor varchar(10)
as
begin
	if not exists(select * from Autores where CodigoInternoAutor=@CodigoInternoAutor)
		return -1 --no existe el autor a elimiar

	--validar que no tenga libros asociados
	if exists(select * from Libros where CodigoInternoAutor=@CodigoInternoAutor)
		return -2 --tiene libros asociados

	delete from Autores where CodigoInternoAutor=@CodigoInternoAutor
	if(@@ERROR != 0)
		return -3 --no se pudo eliminar

	return 1
end
go

-- Listado de autores
create or alter proc ListadoAutores
as
begin 
	select *
	from Autores
end
go

exec ListadoAutores
go


declare @retorno int
exec @retorno = EliminarAutor 'AR1002' --no se puede eliminar este ej porque tiene libros asociados
if @retorno = 1
    print 'Autor eliminado con éxito.';
else if  @retorno = -1
    print 'No existe el autor a eliminar';
else if @retorno = -2
    print 'No se puede eliminar porque tiene libros asociados';
else if @retorno = -3
	print 'No se pudo eliminar'
go

--Buscar autor (select)
create or alter proc BuscarAutor
@codigoInternoAutor varchar(10)
as
begin
	select * from Autores where codigoInternoAutor = @codigoInternoAutor
end
go

exec BuscarAutor 'AR1002'
go


-- LISTADO EDITORIALES
CREATE OR ALTER PROC ListadoEditoriales
AS
BEGIN 
    SELECT *
    FROM Editoriales
    ORDER BY NombreEditorial;
END
GO

exec ListadoEditoriales
go

--Libros de las editoriales
create or alter proc LibrosEditoriales
@NombreEditorial  varchar(20)
as
begin
	select *
	from Libros
	where @NombreEditorial = NombreEditorial

end
go

exec LibrosEditoriales'Editorial En Papel'
go



-- Listar solicitudes por Editoriales
create or alter proc ListadoSolicitudesXEditoriales
@NombreEditorial varchar(20)
as
begin
	select Solicitudes.*
	from Solicitudes inner join Libros on Solicitudes.ISBNLibro = Libros.ISBNLibro
	where @NombreEditorial = Libros.NombreEditorial
end
go

exec ListadoSolicitudesXEditoriales 'Editorial En Papel' 
go

--B) CRUD LIBRERIAS
--Agregar Libreria
create or alter proc AgregarLibreria
@RUTLibrerias varchar(12), @NombreLibreria varchar(20), @DireccionL varchar(50), @CiudadL varchar(20), @Telefono varchar(15) 
as
begin
	--validar que no exista ya la libreria
	if exists( select * from Librerias where RUTLibrerias = @RUTLibrerias )
		return -1 --ya existe libreria


	insert into Librerias (RUTLibrerias, NombreLibreria, DireccionL, CiudadL, Telefono) values (@RUTLibrerias , @NombreLibreria , @DireccionL , @CiudadL , @Telefono)
	if(@@ERROR !=0)
		return -2

	return 1 --se agrego con exito
end
go

declare @retorno int
exec @retorno = AgregarLibreria 123456789123, 'Naty', 'moller', 'la paz',  '099887744' --ejemplo
--captura de errores
if @retorno = 1
    print 'Libreria agregada con éxito.';
else if  @retorno = -1
    print 'Ya existe la libreria';
else if @retorno = -2
    print 'Ocurrio un error al agregar la libreria';
go


--select * from Librerias


--Modificar Libreria
create or alter proc ModificarLibreria
@RUTLibrerias varchar(12), @NombreLibreria varchar(20), @DireccionL varchar(50), @CiudadL varchar(20), @Telefono varchar(15) 
as
begin
	if not exists(select * from Librerias where RUTLibrerias = @RUTLibrerias)
		return -1 --no existe la libreria a modificar

	update Librerias set NombreLibreria = @NombreLibreria, DireccionL = @DireccionL, CiudadL = @CiudadL, Telefono = @Telefono
	where RUTLibrerias = @RUTLibrerias
	if(@@ERROR != 0)
		return -2

	return 1 -- modificacion con exito

end
go

declare @retorno int
exec @retorno = ModificarLibreria 123456789123, 'Miquele', 'Conrado', 'Las Piedras', '093497048'
if @retorno = 1
    print 'Libreria modificada con éxito.';
else if  @retorno = -1
    print 'No existe la libreria a modificar';
else if @retorno = -2
    print 'Ocurrio un problema al modificar la libreria';

go

--Eliminar Libreria
create or alter proc EliminarLibreria
@RUTLibrerias varchar(12)
as
begin
	if not exists(select * from Librerias where RUTLibrerias = @RUTLibrerias)
		return -1 --no existe la libreria a eliminar

	--validar que no tenga solicitudes asociados
	if exists(select * from Solicitudes where RUTLibrerias = @RUTLibrerias)
		return -2 --tiene solicitudes asociadas

	delete from Librerias where RUTLibrerias = @RUTLibrerias
	if(@@ERROR != 0)
		return -3 --no se pudo eliminar

	return 1
end
go


declare @retorno int
exec @retorno = EliminarLibreria 223335555566 --ejemplo de libreria con solicitud asociada, no se puede eliminar
if @retorno = 1
    print 'Libreria eliminada con éxito.';
else if  @retorno = -1
    print 'No existe la libreria a eliminar';
else if @retorno = -2
    print 'No se puede eliminar porque tiene solicitudes asociadas';
else if @retorno = -3
	print 'No se pudo eliminar'
go

-- Listado de Librerias
create or alter proc ListadoLibrerias
as
begin 
	select *
	from Librerias
end
go

exec ListadoLibrerias
go

--Listar Solicitudes que realizo la libreria
create or alter proc ListadoSolicitudesXLibreria
@rutLibrerias varchar(12)
as
begin
	select *
	from Solicitudes
	where @rutLibrerias = RUTLibrerias
end
go

--Buscar libreria 
create or alter proc BuscarLibreria
@RUTLibreria varchar(12)
as
begin
	select * from Librerias where RUTLibrerias = @RUTLibreria
end
go

exec BuscarLibreria '123456789123' --ejemplo propio
go

--C) Agregar LIBRO
create or alter proc AgregarLibro
@ISBNLibro varchar(13) , @NombreL varchar(30) , @Resena varchar(100) , @Genero varchar(30) , @StockActual int , @CodigoInternoAutor varchar(10), @NombreEditorial varchar(20) 
as
begin

	-- Validar que no exista el libro
    If exists (select * from Libros where ISBNLibro = @ISBNLibro)
        return -1; -- Ya existe el ISBN

	--Si no existe autor y editorial en la BD error
	if not exists(select * from Autores where CodigoInternoAutor =@CodigoInternoAutor)
		return -2 --no hay autor en la BD

	if not exists(select * from Editoriales where NombreEditorial =@NombreEditorial)
		return -3 --no hay editorial en la BD

	--el stock no puede ser negativo
	if (@StockActual < 0)
       return -4; -- stock no puede ser negativo

	insert into Libros (ISBNLibro, NombreL, Resena, Genero, StockActual, CodigoInternoAutor, NombreEditorial) values (@ISBNLibro, @NombreL , @Resena , @Genero, @StockActual , @CodigoInternoAutor, @NombreEditorial)
	if(@@ERROR !=0)
		return -5

	return 1 --se agrego con exito
end
go

--select * from Libros
declare @retorno int
exec @retorno = AgregarLibro 12345678,'Harry','pepe','love',10,'AR1002','Debolsillo' --prueba
--captura de errores
if @retorno = 1
    print 'Libro agregado con éxito.';
else if  @retorno = -1
    print 'El libro ya existe.';
else if @retorno = -2
    print 'No hay autor ingresado en la base de datos';
else if @retorno = -3
    print 'No hay editorial ingresada en la base de datos';
else if @retorno = -4
    print 'El stock no puede ser negativo';
else if @retorno = -5
    print 'Ocurrio un error al ingresar el libro';
go


--D) Agregar solicitud
create or alter proc AgregarSolicitud
@FechaEntrega datetime, @CantUnidadesSol int, @RUTLibrerias varchar(12) , @ISBNLibro varchar(13)
as
begin
	--validar que exista el libro y libreria a solicitar con ISBNLibro y RUT
	if not exists(select * from Librerias where RUTLibrerias = @RUTLibrerias)
		return -1 -- no existe la libreria 

	if not exists( select * from Libros where ISBNLibro = @ISBNLibro)
		return -2 -- no existe el libro

	--validar que la fecha sea al menos 24 hs posterior que la actual
	if(@FechaEntrega < DATEADD(HOUR, 24, GETDATE()))
		return -3 --no pasaron 24 hs de la solcitud

	--validar que haya stock de libros
	if((select StockActual from Libros where ISBNLibro = @ISBNLibro) < @CantUnidadesSol)
		return -4 --no hay stock del libro solicitado

	--variable para el ID
	declare @IDSolicitud int

	--realizar transaccion porque tengo que hacer un insert y un update si es exitoso
	begin tran

	--ingresar la soliticud
	insert into Solicitudes (FechaEntrega, CantUnidadesSol, RUTLibrerias, ISBNLibro) values (@FechaEntrega, @CantUnidadesSol, @RUTLibrerias, @ISBNLibro)
	if(@@ERROR !=0)
	begin 
		rollback tran
		return -5 --corta la transaccion, error al crear la solicitud
	end
	
	--agregarle el ID autogenerado
	set @IDSolicitud = SCOPE_IDENTITY() --autogenerado de esta tabla
	
	--actualizar valor del stock actual del libro ingresado y segun la cantidad solicitada
	update Libros set StockActual -= @CantUnidadesSol where ISBNLibro = @ISBNLibro
	if(@@ERROR != 0 )
	begin
		rollback tran
		return -6 --fallo el update, corta la transaccion
	end

	commit tran --si todo sale bien se realiza la tran
	return @IDSolicitud --ID autogenerado si todo sale ok
end
go

declare @retorno int
exec @retorno = AgregarSolicitud '20270525', 2, '223335555566', '978987738' --datos prueba

if(@retorno = -1)
	print 'No existe la libreria a solicitar'
else if(@retorno = -2)
	print 'No existe el libro a solicitar'
else if(@retorno = -3)
	print 'La solicitud tiene que ser mayor a 24hs'
else if(@retorno = -4)
	print 'No hay stock disponible de libro a solicitar'
else if(@retorno = -5)
	print 'Error al crear la solicitud'
else if(@retorno = -6)
	print 'Fallo la actualizacion del stock actual del libro'
else if(@retorno > 0) -- si el retorno es positivo es pq se hizo el autogenerado
	print 'Solicitud agregada con exito. Numero de solicitud: ' + CAST(@retorno as varchar)
go

--E) ELIMINAR UN LIBRO (delete)
create or alter proc EliminarLibro
@ISBNLibro varchar(13)
as
begin
	--ver si existe libro
	if not exists(select * from Libros where ISBNLibro = @ISBNLibro)
		return -1 --no existe el libro en la BD

	begin tran
	--eliminar FK
	delete from Solicitudes where ISBNLibro = @ISBNLibro
	if(@@ERROR !=0)
	begin
		rollback tran
		return -2 -- no se elimino la solicitud del libro 
	end
	--eliminar PK
	delete from Libros where ISBNLibro = @ISBNLibro
	if(@@ERROR !=0)
	begin
		rollback tran 
		return -2 --no se elimino el libro 
	end
	commit tran
	return 1 --se elimino el libro

end
go

declare @retorno int
exec @retorno = EliminarLibro 11225588
if @retorno = 1
    print 'Libro eliminado con exito';
else if  @retorno = -1
    print 'No existe el libro en la BD';
else if @retorno = -2
    print 'Error en la transaccion';

go

--F)Listado de SOLICITUDES (Solictudes completo, Libreria(nombre), Libros(nombre))
create or alter proc ListadoSolicitudes
as
begin
	select ID, FechaA as [Fecha Actual], FechaEntrega, CantUnidadesSol as [Cantidad Solicitadas], NombreLibreria, NombreL as [Nombre Libro], Solicitudes.RUTLibrerias, Solicitudes.ISBNLibro
	from Solicitudes inner join Librerias on Solicitudes.RUTLibrerias = Librerias.RUTLibrerias
	inner join Libros on Solicitudes.ISBNLibro = Libros.ISBNLibro
	order by FechaEntrega asc
end
go

EXEC ListadoSolicitudes;
GO

--G) Total solicitudes por AUTOR Y LIBRO
create or alter proc TotalSolicitudes
as
begin
	select  Autores.NombreA as [Nombre Autor], Libros.NombreL as [Nombre Libro], COUNT(Solicitudes.ISBNLibro) as [Total Solicitudes]
	from Autores inner join Libros on Autores.CodigoInternoAutor = Libros.CodigoInternoAutor left join Solicitudes on Solicitudes.ISBNLibro = Libros.ISBNLibro
	group by Autores.NombreA, Autores.CodigoInternoAutor, Libros.NombreL, Libros.ISBNLibro
	order by [Total Solicitudes] desc
end
go

exec TotalSolicitudes
go




