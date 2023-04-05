using Admin.Models;
using Flower_Management.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using System.Net.Http.Headers;


namespace Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(Product pm,RegisterModel rm,slidermodel sm )
        {

            if (TempData.Peek("Admin_id") != null)
            {
                ViewBag.Name = TempData.Peek("Admin_name");
                return RedirectToAction("dashboard");
            }
            else
            {
                ViewBag.Name = "";
            }



            return View();
        }

        [HttpPost]
        public IActionResult Index(Loginmodel lm)
        {
            DataSet ds = lm.Login(lm.email, lm.password);
            ViewBag.user_data = ds.Tables[0];

            foreach (System.Data.DataRow dr in ViewBag.user_data.Rows)
            {
                TempData["Admin_id"] = dr["id"].ToString();
                TempData["Admin_name"] = dr["user_name"].ToString();
                TempData["Admin_email"] = dr["email"].ToString();
                return RedirectToAction("dashboard");
            }
            return RedirectToAction("dashboard");
        }

        public IActionResult dashboard(Product pm,ordermodel om)
        {
            DataSet ds = pm.get_product();

            DataSet dom = om.get_all_order();

            DataSet com = om.get_com_order();

            DataSet ao = om.get_order();


            TempData["Total_Product"] = ds.Tables[0].Rows.Count;
            TempData["Total_order"] = dom.Tables[0].Rows.Count;
            TempData["Total_com_order"] = com.Tables[0].Rows.Count;
            TempData["Total_order_in"] = ao.Tables[0].Rows.Count;




            return View();
        }

        [HttpGet]
        public IActionResult Add_slider()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> add_slider(slidermodel sm, IFormFile formFile)
        {
            var imgname = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "sliderimage", formFile.FileName);

            using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }


            int record = sm.slider(sm.title,sm.description, imgname);

            if (record > 0)
            {
                TempData["Slider_msg"] = "Add_slider";
                return RedirectToAction("Add_slider");

            }
            else
            {
                return RedirectToAction("");
            }

           
        }

        public IActionResult View_slider(slidermodel sm)
        {
            DataSet ds = sm.get_slider();
            ViewBag.Slider = ds.Tables[0];

            return View();
        }

        [HttpGet]
        public IActionResult Add_product(category_model cam)
        {

            DataSet cd = cam.get_category();
            ViewBag.category = cd.Tables[0];

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> add_product(Product pm, IFormFile formFile)
        {
            var imgname = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "productImage", formFile.FileName);

            using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }


            int record = pm.product_add(pm.p_name, pm.p_description,pm.p_price,pm.p_quntity ,imgname,pm.p_category);

            if (record > 0)
            {
                return RedirectToAction("Add_product");
            }
            else
            {
                return RedirectToAction("");
            }

           
        }

        [HttpGet]
        public IActionResult View_product(Product pm)
        {
            DataSet ds = pm.get_product();
            ViewBag.product = ds.Tables[0];

            TempData["Total_Product"] = ds.Tables[0].Rows.Count;

            return View();
        }

        public IActionResult Confirm_order(ordermodel om)
        {
            int id = Convert.ToInt32(TempData.Peek("user_id"));

            DataSet ds = om.get_c_order(id);
            ViewBag.orders = ds.Tables[0];

            return View();
        }

        public IActionResult View_Order(ordermodel om)
        {
            int id = Convert.ToInt32(TempData.Peek("user_id"));

            DataSet ds = om.get_order(id);
            ViewBag.orders = ds.Tables[0];

            return View();
        }

        public IActionResult update_order_status(ordermodel om,int accept_id = 0,int Decline_id = 0)
        {
            int user_id = Convert.ToInt32(TempData.Peek("user_id"));

            if (accept_id != 0)
            {
                om.update_status("1", accept_id , user_id);
            }

            if (Decline_id != 0)
            {
                om.update_status("2", Decline_id , user_id);
            }
            




            return Redirect("View_Order");
        }

        [HttpGet]
        public IActionResult delete_product(Product pm, int product_id)
        {
            pm.delete_product(product_id);
            return RedirectToAction("View_product");
        }

        [HttpGet]
        public IActionResult delete_slider(slidermodel sm, int slider_id)
        {
            sm.delete_slider(slider_id);
            return RedirectToAction("View_slider");
        }

        [HttpPost]
        public IActionResult update_product_data(Product pm)
        {
            int produ_id = Convert.ToInt32(TempData.Peek("product_id"));
            int record = pm.update_product_data(pm.p_name, pm.p_description, pm.p_price, pm.p_quntity, produ_id);

            return RedirectToAction("View_product");
        }

        [HttpGet]
        public IActionResult upadte_product(Product pm, int product_id)
        {
            DataSet ds = pm.update_product(product_id);
            ViewBag.update_product = ds.Tables[0];
            TempData["product_id"] = product_id;

            return View();
        }

        [HttpGet]
        public IActionResult logout()
        {
            TempData.Clear();   

            return RedirectToAction("Index");
        }


        public IActionResult add_category()
        {
            return  View();
        }

        [HttpPost]
        public IActionResult add_category(category_model cm)
        {
            var cat_name = cm.category_name;

            var result = cm.add_cat(cat_name);

            if (result > 0)
            {
                RedirectToAction("add_category");
            }


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}