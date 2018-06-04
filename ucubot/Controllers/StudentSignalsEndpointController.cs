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
        private readonly IConfiguration _configuration;
        private readonly IStudentSignalsRepository _studentSignalsRepository;
        private string connectionString;

        public StudentSignalsEndpointController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("BotDatabase");
            _studentSignalsRepository = new StudentSignalsRepository(connectionString);
        }

        [HttpGet]
        public IEnumerable<StudentSignal> ShowSignals()
        {
            return _studentSignalsRepository.GetAll();
        }
    }
}