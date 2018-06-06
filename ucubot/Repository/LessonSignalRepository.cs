using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ucubot.Model;

namespace ucubot.Repository
{
    public class LessonSignalRepository : ILessonSignalRepository
    {
        private readonly string connectionString;

        public LessonSignalRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("BotDatabase");
        }

        public bool Insert(SlackMessage message)
        {
            var userId = message.user_id;
            var signalType = message.text.ConvertSlackMessageToSignalType();
            
            using (var conn = new MySqlConnection(connectionString))
            {  
                const string studentUseridQuery = "SELECT * FROM student WHERE student.UserId = @UserId";
                var listOfStudents = conn.Query<Student>(studentUseridQuery, new {UserId = userId}).ToList();
                if (!listOfStudents.Any())
                {
                    return false;
                }
                const string insertQuery = "INSERT INTO lesson_signal (student_id, SignalType, TimeStamp_) VALUES (@ID, @signal_type, @time_stamp);";
                var student = listOfStudents.First();
                conn.Query<LessonSignalDto>(insertQuery,
                    new {ID = student.Id, signal_type = signalType, time_stamp = DateTime.Now});                   
            }

            return true;
        }

        public async void RemoveById(long id)
        {
            using (var conn = new MySqlConnection(connectionString))   
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM lesson_signal WHERE ID = @id;";
                cmd.Parameters.Add(new MySqlParameter("ID", id));
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public IEnumerable<LessonSignalDto> GetAll()
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                const string query = "SELECT lesson_signal.ID as Id, lesson_signal.Timestamp_ as Timestamp, lesson_signal.SignalType as Type, student.UserId as UserId FROM lesson_signal LEFT JOIN (student) ON (lesson_signal.student_id = student.Id);";
                return conn.Query<LessonSignalDto>(query).ToList();
            }
        }

        public LessonSignalDto GetById(long id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                const string query = "SELECT lesson_signal.ID as Id, lesson_signal.Timestamp_ as Timestamp, lesson_signal.SignalType as Type, student.UserId as UserId FROM lesson_signal LEFT JOIN (student) ON (lesson_signal.student_id = student.Id) WHERE lesson_signal.ID = @ID;";
                var signals = conn.Query<LessonSignalDto>(query, new {ID = id}).ToList();
                return signals.Any() ? signals.First() : null; 
            }
        }
    }
}