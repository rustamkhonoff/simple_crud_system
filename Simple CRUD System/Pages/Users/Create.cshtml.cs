using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Simple_CRUD_System.Pages.Users
{
    public class CreateModel : PageModel
    {
        public UserInfo UserInfo = new UserInfo();

        public string ErrorMessage = "";
        public string SuccessMessage = "";

        public void OnGet()
        {

        }
        public void OnPost()
        {
            var tempUser = UserInfo;

            tempUser.Name = Request.Form["name"];
            tempUser.Email = Request.Form["email"];
            tempUser.Phone = Request.Form["phone"];
            tempUser.Address = Request.Form["address"];

            if (tempUser.IsEmpty)
            {
                ErrorMessage = "All data are required !";
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(Properties.ConnectionString))
                {
                    connection.Open();

                    string sqlCommand = "INSERT INTO users " +
                        "(name, email, phone, address) " +
                        "VALUES " +
                        "(@name, @email, @phone, @address)";

                    using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                    {
                        command.Parameters.AddWithValue("@name", UserInfo.Name);
                        command.Parameters.AddWithValue("@email", UserInfo.Email);
                        command.Parameters.AddWithValue("@phone", UserInfo.Phone);
                        command.Parameters.AddWithValue("@address", UserInfo.Address);

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
