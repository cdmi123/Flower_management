using System.Data.SqlClient;

namespace Flower_Management.Models
{
    public class RegisterModel
    {
        public string name { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Database=Flower_management;User Id=sa;pwd=cdmi@3420");

        public int Register(string name, string email, string password)
        {
            SqlCommand cmd = new SqlCommand("insert into [dbo].[users](user_name,email,password)values('" + name + "','" + email + "','" + password + "')", con);
            con.Open();

            return cmd.ExecuteNonQuery();
        }
    }
}
