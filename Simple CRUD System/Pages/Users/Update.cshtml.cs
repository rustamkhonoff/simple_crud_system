using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Simple_CRUD_System.Pages.Users
{
    public class UpdateModel : PageModel
    {
        public UserInfo UserInfo = new UserInfo();

        public string ErrorMessage = "";
        public string SuccessMessage = "";
        public void OnGet()
        {
            string id = Request.Query["id"];

            try
            {
                using (SqlConnection connection = new SqlConnection(Properties.ConnectionString))
                {
                    connection.Open();
                    string sqlCommand = "SELECT * FROM users WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserInfo.Id = "" + reader.GetInt32(0);
                                UserInfo.Name = reader.GetString(1);
                                UserInfo.Email = reader.GetString(2);
                                UserInfo.Phone = reader.GetString(3);
                                UserInfo.Address = reader.GetString(4);
                                UserInfo.CreatedTime = reader.GetDateTime(5).ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }

        public void OnPost()
        {
            UserInfo.Id = Request.Form["id"];
            UserInfo.Name = Request.Form["name"];
            UserInfo.Email = Request.Form["email"];
            UserInfo.Phone = Request.Form["phone"];
            UserInfo.Address = Request.Form["address"];

            if (UserInfo.IsEmpty)
            {
                ErrorMessage = "All data are required !";
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(Properties.ConnectionString))
                {
                    connection.Open();

                    string sqlCommand = "UPDATE users " +
                        "SET name=@name, email=@email, phone=@phone, address=@address " +
                        "WHERE id=@id";


                    using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                    {
                        command.Parameters.AddWithValue("@name", UserInfo.Name);
                        command.Parameters.AddWithValue("@email", UserInfo.Email);
                        command.Parameters.AddWithValue("@phone", UserInfo.Phone);
                        command.Parameters.AddWithValue("@address", UserInfo.Address);

                        command.Parameters.AddWithValue("@id", UserInfo.Id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return;
            }
            finally
            {
                Response.Redirect("/Users");
            }
        }
    }
}
