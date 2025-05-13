using System.ComponentModel.DataAnnotations.Schema;
using WebApplication7.Models.Base;

namespace WebApplication7.Models
{
    public class Member:BaseEntity
    {
        public string FullName { get; set; }

        public string Job {  get; set; }

        public string? ImgUrl { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
}

