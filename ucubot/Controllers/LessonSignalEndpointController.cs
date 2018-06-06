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
        private readonly ILessonSignalRepository signalRepository;

        public LessonSignalEndpointController(ILessonSignalRepository lessonSignalRepository)
        {
            signalRepository = lessonSignalRepository;
        }

        [HttpGet]
        public IEnumerable<LessonSignalDto> ShowSignals()
        {
            return signalRepository.GetAll();
        }
        
        [HttpGet("{id}")]
        public LessonSignalDto ShowSignal(long id)
        {
            return signalRepository.GetById(id);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateSignal(SlackMessage message)
        {
            var wasCreated = signalRepository.Insert(message);

            if (!wasCreated)
            {
                return BadRequest();
            }
            
            return Accepted();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSignal(long id)
        {
            signalRepository.RemoveById(id);
            
            return Accepted();
        }
    }
}
