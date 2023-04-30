using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookstoreAPI.Models
{
    public class Job
    {
        public Guid Id { get; set; }

        public string Status { get; set; }

        public List<UpdateBookEntity> Books { get; set; }
    }
}