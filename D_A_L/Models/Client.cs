using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace D_A_L.Model
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(20)]
        public string ClientName { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(30)]
        public string ClientSurname { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(100)]
        public string ClientAddress { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(11)]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(11)]
        public string PhoneNumber1 { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(11)]
        public string PhoneNumber2 { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string ClientMail { get; set; }

        public bool Status { get; set; }

      
    }

   
}
