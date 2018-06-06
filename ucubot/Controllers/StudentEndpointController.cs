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
        private readonly IStudentRepository studentRepository;

        public StudentEndpointController(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }
        
        [HttpGet]
        public IEnumerable<Student> ShowStudents()
        {
            return studentRepository.GetAll();
        }
        
        [HttpGet("{id}")]
        public Student ShowStudent(long id)
        {
            return studentRepository.GetById(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            var wasCreated = studentRepository.Insert(student);

            if (!wasCreated)
            {
                return StatusCode(409); 
            }

            return Accepted();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudent(Student student)
        {
            studentRepository.Update(student);

            return Accepted();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveStudent(long id)
        {
            var wasCreated = studentRepository.RemoveById(id);

            if (!wasCreated)
            {
                return StatusCode(409);
            }

            return Accepted();
        }
        
    }
}