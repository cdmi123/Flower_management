using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Xml.Linq;

namespace Admin.Models
{
    public class cartmodel
    {
        public int product_id { get; set; }
        public int user_id { get; set; }
        public int quntity { get; set; }

        SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Database=Flower_management;User Id=sa;pwd=cdmi@3420");

        public int add_cart(int product_id,int user_id, int quntity)
        {
            if (quntity == 0)
            {
                quntity= 1;
            }

            SqlCommand cmd = new SqlCommand("insert into [dbo].[cart](product_id,user_id,quntity)values('" + product_id + "','" + user_id + "','"+quntity+"')", con);
            con.Open();

            return cmd.ExecuteNonQuery();
        }

        public int get_cart_details(int product_id,int user_id)
        {
            SqlCommand cmd = new SqlCommand("select * from [dbo].[cart] where product_id=" + product_id + "and user_id="+user_id+"and status=0",con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            int total = ds.Tables[0].Rows.Count;

            return total;

        }

        public DataSet get_cart(int user_id)
        {
            SqlCommand cmd = new SqlCommand("SELECT cart.quntity,cart.user_id, product.* FROM cart JOIN product ON cart.product_id=product.id where cart.user_id='"+user_id+"' and status=0",con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;  
        }

    }
}
