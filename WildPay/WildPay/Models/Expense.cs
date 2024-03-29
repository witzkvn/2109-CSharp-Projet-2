﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WildPay.Models
{
    public class Expense
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        [DisplayName("Date")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Le titre de la dépense est obligatoire")]
        [MaxLength(30, ErrorMessage = "Le titre de la dépense doit faire 30 caractères au maximum")]
        [MinLength(2, ErrorMessage = "Le titre de la dépense doit faire 2 caractères au minimum")]
        [DisplayName("Titre")]
        public string Title { get; set; }

        [Required(ErrorMessage = "La valeur de la dépense est obligatoire")]
        [Range(0, Double.PositiveInfinity)]
        [DisplayName("Montant")]
        public double Value { get; set; }

        [Required(ErrorMessage ="Le nom de l'utilisateur est obligatoire")]
        public int FkUserId { get; set; }
        [ForeignKey("FkUserId")]
        public User User { get; set; }

        public int? FkCategoryId { get; set; }
        [ForeignKey("FkCategoryId")]
        public Category Category { get; set; }

        public int FkGroupId { get; set; }
        [ForeignKey("FkGroupId")]
        public Group Group { get; set; }

        public Expense()
        {

        }
    }
}