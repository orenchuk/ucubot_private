using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ucubot.Model;

namespace ucubot.Repository
{
    public class StudentSignalsRepository : IStudentSignalsRepository
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public StudentSignalsRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration.GetConnectionString("BotDatabase");
        }

        public IEnumerable<StudentSignal> GetAll()
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                const string query = "SELECT * FROM student_signals;";
                return conn.Query<StudentSignal>(query).ToList();
            }
        }
    }
}