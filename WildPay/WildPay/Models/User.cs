using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [EmailAddress(ErrorMessage = "Adresse email invalide")]
        [DisplayName("Adresse email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [MaxLength(20, ErrorMessage = "Le prénom doit faire 20 caractères au maximum"), MinLength(0)]
        [DisplayName("Prénom")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Le nom de famille est obligatoire")]
        [MaxLength(20, ErrorMessage = "Le nom doit faire 20 caractères au maximum"), MinLength(0)]
        [DisplayName("Nom")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Le mot de passe est obligatoire")]
        [DisplayName("Mot de passe")]
        public string Password { get; set; }

        public byte[] Image { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }

        public User()
        {

        }
    }
}