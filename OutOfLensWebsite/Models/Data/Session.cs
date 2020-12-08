using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OutOfLensWebsite.Models.Data
{
    public class Session
    {
        public TableReference<Order> Order { get; set; }
        public string Address { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }

        public void Insert(DatabaseConnection connection)
        {
            connection.Run(@"
                insert into SESSÃO (ENDEREÇO, HORARIO_INÍCIO, HORARIO_FINALIZAÇÃO, CÓDIGO_PEDIDO, DESCRIÇÃO) 
                values (@address, @start_time, @end_time, @order_id, @description)
            ",
                new Dictionary<string, object>
                {
                    ["address"] = Address,
                    ["start_time"] = StartTime,
                    ["end_time"] = EndTime,
                    ["order_id"] = Order.Identifier,
                    ["description"] = Description
                });
        }

        public static IEnumerable<SelectListItem> ListItems(DatabaseConnection connection)
        {
            return connection.Query("select CÓDIGO as 'id' from SESSÃO").Select(
                row => new SelectListItem(row["id"].ToString(), row["id"].ToString())
            );
        }
        
        public static TableViewModel GetTable(DatabaseConnection connection)
        {
            return new TableViewModel
            {
                Data = connection.Query(@"
                    select CÓDIGO, ENDEREÇO, HORARIO_INÍCIO, HORARIO_FINALIZAÇÃO, CÓDIGO_PEDIDO, DESCRIÇÃO
                    from SESSÃO"
                ),
                Labels = new []
                {
                    "Código", "Endereço", "Horário de Inicio", "Horario de Fim", "Código do Pedido",
                    "Descrição"
                }
            };
        }
    }
}