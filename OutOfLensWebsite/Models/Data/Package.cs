using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OutOfLensWebsite.Models.Data
{
    public class Package
    {
        [Required(ErrorMessage = "O tipo é obrigatório")]
        public int Type { get; set; }

        [Required(ErrorMessage = "A qualidade é obrigatória")]
        public string Quality { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "A altura é obrigatória")]
        public int PhotoHeight { get; set; }

        [Required(ErrorMessage = "A largura é obrigatória")]
        public int PhotoWidth { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        public string Description { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório")]
        public string Price { get; set; }


        public void Insert(DatabaseConnection connection)
        {
            connection.Run(@"
                insert into PACOTE (CÓDIGO_TIPO_PACOTE, VALOR, QUALIDADE, QUANTIDADE, TAMANHO_A, TAMANHO_L,
                                    DISPONÍVEL, DESCRIÇÃO)
                values (@type_id, @price, @quality, @quantity, @height, @width, @available, @description)",
                new Dictionary<string, object>
                {
                    ["type_id"] = Type,
                    ["price"] = Price,
                    ["quality"] = Quality,
                    ["quantity"] = Quantity,
                    ["height"] = PhotoHeight,
                    ["width"] = PhotoWidth,
                    ["available"] = true,
                    ["description"] = Description,
                });
        }


        public static TableViewModel GetTable(DatabaseConnection connection)
        {
            return new TableViewModel
            {
                Data = connection.Query(@"
                    select 
                           PACOTE.CÓDIGO, VALOR, QUALIDADE, QUANTIDADE, TAMANHO_A, TAMANHO_L, PACOTE.DISPONÍVEL, 
                           TP.TIPO_PACOTE
                    from PACOTE inner join TIPO_PACOTE TP on PACOTE.CÓDIGO_TIPO_PACOTE = TP.CÓDIGO
                "),
                Labels = new[]
                {
                    "Código", "Valor", "Qualidade", "Quantidade", "Altura", "Largura", "Disponível",
                    "Tipo de Pacote",
                }
            };
        }

        public static IEnumerable<SelectListItem> ListItems(DatabaseConnection connection)
        {
            return connection.Query(@"
                select CÓDIGO as 'id', DESCRIÇÃO as 'name' from PACOTE
            ").Select(
                row => new SelectListItem((string) row["name"], row["id"].ToString()));
        }
    }
}