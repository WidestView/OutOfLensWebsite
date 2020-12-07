using System;

namespace OutOfLensWebsite.Models
{
    public class ImmutableTableReference<T> : ITableReference<T> where T : class
    {
        private T _reference;
        private readonly DatabaseConnection _database;
        private readonly ITableReference<T>.ReferenceBuilder _referenceBuilder; 
        
        public int Identifier { get; }

        public T Reference => _reference ??= _referenceBuilder?.Invoke(Identifier, _database);



        public ImmutableTableReference(int id, T reference)
        {
            Identifier = id;

            _reference = reference ?? throw new ArgumentNullException(nameof(reference),
                "If a null TableReference is desired, use null instead.");
            _referenceBuilder = null;
            _database = null;
        }

        public ImmutableTableReference(ITableReference<T>.ReferenceBuilder builder, int id, DatabaseConnection database)
        {
            Identifier = id;
            _referenceBuilder = builder ?? throw new ArgumentNullException(nameof(builder));
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }
    }
}