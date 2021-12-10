using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WildPay.Models
{
    public class Group
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Le nom du groupe est obligatoire")]
        [MaxLength(50, ErrorMessage = "Le nom doit faire 50 caractères au maximum"), MinLength(0)]
        public string Name { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        //[InverseProperty("Members")]
        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }

    }
}