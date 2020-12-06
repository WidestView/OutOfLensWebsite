using System;
using System.Collections.Generic;

namespace OutOfLens_ASP.Models
{
    public class Employee
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string SocialName { get; set; }
        public string Gender { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string Rfid { get; set; }
        public int AccessLevel { get; set; }

        public Employee()
        {
        }

        public Employee(Dictionary<string, object> source)
        {
            Id = (int) source["id"];
            Name = (string) source["name"];
            SocialName = (string) source["social_name"];
            Gender = (string) source["gender"];
            Rg = (string) source["rg"];
            Cpf = (string) source["cpf"];
            BirthDate = (DateTime) source["birth_date"];
            Phone = (string) source["phone"];
            Cellphone = (string) source["cell_phone"];
            Email = (string) source["email"];
            Password = (string) source["password"];
            IsActive = (bool) source["is_active"];
            Rfid = (string) source["rfid"];
            AccessLevel = (int) source["access_level"];
        }

        public static Employee From(int id, DatabaseConnection database)
        {
            
            string command = @"select
            FUNCIONÁRIO.CÓDIGO as 'id',
            NOME as 'name',
            NOME_SOCIAL as 'social_name',
            GENERO as 'gender',
            RG as 'rg',
            CPF as 'cpf',
            NASCIMENTO as 'birth_date',
            TELEFONE as 'phone',
            CEL as 'cell_phone',
            EMAIL as 'email',
            SENHA as 'password',
            ATIVO as 'is_active',
            RFID as 'rfid',
            NÍVEL_ACESSO as 'access_level'
            from FUNCIONÁRIO inner join USUÁRIO U on FUNCIONÁRIO.CÓDIGO_USUÁRIO = U.CÓDIGO
            where FUNCIONÁRIO.CÓDIGO = @id limit 1";

            var result = database.Query(command, new Dictionary<string, object>
            {
                ["id"] = id
            });

            if (result.Count == 0)
            {
                throw new KeyNotFoundException($"ID {id} was not found.");
            }
            
            return new Employee(result[0]);
        }

        public static TableReference<Employee> FromRfid(string rfid, DatabaseConnection database)
        {
            string query =
                "select FUNCIONÁRIO.CÓDIGO from FUNCIONÁRIO inner join USUÁRIO U on FUNCIONÁRIO.CÓDIGO_USUÁRIO = U.CÓDIGO where RFID = @rfid";

            int? id = (int?) database.Run(query, new Dictionary<string, object>
            {
                ["rfid"] = rfid
            });

            if (id != null)
            {
                return new TableReference<Employee>(From, (int) id, database);
            }

            return null;
        }

        public TableReference<Employee> Register(DatabaseConnection database)
        {
            string command = @"insert into USUÁRIO 
            (NOME, NOME_SOCIAL, GENERO, RG, CPF, NASCIMENTO, TELEFONE, CEL, EMAIL, SENHA)
            values 
            (@name, @social_name, @gender, @rg, @cpf, @birth_date, @phone, @cellphone, @email, @password)";


            database.Run(command, new Dictionary<string, object>
            {
                ["name"] = Name,
                ["social_name"] = SocialName,
                ["gender"] = Gender,
                ["rg"] = Rg,
                ["cpf"] = Cpf,
                ["birth_date"] = BirthDate,
                ["phone"] = Phone,
                ["cellphone"] = Cellphone,
                ["email"] = Email,
                ["password"] = Password
            });

            int userId = (int) database.GetLastInsertionId();

            command = @"insert into FUNCIONÁRIO
            (CÓDIGO_USUÁRIO, NÍVEL_ACESSO, RFID)
            values 
           (@user_id, @access_level, @rfid)";

            database.Run(command, new Dictionary<string, object>
            {
                ["user_id"] = userId,
                ["access_level"] = AccessLevel,
                ["rfid"] = Rfid
            });
            
            return new TableReference<Employee>((int) database.GetLastInsertionId(), this);
        }

        public static IEnumerable<Employee> ListFrom(DatabaseConnection database)
        {
            string command = @"select
            FUNCIONÁRIO.CÓDIGO as 'id',
            NOME as 'name',
            NOME_SOCIAL as 'social_name',
            GENERO as 'gender',
            RG as 'rg',
            CPF as 'cpf',
            NASCIMENTO as 'birth_date',
            TELEFONE as 'phone',
            CEL as 'cell_phone',
            EMAIL as 'email',
            SENHA as 'password',
            ATIVO as 'is_active',
            RFID as 'rfid',
            NÍVEL_ACESSO as 'access_level'
            from FUNCIONÁRIO inner join USUÁRIO U on FUNCIONÁRIO.CÓDIGO_USUÁRIO = U.CÓDIGO";
            
            var result = database.Query(command);
            
            List<Employee> employees = new List<Employee>();
            
            foreach (var row in result)
            {
                employees.Add(new Employee(row));
            }

            return employees;
        }
    }
}