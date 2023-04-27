using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookstoreAPI.Models
{
    public class Author
    {
        [JsonPropertyName("id")]
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
    }

    public class UpdateAuthorEntity
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }
    }
}