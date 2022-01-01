
using System;
using System.Collections.Generic;
using System.Text;

namespace MabaCore.Data.Models
{
    public class ProductUser
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
