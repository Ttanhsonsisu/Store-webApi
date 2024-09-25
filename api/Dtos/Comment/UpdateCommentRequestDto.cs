using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be 5 character")]
        [MaxLength(255, ErrorMessage = "Title can't over 255 character")]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [MinLength(5, ErrorMessage = "Title must be 5 character")]
        [MaxLength(255, ErrorMessage = "Title can't over 255 character")]
        public string Content { get; set; } = string.Empty;
    }
}