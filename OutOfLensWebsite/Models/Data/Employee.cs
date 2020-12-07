using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OutOfLensWebsite.Models.Data
{
    public class Employee
    {
        public int Id { get; private set; }
        
        [Required(ErrorMessage = "O nome é obrigatório")]
        
        public string Name { get; set; }
        
        [Required(ErrorMessage = "O nome social é obrigatório")]
        public string SocialName { get; set; }
        
        [Required(ErrorMessage = "O nome social é obrigatório")]
        public string Gender { get; set; }
        
        [Required(ErrorMessage = "O RG é obrigatório")]
        public string Rg { get; set; }
        
        [Required(ErrorMessage = "O CPF é obrigatório")]
        [MinLength(11, ErrorMessage = "O CPF deve ter 11 caracteres")]
        [MaxLength(11, ErrorMessage = "O CPF deve ter 11 caracteres")]
        [RegularExpression("^\\d{11}$", ErrorMessage = "CPF inválido")]
        public string Cpf { get; set; }
        
        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        public DateTime BirthDate { get; set; }
        
        [Required(ErrorMessage = "O telefone é obrigatório")]
        [RegularExpression("^\\d+$", ErrorMessage = "O celular aceita apenas caracteres numéricos")]
        public string Phone { get; set; }
        
        [Required(ErrorMessage = "O celular é obrigatório")]
        [RegularExpression("^\\d+$", ErrorMessage = "O celular aceita apenas caracteres numéricos")]
        public string Cellphone { get; set; }
        
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "A senha é obrigatória")]
        [MinLength(8, ErrorMessage = "A senha precisa de pelo menos 8 caracteres")]
        public string Password { get; set; }
        
        public bool IsActive { get; set; }
        
        [Required(ErrorMessage = "O RFID é obrigatório")]
        public string Rfid { get; set; }
        
        [Required(ErrorMessage = "O nivel de acesso é obrigatório")]
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
            from FUNCIONÁRIO inner join PESSOA P on FUNCIONÁRIO.CÓDIGO_USUÁRIO = P.CÓDIGO
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

        public static ImmutableTableReference<Employee> FromRfid(string rfid, DatabaseConnection database)
        {
            string query =
                "select FUNCIONÁRIO.CÓDIGO from FUNCIONÁRIO inner join PESSOA P on FUNCIONÁRIO.CÓDIGO_USUÁRIO = P.CÓDIGO where RFID = @rfid";

            int? id = (int?) database.Run(query, new Dictionary<string, object>
            {
                ["rfid"] = rfid
            });

            if (id != null)
            {
                return new ImmutableTableReference<Employee>(From, (int) id, database);
            }

            return null;
        }

        /// <summary>
        /// Registers the employee in the database.
        /// </summary>
        /// <param name="database">The database connection in which to store the data</param>
        /// <returns>A table reference to the newly inserted Employee.</returns>
        /// <remarks>The Id property is updated to match the inserted one, which equals the Identifier
        /// of the resulting TableReference</remarks>
        public ImmutableTableReference<Employee> Register(DatabaseConnection database)
        {
            string command = @"insert into PESSOA 
            (NOME, NOME_SOCIAL, GENERO, RG, CPF, NASCIMENTO, TELEFONE, CEL, EMAIL)
            values 
            (@name, @social_name, @gender, @rg, @cpf, @birth_date, @phone, @cellphone, @email)";


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
            (CÓDIGO_USUÁRIO, NÍVEL_ACESSO, RFID, SENHA)
            values 
           (@user_id, @access_level, @rfid, @password)";

            database.Run(command, new Dictionary<string, object>
            {
                ["user_id"] = userId,
                ["access_level"] = AccessLevel,
                ["rfid"] = Rfid,
                ["password"] = Password
            });

            Id = (int) database.GetLastInsertionId();
            
            return new ImmutableTableReference<Employee>(Id, this);
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
            from FUNCIONÁRIO inner join PESSOA P on FUNCIONÁRIO.CÓDIGO_USUÁRIO = P.CÓDIGO";
            
            var result = database.Query(command);
            
            List<Employee> employees = new List<Employee>();
            
            foreach (var row in result)
            {
                employees.Add(new Employee(row));
            }

            return employees;
        }

        public static ImmutableTableReference<Employee> Login(string email, string password, DatabaseConnection database)
        {
            string command = @"
            select FUNCIONÁRIO.CÓDIGO from FUNCIONÁRIO
                inner join PESSOA P on FUNCIONÁRIO.CÓDIGO_USUÁRIO = P.CÓDIGO
            where EMAIL = @email and SENHA = @password
            ";

            object result = database.Run(command, new Dictionary<string, object>
            {
                ["email"] = email,
                ["password"] = password
            });

            if (result == null)
            {
                return null;
            }

            int id = (int) result;
            
            return new ImmutableTableReference<Employee>(From, id, database);
        }

        public static IEnumerable<SelectListItem> ListItems(DatabaseConnection connection)
        {
            return connection.Query(@"
                select NOME as 'name', FUNCIONÁRIO.CÓDIGO as 'id'
                from FUNCIONÁRIO
                    inner join PESSOA on FUNCIONÁRIO.CÓDIGO_USUÁRIO = PESSOA.CÓDIGO").Select(
                
                row =>  new SelectListItem((string) row["name"], row["id"].ToString())
            );
        }
    }
}