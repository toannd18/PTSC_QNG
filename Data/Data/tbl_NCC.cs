namespace Data.Data
{
    using System.ComponentModel.DataAnnotations;

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
        public string Hang_Hoa { get; set; }
        public string Dich_Vu { get; set; }
        public string Diem { get; set; }
        public int? Time { get; set; }
    }
}