# Proyecto Ingeniería Web CORE - Alain Ruales
En el siguiente repositorio se encuentra la implementación del proyecto CORE para la materia de ingeriería web periodo 202310
## Descripción de proyecto
Año con año, los planes de enseñanza de todos los profesores van cambiando y adaptándose
en base al contenido impartido dentro de periodos anteriores y los resultados obtenidos. Estos
cambios se hacen con el objetivo de aumentar el engagement en los estudiantes con respecto
a una materia. Sin embargo, esta actividad puede resultar tediosa y a veces complicada.

El Core que se desea crear, busca simplificar esta tarea. Para lograr esto, se realizará una
aplicación web con Razor y C# la cual tiene la funcionalidad de calcular el nivel de engagement
de diferentes tareas en base a tres parámetros. Con estos parámetros, se mostrará una
calificación que permitirá identificar si una tarea está siendo relevante y aporta en mantener y
aumentar el nivel de engagement en los estudiantes o si está causando lo contrario. En base a
esta información, se puede tomar decisiones importantes con respecto a la evolución de un
curso

## Estructura de proyecto
El proyecto actualmente es funcional para realizar agregar, editar y visualizar usuarios, cursos y task y contiene un Login para acceder a estas funcionalidades. Si no se realiza Login, no se puede acceder y se envía una pantalla de error.
Recientemente, se implemntó roles, en donde se tiene a docentes, estudiantes y un admin. 
Actualmente se cuenta con una funcionalidad básica del Admin, el cual puede crear usuarios, task, cursos y agregar estudiantes a cursos. Esto con el objetivo de facilitar la creación de datos de prueba. Posteriormente, con el perfil de docente, se podrá agregar task a los cursos y con ello, agregar calificaciones.
## Estructura de proyecto

El proyecto de ingeniería Web fue creado con ASP.NET Core con .NET 6 en donde se tiene los siguientes componentes
- Razor -> Front-End
- C# -> Back-End

Este proyecto se encuentra dentro de una solución que puede ser abierta por Visual Studio. 
Dentro de esta solución, se tiene el proyecto MVC Web y un proyecto con los modelos y contexto necesarios para la conexión con la base de datos.

Para el almacenamiento se utiliza Microsoft SQL Server.

## Ejecución de proyecto
Para ejecutar el proyecto, de preferencia se debe utilizar Visual Studio 2022 con la instalación de "Desarrollo de APS.NET web"
<img src="https://user-images.githubusercontent.com/87552871/194952850-190061f6-fc18-4ae9-8d14-b0b06d1cd211.png" width=50% height=50%>

Dentro de Visual Studio, es necesario seleccionar al proyecto MVC para ser proyecto de inicio. 
<img src="https://user-images.githubusercontent.com/87552871/194955051-d5ee88b0-4854-43d3-ab8d-589af219e1c9.png" width=50% height=50%>

Posteriormtente, se debe ejecutar el proyecto desde la interfaz gráfica.

<img src="https://user-images.githubusercontent.com/87552871/194955067-86715fb8-955a-4cef-a239-bde48400ce74.png" width=50% height=50%>

## Restaurar base de datos
Para restaurar la base de datos en Microft SQL Server es necesario simplemente ejecutar el programa dentro de Visual Studio 2022
