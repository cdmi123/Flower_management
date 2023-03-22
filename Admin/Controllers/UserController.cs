using Admin.Models;
using Flower_Management.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Cryptography;

namespace Admin.Controllers
{
    public class UserController : Controller
    {


        public IActionResult Index(slider sd,Product pm,cartmodel cm)
        {
            if (TempData.Peek("Name") != null)
            {
                ViewBag.Name = TempData.Peek("Name");
                ViewBag.Id = TempData.Peek("user_id");

            }
            else
            {
                ViewBag.Name = "";
            }

            DataSet ds = sd.get_slider();
            ViewBag.DataSource = ds.Tables[0];
            @ViewBag.count = 0;

            DataSet ds1 = pm.get_product();
            ViewBag.product = ds1.Tables[0];

          int user_id =   Convert.ToInt32(TempData.Peek("user_id"));


            DataSet ds2 = cm.get_cart(user_id);
            ViewBag.Cart = ds2.Tables[0];

           ViewBag.count =  ViewBag.Cart.Rows.Count;


            return View();
        }

        public IActionResult Shop(Product pm)
        {
            DataSet ds1 = pm.get_product();
            ViewBag.product = ds1.Tables[0];
            return View();
        }

        public IActionResult Aboutus()
        {
            return View();
        }

        public IActionResult Contactus()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            DataSet ds = loginModel.Login(loginModel.email, loginModel.password);
            ViewBag.user_data = ds.Tables[0];

            foreach (System.Data.DataRow dr in ViewBag.user_data.Rows)
            {
                TempData["user_id"] = dr["user_id"].ToString();
                TempData["Name"] = dr["user_name"].ToString();
                TempData["Email"] = dr["email"].ToString();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterModel registerModel)
        {
            int record = registerModel.Register(registerModel.name, registerModel.email, registerModel.password);

            if (record > 0)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

     

        public IActionResult forget()
        {
            return View();
        }
        public IActionResult product_details(int id,Product pm)
        {
            DataSet ds = pm.get_product_details(id);
            ViewBag.product_detail = ds.Tables[0];

            return View();
        }
        public IActionResult cart()
        {
            return View();
        }
        public IActionResult wishlist()
        {
            return View();
        }

        public IActionResult checkout(cartmodel cm)
        {

            int user_id = Convert.ToInt32(TempData.Peek("user_id"));

            DataSet ds2 = cm.get_cart(user_id);
         
            ViewBag.Cart = ds2.Tables[0];

            return View();
        }

        public IActionResult order(ordermodel om) 
        {
            int user_id = Convert.ToInt32(TempData.Peek("user_id"));

           int data =  om.place_order(om.cname,om.fname,om.lname,om.com_name,om.address,om.city_name, om.state,om.code,om.email,om.phoneno,user_id);



            if (data > 0)
            {
                return Redirect("successfull");
            }
            else 
            {
                return Redirect("checkout");
            }
        }

        [HttpGet]
        public IActionResult add_to_cart(cartmodel cm , int product_id=0 , int user_id=0 , int quntity=1)
        {
            user_id = Convert.ToInt32(TempData.Peek("user_id"));

            int product_count = cm.get_cart_details(product_id,user_id);

            if (product_count == 0)
            {
                int data = cm.add_cart(product_id, user_id,quntity);

                if (data > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            { 
                return RedirectToAction("Index");
            }
        }

        public IActionResult view_order(ordermodel om)
        {
            int user_id = Convert.ToInt32(TempData.Peek("user_id"));

           DataSet ds =  om.user_order(user_id);
            ViewBag.u_order = ds.Tables[0];

            return View();
        }

        public IActionResult Logout()
        {
            TempData.Clear();
            return RedirectToAction("Index");   
        }

        public IActionResult successfull()
        {
            return View();
        }


    }
}
