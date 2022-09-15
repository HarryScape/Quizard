using Quizard.Interfaces;
using Quizard.Models;
using Quizard.ViewModels;
using Quizard.Data.Enum;
using System.Text.RegularExpressions;

namespace Quizard.Services
{
    public class MarkdownParserService : IMarkdownParserService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public MarkdownParserService(IQuizRepository quizRepository, IHttpContextAccessor contextAccessor)
        {
            _quizRepository = quizRepository;
            _contextAccessor = contextAccessor;
        }


        public async Task<bool> ParseQuizA(IFormFile file)
        {
            // bool isComment = line.StartsWith("...");
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
                            question.QuestionTitle = string.Join(" ", questionList[0].Split().Skip(1));
                            question.SectionId = section.Id;
                            _quizRepository.Add(question);

                            List<Answer> answers = new List<Answer>();
                            int count = 0;
                            bool F = false;
                            bool T = false;

                            for (int j = 1; j < questionList.Count; j++)
                            {
                                Answer answer = new Answer();
                                if (questionList[j].Contains("*"))
                                {
                                    answer.QuestionAnswer = string.Join(" ", questionList[j].Split().Skip(1));
                                    answer.isCorrect = true;
                                    count++;
                                }
                                else if (questionList[j].Contains("___"))
                                {
                                    answer.QuestionAnswer = "___";
                                    question.QuestionType = Enum.Parse<QuestionType>("ESS");
                                    _quizRepository.Update(question);
                                }
                                else
                                {
                                    answer.QuestionAnswer = string.Join(" ", questionList[j].Split().Skip(1));
                                }

                                if (questionList[j].Contains("True"))
                                {
                                    answer.QuestionAnswer = string.Join(" ", questionList[j].Split().Skip(1));
                                    T = true;
                                }
                                else if (questionList[j].Contains("False"))
                                {
                                    answer.QuestionAnswer = string.Join(" ", questionList[j].Split().Skip(1));
                                    F = true;
                                }
                                answer.QuestionId = question.Id;
                                answers.Add(answer);
                            }

                            if (T && F)
                            {
                                question.QuestionType = Enum.Parse<QuestionType>("TF");
                                _quizRepository.Update(question);
                            }
                            else if (count == (lines.Count - 1))
                            {
                                question.QuestionType = Enum.Parse<QuestionType>("FIB");
                                _quizRepository.Update(question);
                            }
                            else if (count > 1)
                            {
                                question.QuestionType = Enum.Parse<QuestionType>("MA");
                                _quizRepository.Update(question);
                            }
                            else if (count == 1)
                            {
                                question.QuestionType = Enum.Parse<QuestionType>("MC");
                                _quizRepository.Update(question);
                            }

                            _quizRepository.Add(answers);
                            questionList.Clear();
                            continue;
                        }
                        questionList.Add(lines[i]);
                    }
                   
                }
            }
            return true;
        }




        public async Task<bool> ParseQuizB(IFormFile file)
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
