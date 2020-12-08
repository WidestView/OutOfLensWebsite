using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OutOfLensWebsite.Models.Data
{
    public class Customer
    {
        private static class Keys
        {
            public const string Id = "id";
            public const string Name = "name";
            public const string SocialName = "social_name";
            public const string Gender = "gender";
            public const string Cpf = "cpf";
            public const string Rg = "rg";
            public const string BirthDate = "birth_date";
            public const string Phone = "phone";
            public const string CellPhone = "cellphone";
            public const string Email = "email";
        }
        
        public int Id { get; private set; }
        
        [Required(ErrorMessage = "O nome  é obrigatório")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "O nome social é obrigatório")]
        public string SocialName { get; set; }
        
        [Required(ErrorMessage = "O sexo é obrigatório")]
        public string Gender { get; set; }
        
        [Required(ErrorMessage = "O CPF é obrigatório")]
        public string Cpf { get; set; }
        
        [Required(ErrorMessage = "O RG é obrigatório")]
        public string Rg { get; set; }
        
        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        public DateTime BirthDate { get; set; }
        
        [Required(ErrorMessage = "O telefone é obrigatório")]
        [MinLength(8, ErrorMessage = "Telefone muito curto (tamanho inválido)")]
        public string Phone { get; set; }
        
        [Required(ErrorMessage = "O celular é obrigatório")]
        public string CellPhone { get; set; }
        
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Insira um email válido")]
        public string Email { get; set; }

        public ImmutableTableReference<Customer> Insert(DatabaseConnection database)
        {
            string command = @"
            insert into PESSOA (NOME, NOME_SOCIAL, GENERO, RG, CPF, NASCIMENTO, TELEFONE, CEL, EMAIL)
            values (@name, @social_name, @gender, @rg, @cpf, @birth_date, @phone, @cellphone, @email)";

            database.Run(command, new Dictionary<string, object>
            {
                ["name"] = Name,
                ["social_name"] = SocialName,
                ["gender"] = Gender,
                ["rg"] = Rg,
                ["cpf"] = Cpf,
                ["birth_date"] = BirthDate,
                ["phone"] = Phone,
                ["cellphone"] = CellPhone,
                ["email"] = Email
            });

            int userId = (int) database.GetLastInsertionId();

            command = @"
            insert into CLIENTE (CÓDIGO_USUÁRIO) 
            values (@user_id)";
            
            database.Run(command,  new Dictionary<string, object>
            {
                ["user_id"] = userId
            });

            Id = (int) database.GetLastInsertionId();
            
            return new ImmutableTableReference<Customer>(Id, this);
        }

        public static TableViewModel GetTable(DatabaseConnection connection)
        {
            return new TableViewModel
            {
                Data = connection.Query(@"
                    select 
                           CLIENTE.CÓDIGO, NOME, NOME_SOCIAL, GENERO, RG, CPF, NASCIMENTO, TELEFONE, CEL, EMAIL, ATIVO
                    from CLIENTE inner join PESSOA P on CLIENTE.CÓDIGO_USUÁRIO = P.CÓDIGO
                "),
                Labels = new []
                {
                    "Código", "Nome", "Nome Social", "Gênero", "RG", "CPF", "Data de nascimento",
                    "Telefone", "Celular", "Email", "Ativo"
                }
            };
        }

        public static IEnumerable<SelectListItem> ListItems(DatabaseConnection connection)
        {
            return connection.Query(@"
                select CLIENTE.CÓDIGO as 'id', NOME as 'name'
                from CLIENTE inner join PESSOA P on CLIENTE.CÓDIGO_USUÁRIO = P.CÓDIGO
            ").Select(row => new SelectListItem((string) row["name"], row["id"].ToString()));
        }
    }
    
}