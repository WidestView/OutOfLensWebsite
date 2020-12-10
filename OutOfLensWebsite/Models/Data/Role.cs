using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OutOfLensWebsite.Models.Data
{
    public class Role
    {
        
        // [Required(ErrorMessage = "O ~ é obrigatório")]
        // [Required(ErrorMessage = "A ~ é obrigatória")]
        
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Name { get; set; }
        [Required(ErrorMessage = "A descrição é obrigatória")]
        public string Description { get; set; }
        
        public void Insert(DatabaseConnection connection)
        {
            connection.Run(@"
                insert into CARGO (NOME, DESCRIÇÃO) values (@name, @description)
            ", new Dictionary<string, object>
            {
                ["name"] = Name,
                ["description"] = Description
            });
        }

        public static TableViewModel GetTable(DatabaseConnection connection)
        {
            return new TableViewModel
            {
                Data = connection.Query(@"
                    select CÓDIGO, NOME, DESCRIÇÃO from CARGO
                "),
                Labels = new []{"Código", "Nome", "Descrição"}
            };
        }
    }
}