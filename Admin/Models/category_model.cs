using System.Data.SqlClient;
using System.Data;

namespace Admin.Models
{
    public class category_model
    {
        public string category_name { get; set; }

        SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Database=Flower_management;User Id=sa;pwd=cdmi@3420");

        public int add_cat(string cat_name)
        {

            SqlCommand cmd = new SqlCommand("insert into [dbo].[category](cat_name)values('" + cat_name + "')", con);
            con.Open();

            return cmd.ExecuteNonQuery();
        }

        public DataSet get_category()
        {
            SqlCommand cmd = new SqlCommand("SELECT category.cat_name , COUNT(product.p_category) AS total_data  from category left join product on category.cat_name = product.p_category group by category.cat_name ORDER BY  COUNT(product.p_category) ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }
    }
}
