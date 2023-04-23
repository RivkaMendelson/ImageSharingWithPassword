using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace ImageSharing_Data
{
    public class ImageDatabase
    {
        private string _connectionString;

        public ImageDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddNewImage(Image image)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Images(FileName, Passcode, Views) VALUES(@name, @code, 0); SELECT SCOPE_IDENTITY()";
            command.Parameters.AddWithValue("@name", image.FileName);
            command.Parameters.AddWithValue("@code", image.Passcode);
            connection.Open();

            image.Id =(int)(decimal)command.ExecuteScalar();
        }

        public Image GetImageById(int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Images WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            return new Image
            {
                Id = (int)reader["Id"],
                FileName = (string)reader["FileName"],
                Passcode = (string)reader["Passcode"],
                Views = (int)reader["Views"]
            };
        }

        public void IncrementViews(int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "Update Images SET Views = (Views+1) WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();

            command.ExecuteNonQuery();
        }
    }
}