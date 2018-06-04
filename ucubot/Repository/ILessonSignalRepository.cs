using System.Collections.Generic;
using System.Threading.Tasks;
using ucubot.Model;

namespace ucubot.Repository
{
    public interface ILessonSignalRepository
    {
        bool Insert(SlackMessage message);
        void RemoveById(long id);
        IEnumerable<LessonSignalDto> GetAll();
        LessonSignalDto GetById(long id);
    }
}