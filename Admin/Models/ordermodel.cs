using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Admin.Models
{
    public class ordermodel
    {
        public string cname { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string com_name { get; set; }
        public string address { get; set; }
        public string city_name { get; set; }    
        public string state { get; set; } 
        public string code { get; set; } 
        public string email { get; set; }
        public string phoneno { get; set; }
        public int user_id { get; set; }

        SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Database=Flower_management;User Id=sa;pwd=cdmi@3420");

        public int place_order(string cname,string fname,string lname,string com_name, string address, string city_name,string state, string code, string email,string phoneno, int user_id)
        {
            SqlCommand cmd = new SqlCommand("insert into [dbo].[orders](cname,fname,lname,com_name,address,city_name,state,code,email,phoneno,user_id)values('" + cname + "','" + fname + "','" + lname + "','" + com_name + "','" + address + "','" + city_name + "','" + state + "','" + code + "','" + email + "','" + phoneno + "','" + user_id + "')", con);
            con.Open();

            con.Close();

            SqlCommand cmd1 = new SqlCommand("update cart set status=1 where user_id="+user_id,con);
            con.Open();

            cmd1.ExecuteNonQuery();

            return cmd.ExecuteNonQuery();
        }

        public DataSet get_order(int user_id)
        {
            SqlCommand cmd = new SqlCommand("SELECT orders.*,product.p_image,product.p_name,cart.* FROM orders JOIN cart ON orders.user_id = cart.user_id JOIN product ON product.id = cart.product_id where cart.status = 1 and orders.status=0", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            da.Fill(ds);

            return ds;
        }

        public DataSet get_c_order(int user_id)
        {
            SqlCommand cmd = new SqlCommand("SELECT orders.*,product.p_image,product.p_name,cart.* FROM orders JOIN cart ON orders.user_id = cart.user_id JOIN product ON product.id = cart.product_id where cart.status = 1 and orders.status=1", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            da.Fill(ds);

            return ds;
        }

        public int update_status(string status , int user_id , int id)
        {
            SqlCommand cmd1 = new SqlCommand("update orders set status='"+status+"' where o_id=" + user_id, con);
            con.Open();
            cmd1.ExecuteNonQuery();
            con.Close();


            SqlCommand cmd = new SqlCommand("update cart set order_status='" + status + "' where user_id=" + id, con);
            con.Open();
            return cmd.ExecuteNonQuery() ;
        }

        public DataSet user_order(int user_id)
        {
            SqlCommand cmd = new SqlCommand("SELECT cart.* , product.* FROM cart Inner JOIN product ON cart.product_id = product.id  where cart.user_id="+user_id, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            da.Fill(ds);

            return ds;
        }


    }
}
