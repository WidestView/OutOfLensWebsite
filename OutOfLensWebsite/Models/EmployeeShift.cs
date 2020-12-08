using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using MySql.Data.MySqlClient;
using OutOfLensWebsite.Models;
using OutOfLensWebsite.Models.Data;

namespace OutOfLens_ASP.Models
{
    public class EmployeeShift
    {
        [JsonPropertyName("employee")]
        public ImmutableTableReference<Employee> Employee { get; set; }

        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }
        
        [JsonPropertyName("end_time")]
        public DateTime? EndTime { get; set; }

        public static EmployeeShift From(ArduinoLogRequest request, DatabaseConnection database)
        {
            EmployeeShift shift = new EmployeeShift
            {
                Employee = OutOfLensWebsite.Models.Data.Employee.FromRfid(request.RfidData(), database)
            };

            if (shift.Employee == null)
            {
                throw new KeyNotFoundException("Employee with request's rfid was not Found");
            }
                

            return shift;
        }

        public void RegisterUsing(DatabaseConnection database)
        {
            var parameters = new Dictionary<string, object>
            {
                ["employee"] = Employee.Identifier
            };

            try
            {
                int? openShiftId = (int?) database.Run(
                    "select CÓDIGO from TURNO where CÓDIGO_FUNCIONÁRIO = @employee and HORÁRIO_SAÍDA IS NULL limit 1"
                    , parameters);

                if (openShiftId == null)
                {
                    database.Run(
                        "insert into TURNO(CÓDIGO_FUNCIONÁRIO, HORÁRIO_ENTRADA, HORÁRIO_SAÍDA) values (@employee, now(), NULL)",
                        parameters);
                }
                else
                {
                    parameters["id"] = openShiftId;

                    database.Run( "update TURNO set HORÁRIO_SAÍDA = now() where CÓDIGO_FUNCIONÁRIO = @employee and CÓDIGO = @id",
                        parameters);
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static IEnumerable<Dictionary<string, object>> GetTable(DatabaseConnection connection)
        {
            return connection.Query(@"
                select TURNO.CÓDIGO, NOME, HORÁRIO_ENTRADA, HORÁRIO_SAÍDA  from TURNO
                    inner join FUNCIONÁRIO on TURNO.CÓDIGO_FUNCIONÁRIO = FUNCIONÁRIO.CÓDIGO
                    inner join PESSOA on FUNCIONÁRIO.CÓDIGO_USUÁRIO = PESSOA.CÓDIGO"
                );
            
        }

        public static EmployeeShift GetLast(DatabaseConnection connection)
        {
            var result = connection.Query(@"
            select CÓDIGO_FUNCIONÁRIO as 'employee_id', 
                   HORÁRIO_SAÍDA as 'exit_time' from UltimoTurno");

            if (result.Count == 0)
            {
                return null;
            }
            else
            {
                var row = result.First();
                
                int id = (int) row["employee_id"];
                    
                object exit = row["exit_time"];
                    
                return new EmployeeShift
                {
                    
                    Employee = new ImmutableTableReference<Employee>(OutOfLensWebsite.Models.Data.Employee.From, id, connection),
                    EndTime = exit == DBNull.Value ? null : (DateTime?) exit
                };
            }
        }
    
    }
    
}