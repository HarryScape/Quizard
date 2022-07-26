using Quizard.Interfaces;
using Quizard.Models;
using Quizard.ViewModels;
using Quizard.Data.Enum;

namespace Quizard.Services
{
    public class CanvasParserService : ICanvasParserService
    {
        private readonly IQuizRepository _quizRepository;
        public CanvasParserService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }


        public async Task<bool> ParseQuiz(IFormFile file, DashboardViewModel dashboardViewModel)
        {
            // make an array of lines until an empty line is reached
            // skip first word of line[0]
            // questionTitle = line[0] - first word
            // foreach nextline of answers:
            //      if line contains "*" isCorrect = true
            //      if line contains "..." add as Comment
            //      questionAnswer = line[i] - first word
            //  if answers contains "___" qtypee = ESS
            //  if answers every line contains "*" qtype = FIB
            //  if answers contains > 1 "*" qtype = MC
            //  if answers contains exactly 1 "*" qtype = MC
            //  if answers contrains "true" || "false" qtype = TF
            // bool isComment = line.StartsWith("...");

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
    }
}
