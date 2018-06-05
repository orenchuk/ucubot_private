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
using ucubot.Repository;

namespace ucubot.Controllers
{
    [Route("api/[controller]")]
    public class StudentEndpointController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public StudentEndpointController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("BotDatabase");
            _studentRepository = new StudentRepository(configuration);
        }
        
        [HttpGet]
        public IEnumerable<Student> ShowStudents()
        {
            return _studentRepository.GetAll();
        }
        
        [HttpGet("{id}")]
        public Student ShowStudent(long id)
        {
            return _studentRepository.GetById(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            var wasCreated = _studentRepository.Insert(student);

            if (!wasCreated)
            {
                return StatusCode(409); 
            }

            return Accepted();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudent(Student student)
        {
            _studentRepository.Update(student);

            return Accepted();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveStudent(long id)
        {
            var wasCreated = _studentRepository.RemoveById(id);

            if (!wasCreated)
            {
                return StatusCode(409);
            }

            return Accepted();
        }
        
    }
}