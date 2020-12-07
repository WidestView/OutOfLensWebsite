using System;
using System.Collections.Generic;

namespace OutOfLensWebsite.Models.Data
{
    public class Report
    {
        public int Id { get; }
        public TableReference<object> Order { get; set; }
        public TableReference<object> Session { get; set; }
        public TableReference<Employee> Employee { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public void Insert(DatabaseConnection connection)
        {
            connection.Run(@"
                insert into RELATÓRIO (DIA, DESCRIÇÃO, CÓDIGO_PEDIDO, CÓDIGO_SESSÃO, CÓDIGO_FUNCIONÁRIO)
                values (@day, @description, @order_id, @session_id, @employee_id)
                ",
                new Dictionary<string, object>
                {
                    ["day"] = Date,
                    ["description"] = Description,
                    ["order_id"] = Order.Identifier,
                    ["session_id"] = Session.Identifier,
                    ["employee_id"] = Employee.Identifier
                }
            );
        }
    }
}