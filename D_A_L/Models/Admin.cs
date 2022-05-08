using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace D_A_L.Model
{
    public class Admin
    {

        [Key]
        public int AdminId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(10)]
        public string Username { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(10)]
        public string Password { get; set; }

        public int Authority { get; set; }
    }
}
