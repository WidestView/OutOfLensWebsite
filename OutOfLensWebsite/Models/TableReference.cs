using System;

namespace OutOfLens_ASP.Models
{
    public class TableReference<T> where T : class
    {
        private T _reference;
        private readonly DatabaseConnection _database;
        private readonly ReferenceBuilder _referenceBuilder; 
        
        public int Identifier { get; }

        public T Reference => _reference ??= _referenceBuilder?.Invoke(Identifier, _database);
                


        public delegate T ReferenceBuilder(int id, DatabaseConnection database);

        public TableReference(int id, T reference)
        {
            Identifier = id;

            _reference = reference ?? throw new ArgumentNullException(nameof(reference),
                "If a null TableReference is desired, use null instead.");
            _referenceBuilder = null;
            _database = null;
        }

        public TableReference(ReferenceBuilder builder, int id, DatabaseConnection database)
        {
            Identifier = id;
            _referenceBuilder = builder ?? throw new ArgumentNullException(nameof(builder));
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }
    }
}