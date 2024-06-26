﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace D_A_L.Model
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(30)]
        public string StaffName { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(30)]
        public string StaffSurname { get; set; }

        public bool Status { get; set; }

    }
}
