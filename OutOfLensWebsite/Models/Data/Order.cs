using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OutOfLensWebsite.Models.Data
{
    public class Order
    {
        // [Required(ErrorMessage = "O ~ é obrigatório")]
        // [Required(ErrorMessage = "A ~ é obrigatória")]
        
        [Required(ErrorMessage = "O cliente é obrigatório")]
        public TableReference<Customer> Customer { get; set; }
        
        [Required(ErrorMessage = "O pacote é obrigatório")]
        public TableReference<Package> Package { get; set; }
        [Required(ErrorMessage = "A data é obrigatória")]
        
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

        public static IEnumerable<CalendarOrder> ListAll(DatabaseConnection connection)
        {
            return connection.Query(@"
                select
                    PACOTE.CÓDIGO as 'id',
                       PESSOA.NOME as 'customer_name',
                       PACOTE.DESCRIÇÃO as 'package_name',
                       DIA as 'date'
                    from PEDIDO
                    inner join PACOTE  on PEDIDO.CÓDIGO_PACOTE = PACOTE.CÓDIGO
                    inner join CLIENTE on PEDIDO.CÓDIGO_CLIENTE = CLIENTE.CÓDIGO
                    inner join PESSOA  on CLIENTE.CÓDIGO_USUÁRIO = PESSOA.CÓDIGO
            ").Select(row => new CalendarOrder
            {
                Id = (int) row["id"],
                CustomerName = (string) row["customer_name"],
                PackageName = (string) row["package_name"],
                Date = (DateTime) row["date"]
            });
        }

        public class CalendarOrder
        {
            public int Id { get; set; }
            public string PackageName { get; set; }
            public string CustomerName { get; set; }
            public DateTime Date { get; set; }
        }
    }
}