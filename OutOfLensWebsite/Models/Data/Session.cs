using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OutOfLensWebsite.Models.Data
{
    public class Session
    {
        public int Id { get; }
        public TableReference<Order> Order;
        public string Address;
        public DateTime StartTime;
        public DateTime EndTime;
        public string Description;

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
    }
}