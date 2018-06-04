using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using ucubot.Model;

namespace ucubot.Repository
{
    public class StudentSignalsRepository : IStudentSignalsRepository
    {
        private readonly string connectionString;

        public StudentSignalsRepository(string connectionString)
        {
            this.connectionString = connectionString;
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