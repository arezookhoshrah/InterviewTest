
using System.ComponentModel.DataAnnotations;

namespace Test.Domain.Common
{
    public class EntityBase
    {
        [Key]
        [Required]
        public int Id { get; set; }
    }
}
