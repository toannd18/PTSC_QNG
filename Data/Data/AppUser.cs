namespace Data.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AppUser")]
    public partial class AppUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AppUser()
        {
            AppUserRoles = new HashSet<AppUserRole>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string Avatar { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BrithDay { get; set; }

        public bool Status { get; set; }

        public bool Gender { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(10)]
        public string Ma_BP { get; set; }

        [StringLength(10)]
        public string Ma_TO { get; set; }

        [StringLength(10)]
        public string Ma_CV { get; set; }

        public virtual tbl_BP tbl_BP { get; set; }

        public virtual tbl_CV tbl_CV { get; set; }

        public virtual tbl_TO tbl_TO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppUserRole> AppUserRoles { get; set; }
    }
}
