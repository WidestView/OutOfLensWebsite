using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.Operations;

namespace OutOfLensWebsite.Models
{
    public interface ITableReference<T> where T : class
    {
        [JsonPropertyName("identifier")]
        public int Identifier { get; }
        
        [JsonPropertyName("data")]
        public T Reference { get; }
        
        public delegate T ReferenceBuilder(int id, DatabaseConnection database);
    }
}