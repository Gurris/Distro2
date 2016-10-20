using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Distro2.Models
{
    public class MessageModel
    {
        [Key]
        public int messageId { get; set; }
        [Display(Name ="Title")]
        [Required]
        public string title { get; set; }
        [Display(Name = "Message")]
        [Required]
        public string message { get; set; }
        [Required]
        public DateTime date { get; set; }
        [Required]
        public bool read { get; set; }
        
        [Display(Name = "Sender")]
        public virtual ApplicationUser fromUser { get; set; } // sender
        
        [Display(Name = "Reciver")]
        public virtual ApplicationUser toUser { get; set; } // reciver

    }

}