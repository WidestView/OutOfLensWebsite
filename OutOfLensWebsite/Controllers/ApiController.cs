using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using OutOfLens_ASP.Models;
using OutOfLensWebsite.Models;

namespace OutOfLensWebsite.Controllers
{
    public class ApiController : Controller
    {
        [HttpPost]
        public async Task<string> LogData()
        {
            try
            {
                using StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);

                string body = await reader.ReadToEndAsync();


                ArduinoLogRequest request = JsonSerializer.Deserialize<ArduinoLogRequest>(body);
                
                try
                {
                    if (request.Valid)
                    {
                        try
                        {
                            DatabaseConnection database = new DatabaseConnection();


                            EmployeeShift.From(request, database).RegisterUsing(database);

                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine(ex);

                            return Messages.Failure("Invalid operation");
                        }
                        catch (KeyNotFoundException)
                        {
                            Console.WriteLine("RFID not found");

                            return Messages.Failure("RFID not found");
                        }
                    }
                    else
                    {
                        return Messages.Failure("Invalid credentials");
                    }
                }
                catch (Exception ex) when (ex is OutOfMemoryException || ex is MySqlException)
                {
                    Console.WriteLine(ex);
                    return Messages.Failure();
                }

                // LOG DATA

                return Messages.Success();
            }
            catch
            {
                return Messages.Failure("Internal server error");
            }
        }

        [HttpGet]
        public string LastShift()
        {
            var connection = new DatabaseConnection();

            var shift = EmployeeShift.GetLast(connection);


            return JsonSerializer.Serialize(shift != null ? new
            {
                employee_name = shift.Employee.Reference.Name,
                is_join = shift.EndTime == null
            } : new object());
        }
    }
}