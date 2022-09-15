using Microsoft.AspNetCore.Mvc;
using Quizard.Interfaces;
using Quizard.Models;
using Quizard.ViewModels;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.InkML;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text;
using System.Net.Http.Headers;

namespace Quizard.Services
{
    public class QuizExportService : IQuizExportService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public QuizExportService(IQuizRepository quizRepository, IHttpClientFactory httpClientFactory)
        {
            _quizRepository = quizRepository;
            _httpClientFactory = httpClientFactory;
        }


        /// <summary>
        /// Generates a quiz ViewModel to contain all quiz objects to bo converted during export
        /// Utilises DocumentFormat.OpenXml
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Quiz ViewModel</returns>
        public async Task<ExportQuizViewModel> GenerateQuizViewModel(int id)
        {
            var quizViewModel = new ExportQuizViewModel();
            quizViewModel.Quiz = await _quizRepository.GetQuizById(id);
            quizViewModel.Sections = await _quizRepository.GetQuizSections(id);
            quizViewModel.Questions = await _quizRepository.GetQuestionByQuizID(id);
            quizViewModel.ParentQuestions = await _quizRepository.GetParentQuestions(id);
            quizViewModel.Answers = await _quizRepository.GetSpecificAnswers(id);

            foreach (var question in quizViewModel.ParentQuestions)
            {
                question.Children = (ICollection<Question>)await _quizRepository.GetChildQuestions(question.Id);
                question.QuestionAnswers = (ICollection<Answer>)await _quizRepository.GetAnswersByQuestion(question.Id);
            }
            foreach (var question in quizViewModel.Questions)
            {
                question.Children = (ICollection<Question>)await _quizRepository.GetChildQuestions(question.Id);
                question.QuestionAnswers = (ICollection<Answer>)await _quizRepository.GetAnswersByQuestion(question.Id);
            }
            return quizViewModel;
        }


        /// <summary>
        /// Generates a .docx file of the quiz for exporting
        /// </summary>
        /// <param name="exportQuizViewModel"></param>
        /// <returns>.docx quiz</returns>
        public async Task<byte[]> GenerateDocx(ExportQuizViewModel exportQuizViewModel)
        {
            //Run lineBreak = new Run(new Break());
            int sectionCount = 1, questionCount = 1, questionChildCount = 0, answerCount = 1;
            var alphabetLabel = Enumerable.Range('a', 'z' - 'a' + 1).Select(c => (char)c).ToList();
            Indentation indentation1 = new Indentation() { Left = "1440", Hanging = "720" };

            using (MemoryStream mem = new MemoryStream())
            {
                // Create Document
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
                {
                    // Add a main document part. 
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());
                    Paragraph para = body.AppendChild(new Paragraph());

                    RunProperties runProperties1 = new RunProperties();
                    FontSize fontSize1 = new FontSize() { Val = "36" };
                    runProperties1.Append(fontSize1);

                    Run run = para.AppendChild(new Run());
                    run.Append(runProperties1);
                    run.AppendChild(new Text(exportQuizViewModel.Quiz.QuizName));
                    run.AppendChild(new Break());

                    foreach (var section in exportQuizViewModel.Sections)
                    {
                        Paragraph paragraph = body.AppendChild(new Paragraph());
                        RunProperties runProperties2 = new RunProperties();
                        FontSize fontSize2 = new FontSize() { Val = "28" };
                        runProperties2.Append(fontSize2);
                        Run runName = paragraph.AppendChild(new Run());
                        runName.Append(runProperties2);
                        runName.AppendChild(new Break());
                        runName.AppendChild(new Text($"Section {sectionCount}: {section.SectionName}"));
                        runName.AppendChild(new Break());

                        // questions are added
                        foreach (var questionParent in exportQuizViewModel.ParentQuestions.Where(i => i.SectionId == section.Id))
                        {
                            Paragraph paragraphParent = body.AppendChild(new Paragraph());
                            Run runParent = paragraphParent.AppendChild(new Run());
                            runParent.Append(new Break());
                            //runParent.AppendChild(new Text($"{questionCount}) {questionParent.QuestionTitle}"));
                            if (questionParent.Mark != null)
                            {
                                runParent.AppendChild(new Text($"{questionCount}) {questionParent.QuestionTitle} [{questionParent.Mark}]"));
                            }
                            else
                            {
                                runParent.AppendChild(new Text($"{questionCount}) {questionParent.QuestionTitle}"));
                            }

                            foreach (var questionChild in questionParent.Children)
                            {
                                Paragraph paragraphChild = body.AppendChild(new Paragraph());
                                Run runChild = paragraphChild.AppendChild(new Run());
                                runChild.Append(new Break());
                                //runChild.AppendChild(new Text($"{alphabetLabel[questionChildCount]}) {questionChild.QuestionTitle}"));
                                if (questionParent.Mark != 0 || questionParent.Mark != null)
                                {
                                    runChild.AppendChild(new Text($"{alphabetLabel[questionChildCount]}) {questionChild.QuestionTitle} [{questionChild.Mark}]"));
                                }
                                else
                                {
                                    runChild.AppendChild(new Text($"{alphabetLabel[questionChildCount]}) {questionChild.QuestionTitle}"));
                                }
                                questionChildCount++;
                                // answers are added
                                if (questionChild.QuestionAnswers != null)
                                {
                                    foreach (var ans in questionChild.QuestionAnswers)
                                    {
                                        if (ans.isCorrect == true)
                                        {
                                            Paragraph paragraphAnswer = body.AppendChild(new Paragraph());
                                            Run runAnswer = paragraphAnswer.AppendChild(new Run());
                                            runAnswer.AppendChild(new Text($"* {answerCount}. {ans.QuestionAnswer}"));
                                            answerCount++;
                                        }
                                        else
                                        {
                                            Paragraph paragraphAnswer = body.AppendChild(new Paragraph());
                                            Run runAnswer = paragraphAnswer.AppendChild(new Run());
                                            runAnswer.AppendChild(new Text($"{answerCount}. {ans.QuestionAnswer}"));
                                            answerCount++;
                                        }
                                    }
                                    answerCount = 1;
                                }
                            }
                            questionChildCount = 0;
                            questionCount++;

                            if (questionParent.QuestionAnswers != null)
                            {
                                foreach (var ans in questionParent.QuestionAnswers)
                                {
                                    if (ans.isCorrect == true)
                                    {
                                        Paragraph paragraphAnswer = body.AppendChild(new Paragraph());
                                        Run runAnswer = paragraphAnswer.AppendChild(new Run());
                                        runAnswer.AppendChild(new Text($"   * {answerCount}. {ans.QuestionAnswer}"));
                                        answerCount++;
                                    }
                                    else
                                    {
                                        Paragraph paragraphAnswer = body.AppendChild(new Paragraph());
                                        Run runAnswer = paragraphAnswer.AppendChild(new Run());
                                        runAnswer.AppendChild(new Text($"   {answerCount}. {ans.QuestionAnswer}"));
                                        answerCount++;
                                    }
                                }
                                answerCount = 1;

                            }
                        }
                        questionCount = 1;
                        sectionCount++;
                    }
                    wordDocument.MainDocumentPart.Document.Save();
                    wordDocument.Close();
                    byte[] rawBytes = mem.ToArray();
                    return rawBytes;
                }
            }
        }


        /// <summary>
        /// Consumes external GETMARKED API to convert .docx quiz to QTI download link
        /// </summary>
        /// <param name="doc">.docx quiz</param>
        /// <returns>URL to download QTI quiz</returns>
        public async Task<string> GenerateQTI(byte[] doc)
        {
            var qtiUrl = "";
            string key = "test1234"; // fake placeholder private key
            var headers = $"AUTHORIZATION: Api-Key {key}";

            // Configure httpClient
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = new TimeSpan(0, 0, 30);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(headers);
            //httpClient.DefaultRequestHeaders.Add("AUTHORIZATION: Api-Key", key);

            // Create job
            string createURL = $"https://digitaliser.getmarked.ai/api/v1.0/job/create_job/";
            //var headers = $"AUTHORIZATION: Api-Key {key}";
            // POST .docx using multipart/form-data
            var content = new MultipartFormDataContent("------------" + Guid.NewGuid());
            var byteArrayContent = new ByteArrayContent(doc);
            byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            content.Add(byteArrayContent, "\"file\"", $"\"quiz.docx\"");

            var response = await httpClient.PostAsync(createURL, content);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                // Parse response to get endpoint
                using var contentStream = await response.Content.ReadAsStreamAsync();
                JsonNode endpointNode = JsonNode.Parse(contentStream)!;
                string endpoint = endpointNode!["data"]["result_endpoint"]!.ToString();

                // Get JSON endpoint via polling
                var quizJson = await httpClient.GetAsync(endpoint);
                quizJson.EnsureSuccessStatusCode();

                // Post JSON quiz to get QTI URL response
                if (response.IsSuccessStatusCode)
                {
                    StringContent jsonQuizContent = new StringContent(quizJson.ToString(), Encoding.UTF8, "application/json");
                    string convertURL = $"https://digitaliser.getmarked.ai/api/v1.0/json_conversion/";
                    var quizUrl = await httpClient.PostAsync(convertURL, jsonQuizContent);
                    quizUrl.EnsureSuccessStatusCode();

                    // Parse QTI URL response
                    if (quizUrl.IsSuccessStatusCode)
                    {
                        using var qtiStream = await quizUrl.Content.ReadAsStreamAsync();
                        JsonNode urlNode = JsonNode.Parse(qtiStream)!;
                        qtiUrl = urlNode!["data"]["url"]!.ToString();

                        return qtiUrl;
                    }
                }
            }
            return null;
        }

    }
}
