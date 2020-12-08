using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OutOfLensWebsite.Models.Data
{
    public class Order
    {
        public int Id { get; }
        public TableReference<Customer> Customer { get; set; }
        public TableReference<Package> Package { get; set; }
        public DateTime Date { get; set; }
        public bool Done;

        public void Insert(DatabaseConnection connection)
        {
            connection.Run(@"
                insert into PEDIDO (DIA, CÓDIGO_CLIENTE, ENTREGUE, CÓDIGO_PACOTE) 
                values (@date, @client_id, @done, @package_type)
            ",
                new Dictionary<string, object>
                {
                    ["date"] = Date,
                    ["client_id"] = Customer.Identifier,
                    ["done"] = Done,
                    ["package_type"] = Package.Identifier
                });
        }

        public static IEnumerable<SelectListItem> ListItems(DatabaseConnection connection)
        {
            return connection.Query(@" select CÓDIGO as 'id' from PEDIDO")
                .Select(row => 
                    new SelectListItem(row["id"].ToString(), row["id"].ToString()));
        }

        public static TableViewModel GetTable(DatabaseConnection connection)
        {
            return new TableViewModel
            {
                Data = connection.Query(@"select CÓDIGO, DIA, CÓDIGO_CLIENTE, ENTREGUE, CÓDIGO_PACOTE from PEDIDO"),
                Labels = new []{"Código", "Dia", "Código do Cliente", "Entregue", "Código do Pacote"}
            };
        }
    }
}