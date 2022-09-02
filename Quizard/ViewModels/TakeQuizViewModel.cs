using Quizard.Data.Enum;
using Quizard.Models;

namespace Quizard.ViewModels
{
    public class TakeQuizViewModel
    {
        public Quiz Quiz { get; set; }
        public IEnumerable<Section> Sections { get; set; }
        public Section Section { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public IEnumerable<Question> ParentQuestions { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
        public QuestionType QuestionType { get; set; }

        public int AttemptId { get; set; }

        public IEnumerable<UserQuestionResponse> QuestionResponses { get; set; }


        //public int SectionCount { get; set; }
    }
}
