using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPI.Model
{
    public class Author
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The first_name field is required.")]

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The last_name field is required.")]

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The country field is required.")]

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("books")]
        public int Books { get; set; }
    }

}