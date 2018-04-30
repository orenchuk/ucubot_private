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
    public class LessonSignalEndpointController : Controller
    {
        private readonly IConfiguration _configuration;

        public LessonSignalEndpointController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<LessonSignalDto> ShowSignals()
        {
            var  connectionString = _configuration.GetConnectionString("BotDatabase");
            using (var conn = new MySqlConnection(connectionString))
            {
                const string query = "SELECT lesson_signal.ID as id, lesson_signal.Timestamp_ as time_stamp, lesson_signal.SignalType as signal_type, lesson_signal.student_id as student_id  FROM lesson_signal LEFT JOIN (student) ON (lesson_signal.student_id = student.UserId);";
                return conn.Query<LessonSignalDto>(query).ToList();
            }
        }
        
        [HttpGet("{id}")]
        public LessonSignalDto ShowSignal(long id)
        {
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            using (var conn = new MySqlConnection(connectionString))
            {
                const string query = "SELECT lesson_signal.ID as id, lesson_signal.Timestamp_ as time_stamp, lesson_signal.SignalType as signal_type, lesson_signal.student_id as student_id  FROM lesson_signal LEFT JOIN (student) ON (lesson_signal.student_id = student.UserId) WHERE id = @id;";
                var signals = conn.Query<LessonSignalDto>(query).ToList();
                return signals.Any() ? signals.First() : null; 
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateSignal(SlackMessage message)
        {
            var userId = message.user_id;
            var signalType = message.text.ConvertSlackMessageToSignalType();
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            
            using (var conn = new MySqlConnection(connectionString))
            {
                var query = "SELECT * FROM student WHERE student.UserId = @userId";
                conn.Open();
                var cmd = conn.CreateCommand();
                
                var exist = conn.CreateCommand();
                exist.CommandText = query;
                exist.Parameters.Add("userId", userId);
                
                var adapter = new MySqlDataAdapter(exist);
                var _dataTable = new DataTable();
                adapter.Fill(_dataTable);
                if (_dataTable.Rows.Count > 0)
                {
                    return BadRequest();
                }
                else
                {
                    cmd.Parameters.AddRange(new[]
                    {
                        new MySqlParameter("userId", userId),
                        new MySqlParameter("signalType", signalType),
                        new MySqlParameter("timestamp", DateTime.Now)
                    });
                    cmd.CommandText =
                        "INSERT INTO lesson_signal (UserID, SignalType, TimeStamp_) VALUES (@userId, @signalType, @timestamp);";
                    await cmd.ExecuteNonQueryAsync();
                }

                conn.Close();
            }

            return Accepted();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveSignal(long id)
        {
            var  connectionString = _configuration.GetConnectionString("BotDatabase");
            using (var conn = new MySqlConnection(connectionString))   
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM lesson_signal WHERE ID = @id;";
                cmd.Parameters.Add(new MySqlParameter("ID", id));
                await cmd.ExecuteNonQueryAsync();
            }
            
            return Accepted();
        }
    }
}
