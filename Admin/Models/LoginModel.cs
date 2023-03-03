using System.Data;
using System.Data.SqlClient;

namespace Flower_Management.Models
{
    public class LoginModel
    {
        public string email { get; set; }
        public string password { get; set; }


        SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Database=Flower_management;User Id=sa;pwd=cdmi@3420");

        public DataSet Login(string email, string password)
        {
            SqlCommand cmd = new SqlCommand("select * from [dbo].[users] where email='" + email + "' and password='" + password + "'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }

    }
}
