using System;
using System.ComponentModel.DataAnnotations;

namespace Distro2.Models
{
    public class UserLoginTime
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public virtual ApplicationUser user { get; set; }
        [Required]
        public DateTime loginDate { get; set; }
    }

    public class IndexPageViewModel
    {
        [Display(Name = "Username")]
        public string name { get; set; }
        [Display(Name = "Last login")]
        public DateTime lastLogin { get; set; }
        [Display(Name = "Nr of logins this month")]
        public int nrOfLoginsThisMonth { get; set; }
    }
}