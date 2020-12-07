using System.Collections.Generic;

namespace OutOfLensWebsite.Models.Data
{
    public class PackageType
    {
        public int Id { get; private set; }
        
        public string Name { get; set; }
        public string Description { get; set; }

        public static PackageType From(int id, DatabaseConnection connection)
        {
            return null;
        }

        public void Insert(DatabaseConnection connection)
        {
            connection.Run(@"
            insert into TIPO_PACOTE (TIPO_PACOTE, DESCRIÇÃO) 
            values (@name, @description)",
                new Dictionary<string, object>
                {
                    ["name"] = Name,
                    ["Description"] = Description
                });
        }
    }
}