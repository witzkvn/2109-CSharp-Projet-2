using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WildPay.Tools;

namespace WildPay.Models
{
    public class ExpenseCategoryJoin
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Title { get; set; }
        public double Value { get; set; }
        public int FkUserId { get; set; }
        public int? FkCategoryId { get; set; }
        public int FkGroupId { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public byte[] UserImage { get; set; }
        public string UserImageFile { get; set; }
        public string DateCourte { get; set; }


    }
}