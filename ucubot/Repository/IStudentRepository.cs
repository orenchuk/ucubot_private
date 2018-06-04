using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ucubot.Model;

namespace ucubot.Repository
{
    public interface IStudentRepository
    {
        bool Insert(Student student);
        bool RemoveById(long id);
        void Update(Student student);
        IEnumerable<Student> GetAll();
        Student GetById(long id);
    }
}