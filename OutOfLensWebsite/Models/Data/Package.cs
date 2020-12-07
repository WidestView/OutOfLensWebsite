using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OutOfLensWebsite.Models.Data
{
    public class Package
    {
        public int Type { get; set; }
        public string Quality { get; set; }
        public int Quantity { get; set; }
        public int PhotoHeight { get; set; }
        public int PhotoWidth { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Observation { get; set; }

        public static IEnumerable<SelectListItem> GetTypes(DatabaseConnection database)
        {
            const string typeId = "type_id";
            const string typeName = "type_name";
            
            string command = $"select CÓDIGO as '{typeId}', TIPO_PACOTE as '{typeName} from TIPO_PACOTE where DISPONÍVEL";
            

            return database
                .Query(command)
                .Select(row => 
                    new SelectListItem((string) row[typeName], row[typeId].ToString()));

        }

        public void Insert(DatabaseConnection connection)
        {
            connection.Run(@"
                insert into PACOTE (CÓDIGO_TIPO_PACOTE, VALOR, QUALIDADE, QUANTIDADE, TAMANHO_A, TAMANHO_L,
                                    DISPONÍVEL, OBSERVAÇÃO)
                values (@type_id, @price, @quality, @quantity, @height, @width, @available, @observation)",
                new Dictionary<string, object>
                {
                    ["type_id"] = Type,
                    ["price"] = Price,
                    ["quality"] = Quality,
                    ["quantity"] = Quantity,
                    ["height"] = PhotoHeight,
                    ["width"] = PhotoWidth,
                    ["available"] = true,
                    ["observation"] = Observation,
                });
        }
        
        
        
    }

}
