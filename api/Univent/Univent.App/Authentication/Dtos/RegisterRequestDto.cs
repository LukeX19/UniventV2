﻿using System.ComponentModel.DataAnnotations;
using Univent.App.ValidationAttributes;
using Univent.Domain.Enums;

namespace Univent.App.Authentication.Dtos
{
    public class RegisterRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        public string? PictureURL { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Range(0, 1, ErrorMessage = "Invalid role.")]
        public AppRole Role { get; set; }

        [Range(0, 8, ErrorMessage = "Invalid university year option.")]
        public UniversityYear Year { get; set; }

        [UniversityRequiredForStudents]
        public Guid? UniversityId { get; set; }
    }
}
