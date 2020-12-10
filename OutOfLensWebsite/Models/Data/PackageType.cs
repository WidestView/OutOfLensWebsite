using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OutOfLensWebsite.Models.Data
{
    public class PackageType
    {
        // [Required(ErrorMessage = "O ~ é obrigatório")]
        // [Required(ErrorMessage = "A ~ é obrigatória")]
        
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "A descrição é obrigatória")]
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