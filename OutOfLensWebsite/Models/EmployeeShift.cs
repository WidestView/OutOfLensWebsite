using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace OutOfLens_ASP.Models
{
    public class EmployeeShift
    {
        public TableReference<Employee> Employee;

        public static EmployeeShift From(ArduinoLogRequest request, DatabaseConnection database)
        {
            EmployeeShift shift = new EmployeeShift
            {
                Employee = Models.Employee.FromRfid(request.RfidData(), database)
            };

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

                    database.Run( "update TURNO set HORÁRIO_SAÍDA = now() where CÓDIGO_FUNCIONÁRIO = @employee and CÓDIGO = @id");
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}