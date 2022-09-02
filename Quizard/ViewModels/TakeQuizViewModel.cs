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
        // each checkbox is a seperate response. 
        // each UserQuestionResponse has a bool checked. 
        // html.checkmarkfor a collection of responses?? HOW
        public bool Checked { get; set; } // needs to be a collection...............


    }
}




// get questionid
// get element checkbox container div
// get list of answers (label)
// get list of checkmarks and their value
// send both arrays to controller
// for loop answers.length
// if checkmarks.value == true
// new question response: question id, responsetext = answers[i]