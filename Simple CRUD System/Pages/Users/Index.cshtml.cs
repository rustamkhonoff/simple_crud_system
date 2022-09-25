using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Simple_CRUD_System.Pages.Users
{
    public class IndexModel : PageModel
    {
        private List<UserInfo> _usersList = new List<UserInfo>();

        public List<UserInfo> UsersList => _usersList;

        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Properties.ConnectionString))
                {
                    connection.Open();
                    string sqlCommand = "SELECT * FROM users";
                    using (SqlCommand cmd = new SqlCommand(sqlCommand, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserInfo userInfo = new UserInfo();
                                userInfo.Id = "" + reader.GetInt32(0);
                                userInfo.Name = reader.GetString(1);
                                userInfo.Email = reader.GetString(2);
                                userInfo.Phone = reader.GetString(3);
                                userInfo.Address = reader.GetString(4);
                                userInfo.CreatedTime = reader.GetDateTime(5).ToString();

                                _usersList.Add(userInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
    public class UserInfo
    {
        public string Id;
        public string Name;
        public string Email;
        public string Phone;
        public string Address;
        public string CreatedTime;

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(Name) ||
                string.IsNullOrEmpty(Email) ||
                string.IsNullOrEmpty(Phone) ||
                string.IsNullOrEmpty(Address);
            }
        }
    }
}
