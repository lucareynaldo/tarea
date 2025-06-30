using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public class GestorBaseDatos
{
    // Modifique para su servidor de SQL. Yo use SQL Server Express
    private string connectionString = "Server=localhost\\SQLEXPRESS;Database=SistemaEscolar;User Id=sa;Password=1234;";

    public DataTable ObtenerEstudiantes()
    {
        DataTable tabla = new DataTable();
        try
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                string consulta = "SELECT ID_Estudiante, Nombre, Apellido, DNI, FechaNacimiento FROM Estudiantes ORDER BY Apellido, Nombre";
                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion);
                adaptador.Fill(tabla);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al obtener estudiantes: " + ex.Message);
        }
        return tabla;
    }

    public bool AgregarEstudiante(string nombre, string apellido, string dni, DateTime fechaNacimiento)
    {
        try
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string consulta = "INSERT INTO Estudiantes (Nombre, Apellido, DNI, FechaNacimiento) VALUES (@nombre, @apellido, @dni, @fecha)";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", nombre);
                    comando.Parameters.AddWithValue("@apellido", apellido);
                    comando.Parameters.AddWithValue("@dni", dni);
                    comando.Parameters.AddWithValue("@fecha", fechaNacimiento);

                    comando.ExecuteNonQuery();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al agregar estudiante: " + ex.Message);
            return false;
        }
    }

    public bool ModificarEstudiante(int id, string nombre, string apellido, string dni, DateTime fechaNacimiento)
    {
        try
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string consulta = "UPDATE Estudiantes SET Nombre=@nombre, Apellido=@apellido, DNI=@dni, FechaNacimiento=@fecha WHERE ID_Estudiante=@id";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    comando.Parameters.AddWithValue("@nombre", nombre);
                    comando.Parameters.AddWithValue("@apellido", apellido);
                    comando.Parameters.AddWithValue("@dni", dni);
                    comando.Parameters.AddWithValue("@fecha", fechaNacimiento);

                    comando.ExecuteNonQuery();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al modificar estudiante: " + ex.Message);
            return false;
        }
    }

    public bool EliminarEstudiante(int id)
    {
        try
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string consulta = "DELETE FROM Estudiantes WHERE ID_Estudiante=@id";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    comando.ExecuteNonQuery();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al eliminar estudiante: " + ex.Message);
            return false;
        }
    }
    public DataTable ObtenerMaterias()
    {
        DataTable tabla = new DataTable();
        try
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                string consulta = "SELECT ID_Materia, NombreMateria FROM Materias ORDER BY NombreMateria";
                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion);
                adaptador.Fill(tabla);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al obtener materias: " + ex.Message);
        }
        return tabla;
    }

    public bool AgregarMateria(string nombreMateria)
    {
        try
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string consulta = "INSERT INTO Materias (NombreMateria) VALUES (@nombre)";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", nombreMateria);
                    comando.ExecuteNonQuery();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al agregar materia: " + ex.Message);
            return false;
        }
    }

    public bool ModificarMateria(int id, string nombreMateria)
    {
        try
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string consulta = "UPDATE Materias SET NombreMateria=@nombre WHERE ID_Materia=@id";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    comando.Parameters.AddWithValue("@nombre", nombreMateria);
                    comando.ExecuteNonQuery();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al modificar materia: " + ex.Message);
            return false;
        }
    }

    public bool EliminarMateria(int id)
    {
        try
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string consulta = "DELETE FROM Materias WHERE ID_Materia=@id";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    comando.ExecuteNonQuery();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al eliminar materia: " + ex.Message);
            return false;
        }
    }

    public DataTable ObtenerNotas()
    {
        DataTable tabla = new DataTable();
        try
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                string consulta = @"SELECT 
                    n.ID_Nota,
                    e.Nombre + ' ' + e.Apellido as Estudiante,
                    m.NombreMateria,
                    n.Calificacion,
                    n.TipoEvaluacion,
                    n.FechaEvaluacion,
                    n.ID_Estudiante,
                    n.ID_Materia
                FROM Notas n
                INNER JOIN Estudiantes e ON n.ID_Estudiante = e.ID_Estudiante
                INNER JOIN Materias m ON n.ID_Materia = m.ID_Materia
                ORDER BY e.Apellido, e.Nombre, m.NombreMateria";

                SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion);
                adaptador.Fill(tabla);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al obtener notas: " + ex.Message);
        }
        return tabla;
    }

    public bool AgregarNota(int idEstudiante, int idMateria, decimal calificacion, string tipoEvaluacion, DateTime fechaEvaluacion)
    {
        try
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string consulta = "INSERT INTO Notas (ID_Estudiante, ID_Materia, Calificacion, TipoEvaluacion, FechaEvaluacion) VALUES (@estudiante, @materia, @calificacion, @tipo, @fecha)";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@estudiante", idEstudiante);
                    comando.Parameters.AddWithValue("@materia", idMateria);
                    comando.Parameters.AddWithValue("@calificacion", calificacion);
                    comando.Parameters.AddWithValue("@tipo", tipoEvaluacion);
                    comando.Parameters.AddWithValue("@fecha", fechaEvaluacion);

                    comando.ExecuteNonQuery();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al agregar nota: " + ex.Message);
            return false;
        }
    }

    public bool ModificarNota(int idNota, int idEstudiante, int idMateria, decimal calificacion, string tipoEvaluacion, DateTime fechaEvaluacion)
    {
        try
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string consulta = "UPDATE Notas SET ID_Estudiante=@estudiante, ID_Materia=@materia, Calificacion=@calificacion, TipoEvaluacion=@tipo, FechaEvaluacion=@fecha WHERE ID_Nota=@id";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", idNota);
                    comando.Parameters.AddWithValue("@estudiante", idEstudiante);
                    comando.Parameters.AddWithValue("@materia", idMateria);
                    comando.Parameters.AddWithValue("@calificacion", calificacion);
                    comando.Parameters.AddWithValue("@tipo", tipoEvaluacion);
                    comando.Parameters.AddWithValue("@fecha", fechaEvaluacion);

                    comando.ExecuteNonQuery();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al modificar nota: " + ex.Message);
            return false;
        }
    }

    public bool EliminarNota(int id)
    {
        try
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                string consulta = "DELETE FROM Notas WHERE ID_Nota=@id";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    comando.ExecuteNonQuery();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al eliminar nota: " + ex.Message);
            return false;
        }
    }
}