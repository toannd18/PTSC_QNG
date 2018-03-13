namespace Data.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Permission
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string FunctionId { get; set; }

        public bool CanRead { get; set; }

        public bool CanUpdate { get; set; }

        public bool CanCreate { get; set; }

        public bool CanDelete { get; set; }

        public virtual AppRole AppRole { get; set; }

        public virtual Function Function { get; set; }
    }
}
