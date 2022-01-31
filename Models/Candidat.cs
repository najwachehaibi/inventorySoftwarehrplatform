using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryBeginners.Models
{
    public class Candidat
    {
        [Key]
        public int CandidatId { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }

        public int Phone { get; set; }
        public string Level { get; set; }
        public string DimplomaTitle { get; set; }

        public int YearsExperience { get; set; }
        public string Experience { get; set; }
        //public string PhotoUrl { get; set; }

        [Display(Name = "Choose the cover photo of your book")]
        [Required]
        [NotMapped]
        public IFormFile CoverPhoto { get; set; }
        public string CoverImageUrl { get; set; }

        [Display(Name = "Upload your book in pdf format")]
        [Required]
        [NotMapped]
        public IFormFile BookPdf { get; set; }
        public string BookPdfUrl { get; set; }
    }
}
