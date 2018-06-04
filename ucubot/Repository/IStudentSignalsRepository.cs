using System.Collections.Generic;
using ucubot.Model;

namespace ucubot.Repository
{
    public interface IStudentSignalsRepository
    {
        IEnumerable<StudentSignal> GetAll();
    }
}