using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OutOfLensWebsite.Models.Data
{
    public class Report
    {
        // [Required(ErrorMessage = "O ~ é obrigatório")]
        // [Required(ErrorMessage = "A ~ é obrigatória")]
        
        public TableReference<object> Order { get; set; }
        
        [Required(ErrorMessage = "A sessão é obrigatória")]
        public TableReference<object> Session { get; set; }
        
        [Required(ErrorMessage = "O funcionário é obrigatório")]
        public TableReference<Employee> Employee { get; set; }
        
        [Required(ErrorMessage = "A data é obrigatória")]
        public DateTime Date { get; set; }
        
        [Required(ErrorMessage = "A descrição é obrigatória")]
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

        public static TableViewModel GetTale(DatabaseConnection connection)
        {
            return new TableViewModel
            {
                Data = connection.Query(@"
                    select RELATÓRIO.CÓDIGO, DIA, DESCRIÇÃO, CÓDIGO_PEDIDO, CÓDIGO_SESSÃO, P.NOME from RELATÓRIO
                    inner join FUNCIONÁRIO F on RELATÓRIO.CÓDIGO_FUNCIONÁRIO = F.CÓDIGO
                    inner join PESSOA P on F.CÓDIGO_USUÁRIO = P.CÓDIGO
                    "),
                Labels = new [] { "Código", "Data", "Descrição", "Código do Pedido", "Código da Sessão", "Nome do funcionário"}
            };
        }
    }
}