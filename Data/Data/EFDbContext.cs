namespace Data.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("name=EFDBContext")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<AppRole> AppRoles { get; set; }
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<AppUserRole> AppUserRoles { get; set; }
        public virtual DbSet<Function> Functions { get; set; }
        public virtual DbSet<Log_in> Log_in { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<tbl_BP> tbl_BP { get; set; }
        public virtual DbSet<tbl_CV> tbl_CV { get; set; }
        public virtual DbSet<tbl_Daily> tbl_Daily { get; set; }
        public virtual DbSet<tbl_DailyDetail> tbl_DailyDetail { get; set; }
        public virtual DbSet<tbl_Job> tbl_Job { get; set; }
        public virtual DbSet<tbl_List_Check> tbl_List_Check { get; set; }
        public virtual DbSet<tbl_List_Request> tbl_List_Request { get; set; }
        public virtual DbSet<tbl_NCC> tbl_NCC { get; set; }
        public virtual DbSet<tbl_Notifications> tbl_Notifications { get; set; }
     
        public virtual DbSet<tbl_TO> tbl_TO { get; set; }
        public virtual DbSet<tbl_DeXuat> tbl_DeXuat { get; set; }
        public virtual DbSet<tbl_DeXuat_KT> tbl_DeXuat_KT { get; set; }

        public virtual DbSet<tbl_DG_NCC> tbl_DG_NCC { get; set; }
        public virtual DbSet<tbl_DeXuat_TM> tbl_DeXuat_TM { get; set; }
        public virtual DbSet<tbl_DG_KT> tbl_DG_KT { get; set; }
        public virtual DbSet<tbl_DG_TM> tbl_DG_TM { get; set; }
        public virtual DbSet<tbl_DeXuat_HD> tbl_DeXuat_HD { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppRole>()
                .Property(e => e.Role)
                .IsUnicode(false);

            modelBuilder.Entity<AppRole>()
                .HasMany(e => e.AppUserRoles)
                .WithRequired(e => e.AppRole)
                .HasForeignKey(e => e.RoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AppRole>()
                .HasMany(e => e.Permissions)
                .WithRequired(e => e.AppRole)
                .HasForeignKey(e => e.RoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.Avatar)
                .IsUnicode(false);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.PasswordHash)
                .IsUnicode(false);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.Ma_BP)
                .IsUnicode(false);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.Ma_TO)
                .IsUnicode(false);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.Ma_CV)
                .IsUnicode(false);

            modelBuilder.Entity<AppUser>()
                .HasMany(e => e.AppUserRoles)
                .WithRequired(e => e.AppUser)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Function>()
                .Property(e => e.Id)
                .IsUnicode(false);

            modelBuilder.Entity<Function>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<Function>()
                .Property(e => e.ParentId)
                .IsUnicode(false);

            modelBuilder.Entity<Function>()
                .Property(e => e.IconCss)
                .IsUnicode(false);

            modelBuilder.Entity<Function>()
                .HasMany(e => e.Functions1)
                .WithOptional(e => e.Function1)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<Function>()
                .HasMany(e => e.Permissions)
                .WithRequired(e => e.Function)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Log_in>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Log_in>()
                .Property(e => e.tbl_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Permission>()
                .Property(e => e.FunctionId)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_BP>()
                .Property(e => e.Ma_BP)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_BP>()
                .HasMany(e => e.tbl_Job)
                .WithRequired(e => e.tbl_BP)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_BP>()
                .HasMany(e => e.tbl_TO)
                .WithRequired(e => e.tbl_BP)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_CV>()
                .Property(e => e.Ma_CV)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Daily>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Daily>()
                .Property(e => e.User_Autho1)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Daily>()
                .Property(e => e.User_Autho2)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Daily>()
                .Property(e => e.User_Autho3)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Daily>()
                .HasMany(e => e.tbl_DailyDetail)
                .WithRequired(e => e.tbl_Daily)
                .HasForeignKey(e => e.DailyId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbl_Job>()
                .Property(e => e.Ma_BP)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Job>()
                .Property(e => e.Ma_TO)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_List_Check>()
                .Property(e => e.Ma_NCC)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_List_Check>()
                .Property(e => e.Ma_TB)
                .IsUnicode(true);

            modelBuilder.Entity<tbl_List_Check>()
                .Property(e => e.User_Nhap)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_List_Check>()
                .Property(e => e.User_Edit)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_List_Request>()
                .Property(e => e.LateId)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_List_Request>()
                .Property(e => e.Ma_BP)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_List_Request>()
                .Property(e => e.User_Nhap)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_List_Request>()
                .Property(e => e.User_Edit)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_List_Request>()
                .Property(e => e.User_Autho)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_List_Request>()
                .Property(e => e.Status_Autho)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_NCC>()
                .Property(e => e.Ma_NCC)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_NCC>()
                .Property(e => e.Tel)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_NCC>()
                .Property(e => e.Fax)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_NCC>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Notifications>()
                .Property(e => e.SendId)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Notifications>()
                .Property(e => e.ReceiveId)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Notifications>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_TO>()
                .Property(e => e.Ma_TO)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_TO>()
                .Property(e => e.Ma_BP)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_DeXuat>()
                .Property(e => e.Ma)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_DeXuat>()
                .Property(e => e.Ten_Dx)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_DeXuat>()
               .Property(e => e.Ten_Dx1)
               .IsUnicode(false);

            modelBuilder.Entity<tbl_DeXuat>()
               .Property(e => e.Ten_Dx2)
               .IsUnicode(false);

            modelBuilder.Entity<tbl_DeXuat>()
               .Property(e => e.Ten_Dx3)
               .IsUnicode(false);

            modelBuilder.Entity<tbl_DeXuat>()
               .Property(e => e.Ten_Dx4)
               .IsUnicode(false);

            modelBuilder.Entity<tbl_DeXuat>()
               .Property(e => e.Ten_Dx5)
               .IsUnicode(false);
            modelBuilder.Entity<tbl_DG_NCC>()
                .Property(e => e.Ma_NCC)
                .IsUnicode(false);
            modelBuilder.Entity<tbl_DeXuat_TM>()
                .Property(e => e.Loai_Tien)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_DeXuat_HD>()
                .Property(e => e.Ma_NCC)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_DeXuat_HD>()
                .Property(e => e.Diem)
                .IsUnicode(false);


        }

       
    }
}
