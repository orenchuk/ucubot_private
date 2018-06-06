using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ucubot.Model;
using ucubot.Repository;

namespace ucubot.Controllers
{
    [Route("api/[controller]")]
    public class StudentSignalsEndpointController : Controller
    {
        private readonly IStudentSignalsRepository studentSignalsRepository;

        public StudentSignalsEndpointController(IStudentSignalsRepository studentSignalsRepository)
        {
            this.studentSignalsRepository = studentSignalsRepository;
        }

        [HttpGet]
        public IEnumerable<StudentSignal> ShowSignals()
        {
            return studentSignalsRepository.GetAll();
        }
    }
}