using CsvHelper.Configuration;
using Quizard.Models;

namespace Quizard.Mappers
{
    public sealed class QuestionMap : ClassMap<Question>
    {
        public QuestionMap()
        {
            Map(m => m.QuestionType).Index(0);
            Map(m => m.QuestionTitle).Index(1);
        }
    }
}
