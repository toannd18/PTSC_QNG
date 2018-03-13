using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataModel.Model.Systems
{
    public class AppUserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Yêu Cầu Nhập Đầy Đủ Họ Tên")]
        [Display(Name ="Họ và Tên")]
        public string FullName { get; set; }
        [Display(Name = "Địa Chỉ")]
        public string Address { get; set; }
        [Display(Name = "Ảnh Đại Diện")]
        public string Avatar { get; set; }
        [Display(Name = "Ngày Sinh Nhật")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<System.DateTime> BrithDay { get; set; }
        [Display(Name = "Tình Trạng")]
        public bool Status { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Chọn Giới Tính")]
        [Display(Name = "Giới Tính")]
        public bool Gender { get; set; }
        [EmailAddress(ErrorMessage = "Yêu Cầu Nhập Địa Chỉ Email")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Yêu Cầu Nhập Mật Khẩu")]
        [Display(Name = "Mật Khẩu")]
        
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Nhập Mật Khẩu")]
        
        [Display(Name = "Xác Nhận Mật Khẩu")]
        [System.ComponentModel.DataAnnotations.Compare("PasswordHash",ErrorMessage ="Mật Khẩu và Mật Khẩu Xác Nhận Phải Trùng Nhau")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage ="Yêu Cầu Nhập Tài Khoản")]
        [Display(Name = "Tài Khoản")]
        [Remote("CheckExist","Users",ErrorMessage ="Tài Khoản này đã tồn tại")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Chọn Phòng Ban")]
        [Display(Name = "Phòng Ban")]
        public string Ma_BP { get; set; }
        
        [Display(Name = "Tổ")]
        public string Ma_TO { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Chọn Chức vụ")]
        [Display(Name = "Chức Vụ")]
        public string Ma_CV { get; set; }
        public string previewImage { get; set; }
        [NotMapped]
        public HttpPostedFileBase uploadanh { get; set; }
        public AppUserModel()
        {
            Avatar = "/images/user.png";
        }
    }
}