using System.Data;
using System.Data.SqlClient;

namespace Admin.Models
{
    public class Product
    {
        SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Database=Flower_management;User Id=sa;pwd=cdmi@3420");

        public string p_name { get; set; }
        public string p_description { get; set; }
        public int p_price { get; set;}
        public int p_quntity { get; set; }
        public string p_image { get; set; }

        public int product_add(string p_name, string p_des, int p_price , int p_quntity , string p_image)
        {
            SqlCommand cmd = new SqlCommand("insert into [dbo].[product](p_name,p_description,p_price,p_quntity,p_image)values('" + p_name + "','" + p_des + "','" + p_price + "','"+ p_quntity + "','"+ p_image + "')", con);
            con.Open();

            return cmd.ExecuteNonQuery();
        }

        public DataSet get_product()
        {
            SqlCommand cmd = new SqlCommand("select * from product",con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet(); 
           
            da.Fill(ds);

            return ds;
        }

        public DataSet get_product_details(int id)
        {
            SqlCommand cmd = new SqlCommand("select * from product where id="+id, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            da.Fill(ds);

            return ds;
        }

        public int delete_product(int id)
        {
            
            SqlCommand cmd = new SqlCommand("delete from product where id=" + id, con);
            con.Open( );
            
            return cmd.ExecuteNonQuery();
        }

        public DataSet update_product(int id)
        {
            SqlCommand cmd = new SqlCommand("select * from product where id=" + id, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            da.Fill(ds);

            return ds;
        }


        public int update_product_data(string p_name, string p_des, int p_price, int p_quntity ,int id)
        {
            SqlCommand cmd = new SqlCommand("update [dbo].[product] set p_name='"+p_name+ "',p_description='" + p_des+"',p_price='"+p_price+"',p_quntity='"+p_quntity+"' where id="+id , con);
            con.Open();

            return cmd.ExecuteNonQuery();
        }

    }
}
