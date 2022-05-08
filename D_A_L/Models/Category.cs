using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace D_A_L.Model
{
   public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(30)]
        public string CategoryName { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
