﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chamsoc.Models
{
    public class Caregiver
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public string Skills { get; set; } = null!;

        public bool IsAvailable { get; set; } = true;

        public string? Degree { get; set; }

        public string? Certificate { get; set; }

        public string? IdentityAndHealthDocs { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public bool IsVerified { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Rating { get; set; }

        public int CompletedJobs { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal HourlyRate { get; set; }

        public string Experience { get; set; } = null!;
        
        [Required]
        public string Pricing { get; set; } = "{\"1Hour\": 800000, \"2Hours\": 1500000, \"5Sessions\": 3500000}";
        
        public double TotalRatings { get; set; }

        public string? Name { get; set; }
        public string? Contact { get; set; }
        public string? AvatarUrl { get; set; }
        public string? CertificateFilePath { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string? FullName { get; set; }

        public ICollection<CareJob> CareJobs { get; set; } = new List<CareJob>();
    }
}