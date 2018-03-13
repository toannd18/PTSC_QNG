namespace Data.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_NCC
    {
        [Key]
        [StringLength(10)]
        public string Ma_NCC { get; set; }

        [Required]
        [StringLength(100)]
        public string Ten_NCC { get; set; }

        [StringLength(100)]
        public string Dia_Chi { get; set; }

        [StringLength(20)]
        public string Tel { get; set; }

        [StringLength(20)]
        public string Fax { get; set; }

        [StringLength(50)]
        public string Attn { get; set; }

        [StringLength(50)]
        public string Email { get; set; }
    }
}
