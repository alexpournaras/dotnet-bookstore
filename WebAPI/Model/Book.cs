using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPI.Model
{
    public class Book
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "The date field is required.")]
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [Required(ErrorMessage = "The title field is required.")]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The category field is required.")]
        [JsonPropertyName("category")]
        public string Category { get; set; }

        [Required(ErrorMessage = "The pages field is required.")]
        [JsonPropertyName("pages")]
        public int? Pages { get; set; }

        [Required(ErrorMessage = "The author_id field is required.")]
        [JsonPropertyName("author_id")]
        public int? AuthorId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Author Author { get; set; }
    }

}