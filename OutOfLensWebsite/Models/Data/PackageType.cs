using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        
        public static IEnumerable<SelectListItem> ListItems(DatabaseConnection database)
        {
            const string typeId = "type_id";
            const string typeName = "type_name";
            
            string command = $"select CÓDIGO as '{typeId}', TIPO_PACOTE as '{typeName}' from TIPO_PACOTE where DISPONÍVEL";
            

            return database
                .Query(command)
                .Select(row => 
                    new SelectListItem((string) row[typeName], row[typeId].ToString()));

        }
    }
}