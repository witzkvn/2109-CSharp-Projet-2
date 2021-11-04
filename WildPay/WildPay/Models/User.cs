using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WildPay.Models
{
    public class User
    {
        public int Id { get; private set; }

        [Required(ErrorMessage = "L'adresse email est obligatoire")]
        [Index(IsUnique = true)]
        [EmailAddress(ErrorMessage = "Adresse email invalide")]
        public string Email { get; set; }

        [MaxLength(20, ErrorMessage = "Le prénom doit faire 20 caractères au maximum"), MinLength(0)]
        public string Firstname { get; set; }

        [MaxLength(20, ErrorMessage = "Le nom doit faire 20 caractères au maximum"), MinLength(0)]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Le mot de passe est obligatoire")]
        public string Password { get; set; }

        public byte[] Image { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }

        public User()
        {

        }
    }
}