using CsvHelper.Configuration;
using Quizard.Models;

namespace Quizard.Mappers
{
    public sealed class AnswerMap : ClassMap<Answer>
    {
        //private List<string> Answers = new List<string> { "Answer", "isCorrect"};

        public AnswerMap()
        {
            // The index is dynamic. Need to figure out how to configure the changing index.
            Map(m => m.QuestionAnswer).Index(3);
            Map(m => m.isCorrect).Index(4);
        }
    }
}
