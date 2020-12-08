using System.Collections.Generic;

namespace OutOfLensWebsite.Models.Data
{
    public class Role
    {
        public string Name { get; set; }
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