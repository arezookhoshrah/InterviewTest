
using System.ComponentModel.DataAnnotations;
using Test.Domain.Common;

namespace Test.Domain.Entity
{
    public class MemberInfo:EntityBase
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Family { get; set; }
        [Required]
        public string NationalCode { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
    }
}
