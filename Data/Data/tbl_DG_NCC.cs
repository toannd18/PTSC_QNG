using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Data.Data
{
    public partial class tbl_DG_NCC
    {
        [Key]
        public int Id { get; set; }
        public int DeXuatId { get; set; }
        public string Ma_NCC { get; set; }
        public bool? DG_KT { get; set; }
        public int? DG_TM { get; set; }
        public int? DG { get; set; }
    }
}
