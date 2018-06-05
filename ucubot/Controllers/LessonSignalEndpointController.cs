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
    public class LessonSignalEndpointController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILessonSignalRepository _signalRepository;

        public LessonSignalEndpointController(IConfiguration configuration)
        {
            _configuration = configuration;
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            _signalRepository = new LessonSignalRepository(connectionString);
        }

        [HttpGet]
        public IEnumerable<LessonSignalDto> ShowSignals()
        {
            return _signalRepository.GetAll();
        }
        
        [HttpGet("{id}")]
        public LessonSignalDto ShowSignal(long id)
        {
            return _signalRepository.GetById(id);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateSignal(SlackMessage message)
        {
            var wasCreated = _signalRepository.Insert(message);

            if (!wasCreated)
            {
                return BadRequest();
            }
            
            return Accepted();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSignal(long id)
        {
            _signalRepository.RemoveById(id);
            
            return Accepted();
        }
    }
}
