using System;
using System.Collections.Generic;

namespace OutOfLensWebsite.Models
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
        public string Name { get; set; }
        public string SocialName { get; set; }
        public string Gender { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }

        public TableReference<Customer> Insert(DatabaseConnection database)
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
            
            return new TableReference<Customer>(Id, this);
        }
    }
}