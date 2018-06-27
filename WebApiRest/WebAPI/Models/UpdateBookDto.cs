using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public abstract class UpdateBookDto
    {
        [Required(ErrorMessage = "Title field can not be NULL or empty")]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}
