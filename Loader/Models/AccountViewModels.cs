﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loader.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
      
        [Required]
        [Display(Name ="Active")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name ="Unlimited")]
        public bool IsUnlimited { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public Nullable<int> EmployeeId { get; set; }

        [Required(ErrorMessage = "UserName Is Required"), MaxLength(100), Column(TypeName = "nvarchar")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> From { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> To { get; set; }

        [Display(Name = "MenuTemplate")]
        public int MTId { get; set; }


    }


    public class ResetPasswordViewModel
    {
        public string UserName { get; set; }
        //[Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }



        [DataType(DataType.Password)]
        [Display(Name = "New Password")]

        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }

        public string Code { get; set; }
        public int userId { get; set; }
    }

    public class ApplicationUserViewModel
    {
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]

        public string NewPassword { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
