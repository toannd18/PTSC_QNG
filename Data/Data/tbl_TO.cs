namespace Data.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_TO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_TO()
        {
            AppUsers = new HashSet<AppUser>();
        }

        [Key]
        [StringLength(10)]
        public string Ma_TO { get; set; }

        [StringLength(50)]
        public string Ten_TO { get; set; }

        [Required]
        [StringLength(10)]
        public string Ma_BP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppUser> AppUsers { get; set; }

        public virtual tbl_BP tbl_BP { get; set; }
    }
}
