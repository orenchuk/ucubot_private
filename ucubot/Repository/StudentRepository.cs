using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dapper;
using MySql.Data.MySqlClient;
using ucubot.Model;

namespace ucubot.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private string connectionString;

        public StudentRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool Insert(Student student)
        {
            var firstName = student.FirstName;
            var lastName = student.LastName;
            var userId = student.UserId;

            using (var conn = new MySqlConnection(connectionString))
            {
                const string selectQuery = "SELECT student.FirstName as FirstName, student.Id as Id, student.LastName as LastName, student.UserId as UserId FROM student WHERE UserId = @UserID;";
                var createdStudents = conn.Query<Student>(selectQuery, new {UserID = userId}).ToList();
                
                if (createdStudents.Any())
                {
                    return false;
                }

                const string insertQuery = "INSERT INTO student (FirstName, LastName, UserId) VALUES (@first_name, @last_name, @user_id);";
                conn.Query<Student>(insertQuery, new {first_name = firstName, last_name = lastName, user_id = userId});
                return true;
            }
        }

        public bool RemoveById(long id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                const string selectQuery = "SELECT * FROM lesson_signal WHERE student_id = @ID;";
                var signals = conn.Query<LessonSignalDto>(selectQuery, new {ID = id}).ToList();
                if (signals.Any())
                {
                    return false;
                }

                const string deleteQuery = "DELETE FROM student WHERE Id = @ID;";
                conn.Query<Student>(deleteQuery, new {ID = id});
                return true;
            }
        }

        public void Update(Student student)
        {
            var FirstName = student.FirstName;
            var LastName = student.LastName;
            var UserId = student.UserId;
            var id = student.Id;

            using (var conn = new MySqlConnection(connectionString))
            {
                const string query = "UPDATE student SET FirstName = @firstName, LastName = @lastName, UserId = @userid WHERE Id = @ID;";
                conn.Query<Student>(query, new {firstName = FirstName, lastName = LastName, userid = UserId, ID = id});
            }
        }

        public IEnumerable<Student> GetAll()
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                const string query = "SELECT student.FirstName as FirstName, student.Id as Id, student.LastName as LastName, student.UserId as UserId FROM student;";
                return conn.Query<Student>(query).ToList();
            }
        }

        public Student GetById(long id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                const string query = "SELECT student.FirstName as FirstName, student.Id as Id, student.LastName as LastName, student.UserId as UserId FROM student WHERE id = @ID;";
                var signals = conn.Query<Student>(query, new {ID = id}).ToList();
                return signals.Any() ? signals.First() : null; 
            }
        }
    }
}