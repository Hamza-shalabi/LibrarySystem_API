﻿using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.DTOs.AuthorDTO
{
    public class UpdateAuthorRequestDto
    {
        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Bio { get; set; } = string.Empty;
    }
}
