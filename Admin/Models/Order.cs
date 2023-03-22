using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class RazorPayOrder
    {
        public string OrderId { get; set; }
        public string RazorPayAPIKey { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }


    public class PaymentRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}
