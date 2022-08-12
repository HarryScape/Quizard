using Quizard.Interfaces;
using Quizard.Models;
using Quizard.ViewModels;
using Quizard.Data.Enum;
using Quizard.Interfaces;
using Quizard.ViewModels;
using System.Text.RegularExpressions;

namespace Quizard.Services
{
    public class MoodleParserService : IMoodleParserService
    {
        private readonly IQuizRepository _quizRepository;
        public MoodleParserService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }
        public async Task<bool> ParseQuiz(IFormFile file, DashboardViewModel dashboardViewModel)
        {
            Quiz quiz = new Quiz();
            quiz.QuizName = file.FileName;
            quiz.DateCreated = DateTime.Now;
            quiz.UserId = dashboardViewModel.UserId;
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
                    List<string> lines = new List<string>();

                    string line;
                    while ((line = fileReader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }

                    List<string> questionList = new List<string>();

                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (lines[i] == String.Empty)
                        {
                            Question question = new Question();
                            var rowParts = lines[0].Split("::");
                            question.QuestionTitle = rowParts[2].Remove(rowParts[2].Length - 2);
                            question.SectionId = section.Id;
                            _quizRepository.Add(question);

                            List<Answer> answers = new List<Answer>();
                            int count = 0;
                            Regex reg = new Regex("[^a-zA-Z']");

                            for (int j = 1; j < questionList.Count; j++)
                            {
                                Answer answer = new Answer();
                                // if qlist.size = 1 qtype == ESS
                                if (questionList.Count == 1)
                                {
                                    question.QuestionType = Enum.Parse<QuestionType>("ESS");
                                    _quizRepository.Update(question);
                                }
                                if (questionList[j].Contains("="))
                                {
                                    answer.QuestionAnswer = reg.Replace(questionList[j], string.Empty);
                                    answer.isCorrect = true;
                                    count++;
                                }

                                    answer.QuestionId = question.Id;
                                answers.Add(answer);
                            }
                        }
                        questionList.Add(lines[i]);
                    }
                }
            }


            return true;
        }
    }
}
