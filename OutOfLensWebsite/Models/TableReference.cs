using System;

namespace OutOfLensWebsite.Models
{
    public class TableReference<T> : ITableReference<T> where T : class
    {
        public DatabaseConnection Connection { get; set; }
        public ITableReference<T>.ReferenceBuilder Builder { get; set; }
        
        public int Identifier { get; set;  }
        
        private T _reference;

        public T Reference
        {
            get => _reference ??= Builder?.Invoke(Identifier, Connection);

            set => _reference = value;
        }



        public TableReference(int id, T reference)
        {
            Identifier = id;

            _reference = reference ?? throw new ArgumentNullException(nameof(reference),
                "If a null TableReference is desired, use null instead.");
        }

        public TableReference(ITableReference<T>.ReferenceBuilder builder, int id, DatabaseConnection database)
        {
            Identifier = id;
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            Connection = database ?? throw new ArgumentNullException(nameof(database));
        }
    }
}