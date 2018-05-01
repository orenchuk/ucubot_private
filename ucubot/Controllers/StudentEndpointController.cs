using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ucubot.Model;
using Dapper;

namespace ucubot.Controllers
{
    [Route("api/[controller]")]
    public class StudentEndpointController : Controller
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public StudentEndpointController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("BotDatabase");
        }
        
        [HttpGet]
        public IEnumerable<Student> ShowStudents()
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                const string query = "SELECT student.FirstName as FirstName, student.Id as Id, student.LastName as LastName, student.UserId as UserId FROM student;";
                return conn.Query<Student>(query).ToList();
            }
        }
        
        [HttpGet("{id}")]
        public Student ShowStudent(long id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                const string query = "SELECT student.FirstName as FirstName, student.Id as Id, student.LastName as LastName, student.UserId as UserId FROM student WHERE id = @ID;";
                var signals = conn.Query<Student>(query, new {ID = id}).ToList();
                return signals.Any() ? signals.First() : null; 
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            var firstName = student.FirstName;
            var lastName = student.LastName;
            var userId = student.UserId;

            using (var conn = new MySqlConnection(connectionString))
            {
                const string selectQuery = "SELECT student.FirstName as FirstName, student.Id as Id, student.LastName as LastName, student.UserId as UserId FROM student WHERE UserId = @id;";
                var createdStudents = conn.Query<Student>(selectQuery, new {id = student.Id}).ToList();
                
                if (createdStudents.Any())
                {
                    return StatusCode(409);
                }

                const string insertQuery = "INSERT INTO student (FirstName, LastName, UserId) VALUES (@first_name, @last_name, @user_id);";
                conn.Query<Student>(insertQuery, new {first_name = firstName, last_name = lastName, user_id = userId});
            }

            return Accepted();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudent(Student student)
        {
            var FirstName = student.FirstName;
            var LastName = student.LastName;
            var UserId = student.UserId;

            using (var conn = new MySqlConnection(connectionString))
            {
                const string query = "UPDATE student SET FirstName = @firstName, LastName = @lastName, UserId = @userid WHERE Id = @id;";
                conn.Query<Student>(query, new {firstName = FirstName, lastName = LastName, userid = UserId});
            }

            return Accepted();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveStudent(long id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                const string selectQuery = "SELECT * FROM lesson_signal WHERE student_id = @ID;";
                var signals = conn.Query<LessonSignalDto>(selectQuery, new {ID = id}).ToList();
                if (signals.Any())
                {
                    return StatusCode(409);
                }

                const string deleteQuery = "DELETE FROM student WHERE id = @ID;";
                conn.Query<Student>(deleteQuery, new {ID = id});
            }

            return Accepted();
        }
        
    }
}