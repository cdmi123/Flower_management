using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace Admin.Models
{
    public class slidermodel
    {
        public string title{ get; set; }

        public string description { get; set; }

        public string image { get; set; }

        SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Database=Flower_management;User Id=sa;pwd=cdmi@3420");

        public int slider(string title, string description,string image)
        {
            SqlCommand cmd = new SqlCommand("insert into [dbo].[slider](title,description,image)values('" + title + "','" +description+ "','"+image+"')",con);
            con.Open();

            return cmd.ExecuteNonQuery();
        }

        public DataSet get_slider()
        {
            SqlCommand cmd = new SqlCommand("select * from [dbo].[slider]", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }
    }
}
