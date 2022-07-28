using Quizard.Data.Enum;
using Quizard.Interfaces;
using Quizard.Models;
using Quizard.Repository;
using Quizard.ViewModels;

namespace Quizard.Services
{
    public class BlackboardParserService : IBlackboardParserService
    {
        private readonly IQuizRepository _quizRepository;
        public BlackboardParserService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<bool> ParseQuiz(IFormFile file, DashboardViewModel dashboardViewModel)
        {
            Quiz quiz = new Quiz();
            quiz.QuizName = file.FileName;
            quiz.DateCreated = DateTime.Now;
            quiz.UserId = dashboardViewModel.UserId;
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
                    var values = line.Split("\t");

                    question.QuestionType = Enum.Parse<QuestionType>(values[0]);
                    question.QuestionTitle = values[1];
                    question.SectionId = section.Id;
                    _quizRepository.Add(question);

                    if (question.QuestionType == QuestionType.MC || question.QuestionType == QuestionType.MA){
                        for (int i = 2; i < values.Length; i += 2)
                        {
                            Answer answer = new Answer();
                            answer.QuestionAnswer = values[i];
                            if (values[i + 1].Contains("Correct")){
                                answer.isCorrect = true;
                        }
                            //answer.isCorrect = values[i + 1];
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
