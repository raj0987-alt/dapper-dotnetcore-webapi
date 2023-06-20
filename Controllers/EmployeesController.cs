﻿using Dapper;
using DapperCrudWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCrudWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase , IDisposable
    {
        private readonly IConfiguration _config;
        private readonly SqlConnection connection;

        public EmployeesController(IConfiguration config)
        {
            _config = config;
            connection = new SqlConnection(_config.GetConnectionString("DBConnection"));
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAllEmployees()
        {
            IEnumerable<Employee> employees = await SelectAllEmployees();
            return Ok(employees);
        }

        private async Task<IEnumerable<Employee>> SelectAllEmployees()
        {
            return await connection.QueryAsync<Employee>("SELECT * FROM Employees");
        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<Employee>> GetEmployee(int employeeId)
        {
            var employee = await connection.QueryFirstOrDefaultAsync<Employee>("SELECT * FROM Employees WHERE Id = @Id",
                new { Id = employeeId });

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            await connection.ExecuteAsync("INSERT INTO Employees (Name, FirstName, LastName, Email) VALUES (@Name, @FirstName, @LastName, @Email)",
                employee);

            return Ok(await SelectAllEmployees());
        }
        

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}