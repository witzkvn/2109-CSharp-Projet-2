using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WildPay.Models
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de la catégorie est obligatoire")]
        [MaxLength(20, ErrorMessage = "Le nom doit faire 20 caractères au maximum")]
        [MinLength(2, ErrorMessage = "Le nom doit faire 2 caractères au minimum")]
        [DisplayName("Nom")]
        public string Name { get; set; }
        public bool IsBase { get; set; } = false;

        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }

        public Category()
        {
            this.Expenses = new List<Expense>();
            this.Groups = new List<Group>();
        }
    }
}