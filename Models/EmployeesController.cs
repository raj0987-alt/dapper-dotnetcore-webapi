using ASP.NETCore6WebAPICRUDWithEntityFramework_CodeFirstApproach_.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCrudWebAPI.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        public readonly IConfiguration _config;
        public EmployeesController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAllEmployees()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DBConnection"));
            IEnumerable<Employee> employees = await SelectAllEmployees(connection);
            return Ok(employees);
        }

        private static async Task<IEnumerable<Employee>> SelectAllEmployees(SqlConnection connection)
        {
            return await connection.QueryAsync<Employee>("SELECT * FROM Employees");
        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<List<Employee>>> GetEmployee(int employeeId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DBConnection"));
            var employee = await connection.QueryFirstAsync<Employee>("SELECT * FROM Employees WHERE Id = @Id",
                new {Id = employeeId});
            return Ok(employee);
        }
    }
}
