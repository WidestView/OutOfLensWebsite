using Microsoft.CodeAnalysis.Operations;

namespace OutOfLensWebsite.Models
{
    public interface ITableReference<T> where T : class
    {
        public int Identifier { get; }
        public T Reference { get; }
        
        public delegate T ReferenceBuilder(int id, DatabaseConnection database);
    }
}