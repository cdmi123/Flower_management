using Admin.Models;
using Flower_Management.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Org.BouncyCastle.Crypto.Macs;
using System.Data;
using System.Security.Cryptography;

namespace Admin.Controllers
{
    public class UserController : Controller
    {
        int total = 0;

        public IActionResult Index(slider sd,Product pm,cartmodel cm, category_model cam)
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

            DataSet cd = cam.get_category();
            ViewBag.cate_data = cd.Tables[0];

            int user_id = Convert.ToInt32(TempData.Peek("user_id"));


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

            foreach (System.Data.DataRow dr in ViewBag.Cart.Rows)
            {
                total = total + Convert.ToInt32(dr["p_price"].ToString());
            }

            return View();
        }

        public IActionResult order(ordermodel om) 
        {
            int user_id = Convert.ToInt32(TempData.Peek("user_id"));

           int data =  om.place_order(om.cname,om.fname,om.lname,om.com_name,om.address,om.city_name, om.state,om.code,om.email,om.phoneno,user_id);

            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("Flower Management", "languagepdf@gmail.com"));
            email.To.Add(new MailboxAddress("Flower Management",om.email));

            email.Subject = "Flower Management Your Order Confirmation!";

            var bodyBuilder = new BodyBuilder();

            bodyBuilder.HtmlBody = "<!DOCTYPE html><html><head>  <meta charset=\"utf-8\">  <meta http-equiv=\"x-ua-compatible\" content=\"ie=edge\">  <title>Email Confirmation</title>  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">  <style type=\"text/css\">  /**   * Google webfonts. Recommended to include the .woff version for cross-client compatibility.   */  @media screen {    @font-face {      font-family: 'Source Sans Pro';      font-style: normal;      font-weight: 400;      src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), format('woff');    }    @font-face {      font-family: 'Source Sans Pro'; font-style: normal; font-weight: 700;      src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'),format('woff');    }  }  /**   * Avoid browser level font resizing.   * 1. Windows Mobile   * 2. iOS / OSX   */  body,  table,  td,  a {    -ms-text-size-adjust: 100%; /* 1 */    -webkit-text-size-adjust: 100%; /* 2 */  }  /**   * Remove extra space added to tables and cells in Outlook.   */  table,  td {    mso-table-rspace: 0pt;    mso-table-lspace: 0pt;  }  /**   * Better fluid images in Internet Explorer.   */  img {    -ms-interpolation-mode: bicubic;  }  /**   * Remove blue links for iOS devices.   */  a[x-apple-data-detectors] {    font-family: inherit !important;    font-size: inherit !important;    font-weight: inherit !important;    line-height: inherit !important;    color: inherit !important;    text-decoration: none !important;  }  /**   * Fix centering issues in Android 4.4.   */  div[style*=\"margin: 16px 0;\"] {    margin: 0 !important;  }  body {    width: 100% !important;    height: 100% !important;    padding: 0 !important;    margin: 0 !important;  }  /**   * Collapse table borders to avoid space between cells.   */  table {    border-collapse: collapse !important;  }  a {    color: #1a82e2;  }  img {    height: auto;    line-height: 100%;    text-decoration: none;    border: 0;    outline: none;  }  </style></head><body style=\"background-color: #e9ecef;\">  <!-- start preheader -->  <div class=\"preheader\" style=\"display: none; max-width: 0; max-height: 0; overflow: hidden; font-size: 1px; line-height: 1px; color: #fff; opacity: 0;\"> " +
                "<table>" +
                "   <tr><td><b>Order Amount is :" + total + " </b></td></tr>" +
                "</table>" +
                "" +
                " </div>  <!-- end preheader -->  <!-- start body -->  <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">    <!-- start logo -->    <tr>      <td align=\"center\" bgcolor=\"#e9ecef\">        <!--[if (gte mso 9)|(IE)]>        <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\">        <tr>        <td align=\"center\" valign=\"top\" width=\"600\">        <![endif]-->        <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">          <tr>            <td align=\"center\" valign=\"top\" style=\"padding: 36px 24px;\">              <a href=\"https://sendgrid.com\" target=\"_blank\" style=\"display: inline-block;\">                <img src=\"./img/paste-logo-light@2x.png\" alt=\"Logo\" border=\"0\" width=\"48\" style=\"display: block; width: 48px; max-width: 48px; min-width: 48px;\">              </a>            </td>          </tr>        </table>        <!--[if (gte mso 9)|(IE)]>        </td>        </tr>        </table>        <![endif]-->      </td>    </tr>    <!-- end logo -->    <!-- start hero -->    <tr>      <td align=\"center\" bgcolor=\"#e9ecef\">        <!--[if (gte mso 9)|(IE)]>        <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\">        <tr>        <td align=\"center\" valign=\"top\" width=\"600\">        <![endif]-->        <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">          <tr>            <td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 36px 24px 0; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; border-top: 3px solid #d4dadf;\">              <h1 style=\"margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;\">Hey "+ om.fname +" "+ om.lname +",</h1><h4>We've got Your order! Your World is about to look  a whole lot better. We'll drop you another email when your order ships.</h4>            </td>          </tr>        </table>        <!--[if (gte mso 9)|(IE)]>        </td>        </tr>        </table>        <![endif]-->      </td>    </tr>    <!-- end hero -->    <!-- start copy block -->    <tr>      <td align=\"center\" bgcolor=\"#e9ecef\">        <!--[if (gte mso 9)|(IE)]>        <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\">        <tr>        <td align=\"center\" valign=\"top\" width=\"600\">        <![endif]-->        <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">          <!-- start copy -->          <tr>            <td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px            </td>          </tr>          <!-- end copy -->          <!-- start button -->          <tr>            <td align=\"left\" bgcolor=\"#ffffff\">              <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">                <tr>                  <td align=\"center\" bgcolor=\"#ffffff\" style=\"padding: 12px;\">                    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">                      <tr>                        <td align=\"center\" bgcolor=\"#1a82e2\" style=\"border-radius: 6px</td>                      </tr>                    </table>                  </td>                </tr>              </table>            </td>          </tr>          <!-- end button -->          <!-- start copy -->          <tr>            <td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\">              <p style=\"margin: 0;\">If that doesn't work, copy and paste the following link in your browser:</p>              <p style=\"margin: 0;\"><a href=\"https://sendgrid.com\" target=\"_blank\">https://same-link-as-button.url/xxx-xxx-xxxx</a></p>            </td>          </tr>          <!-- end copy -->          <!-- start copy -->          <tr>            <td align=\"left\" bgcolor=\"#ffffff\" style=\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf\">              <p style=\"margin: 0;\">Cheers,<br> Paste</p>            </td>          </tr>          <!-- end copy -->        </table>        <!--[if (gte mso 9)|(IE)]>        </td>        </tr>        </table>        <![endif]-->      </td>    </tr>    <!-- end copy block -->    <!-- start footer -->    <tr>      <td align=\"center\" bgcolor=\"#e9ecef\" style=\"padding: 24px;\">        <!--[if (gte mso 9)|(IE)]>        <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\">        <tr>        <td align=\"center\" valign=\"top\" width=\"600\">        <![endif]-->        <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">          <!-- start permission -->          <tr>            <td align=\"center\" bgcolor=\"#e9ecef\" style=\"padding: 12px 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; color: #666;\">              <p style=\"margin: 0;\">You received this email because we received a request for [type_of_action] for your account. If you didn't request [type_of_action] you can safely delete this email.</p>            </td>          </tr>          <!-- end permission -->          <!-- start unsubscribe -->          <tr>            <td align=\"center\" bgcolor=\"#e9ecef\" style=\"padding: 12px 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 20px; color: #666;\">              <p style=\"margin: 0;\">To stop receiving these emails, you can <a href=\"https://sendgrid.com\" target=\"_blank\">unsubscribe</a> at any time.</p>              <p style=\"margin: 0;\">Paste 1234 S. Broadway St. City, State 12345</p>            </td>          </tr>          <!-- end unsubscribe -->        </table>        <!--[if (gte mso 9)|(IE)]>        </td>        </tr>        </table>        <![endif]-->      </td>    </tr>    <!-- end footer -->  </table>  <!-- end body --></body></html>";

            email.Body = bodyBuilder.ToMessageBody();


            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                smtp.Authenticate("languagepdf@gmail.com", "olwjvldqpcfdfyxb");

                smtp.Send(email);
                smtp.Disconnect(true);
            }

            if (data > 0)
            {
                if (om.btn == "Order COD")
                {
                      return Redirect("success");
                }
                else
                {
                    return Redirect("successfull");
                }
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

        [HttpGet]
        public IActionResult success()
        {
            return View();
        }


    }
}
