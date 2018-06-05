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
        private readonly IStudentSignalsRepository _studentSignalsRepository;

        public StudentSignalsEndpointController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("BotDatabase");
            _studentSignalsRepository = new StudentSignalsRepository(configuration);
        }

        [HttpGet]
        public IEnumerable<StudentSignal> ShowSignals()
        {
            return _studentSignalsRepository.GetAll();
        }
    }
}