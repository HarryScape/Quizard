using Quizard.Data.Enum;
using Quizard.Interfaces;
using Quizard.Models;

namespace Quizard.Services
{
    public class BlackboardParserService : IBlackboardParserService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public BlackboardParserService(IQuizRepository quizRepository, IHttpContextAccessor contextAccessor)
        {
            _quizRepository = quizRepository;
            _contextAccessor = contextAccessor;
        }


        /// <summary>
        /// Parses an uploaded quiz and adds it to the DB
        /// </summary>
        /// <param name="file"></param>
        /// <returns>true if success</returns>
        public async Task<bool> ParseQuiz(IFormFile file)
        {
            var currentUser = _contextAccessor.HttpContext.User.GetUserId();

            Quiz quiz = new Quiz();
            quiz.QuizName = file.FileName;
            quiz.DateCreated = DateTime.Now;
            quiz.UserId = currentUser;
            quiz.Shuffled = false;
            quiz.Deployed = false;
            _quizRepository.Add(quiz);

            Section section = new Section();
            section.SectionName = "Default Question Pool";
            section.QuizId = quiz.Id;
            _quizRepository.Add(section);

            using (StreamReader fileReader = new StreamReader(file.OpenReadStream()))
            {
                while (!fileReader.EndOfStream)
                {
                    Question question = new Question();
                    List<Answer> answers = new List<Answer>();

                    var line = fileReader.ReadLine();
                    // tab delimitation
                    var values = line.Split("\t");

                    question.QuestionType = Enum.Parse<QuestionType>(values[0]);
                    question.QuestionTitle = values[1];
                    question.SectionId = section.Id;
                    _quizRepository.Add(question);

                    if (question.QuestionType == QuestionType.MC || question.QuestionType == QuestionType.MA)
                    {
                        for (int i = 2; i < values.Length; i += 2)
                        {
                            Answer answer = new Answer();
                            answer.QuestionAnswer = values[i];
                            if (values[i + 1].Contains("Correct"))
                            {
                                answer.isCorrect = true;
                            }
                            answer.QuestionId = question.Id;
                            answers.Add(answer);
                        }
                    }
                    else if (question.QuestionType == QuestionType.TF || question.QuestionType == QuestionType.FIB || question.QuestionType == QuestionType.NUM)
                    {
                        for (int i = 2; i < values.Length; i += 2)
                        {
                            Answer answer = new Answer();
                            answer.QuestionAnswer = values[i];
                            answer.isCorrect = true;
                            answer.QuestionId = question.Id;
                            answers.Add(answer);
                        }
                    }
                    else if (question.QuestionType == QuestionType.ESS || question.QuestionType == QuestionType.SR)
                    {
                        for (int i = 2; i < values.Length; i += 2)
                        {
                            Answer answer = new Answer();
                            answer.QuestionAnswer = "...";
                            answer.QuestionId = question.Id;
                            answers.Add(answer);
                        }
                    }
                    _quizRepository.Add(answers);
                }
            }
            return true;
        }
    }
}
