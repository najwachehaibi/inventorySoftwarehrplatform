using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace InventoryBeginners.Models
{
    [Table("employee")]
    public partial class Employee
    {
        
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        //[Column("EMPCODE")]
       // public int Empcode { get; set; }
        [Required]
        [Column("EMPNAME")]
        [StringLength(100)]
        public string Empname { get; set; }
        [Required]
        [Column("EMPSURNAME")]
        [StringLength(100)]
        public string Empsurname { get; set; }
        [Column("PHONENUM")]
        public int Phonenum { get; set; }
        [Required]
        [Column("JOB")]
        [StringLength(50)]
        public string Job { get; set; }
        [Column("BIRTHDATE", TypeName = "date")]
        public DateTime Birthdate { get; set; }
        [Column("HIREDATE", TypeName = "date")]

        public DateTime Hiredate { get; set; }
        [Required]
        [Column("ADDRESS")]
        [StringLength(100)]
        public string Address { get; set; }
        [Required]
        [Column("SALARY")]
        public int Salary { get; set; }
        [StringLength(100)]
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage ="Email Address is not valid")]
        public string EmailAddress { get; set; }
        [StringLength(600)]
        [Required]
        public string Photo { get; set; }
        [StringLength(30)]
        [Required]
        public string Gender { get; set; }
        [StringLength(30)]
        [Required]
        public string Level { get; set; }

        [Display(Name = "Choose the cover photo of your book")]
        [Required]
        [NotMapped]
        public IFormFile CoverPhoto { get; set; }
        public string CoverImageUrl { get; set; }


    }
}
