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


        //string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "TempUpload")).ToString();


        //public byte[] GenerateDocx(ExportQuizViewModel exportQuizViewModel)
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

                        // question loop here
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
                                // answers
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
                            //if(questionParent.QuestionType == Data.Enum.QuestionType.GROUP)
                            //{
                            //    //append space
                            //    runParent.AppendChild(new Break());
                            //}
                        }
                        questionCount = 1;
                        sectionCount++;
                    }
                    wordDocument.MainDocumentPart.Document.Save();
                    wordDocument.Close();
                    //string file = $"C:\\data\\newFileName.docx";
                    //File.WriteAllBytes("C:\\data\\newFileName.docx", mem.ToArray());
                    //return new FileStreamResult(mem, "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                    //{
                    //    FileDownloadName = "test.docx"
                    //};
                    byte[] rawBytes = mem.ToArray();
                    return rawBytes;
                }
            }
            return null;
        }


        //public void DocxChildQuestions(IEnumerable<Question> questionChild)
        //{
        //    // do something here to add child questions to the document...
        //    foreach(var question in questionChild)
        //    {
        //        Paragraph paragraphParent = body.AppendChild(new Paragraph());
        //        Run runParent = paragraphParent.AppendChild(new Run());
        //        runParent.AppendChild(new Text($"{questionCount}) {question.QuestionTitle}"));
        //    }
        //}


        public async Task<string> GenerateQTI(byte[] doc)
        {
            //var docToSend = "replace later";
            var qtiUrl = "";
            string key = "test1234"; // fake placeholder private key
            var headers = $"AUTHORIZATION: Api-Key {key}";

            // Configure httpClient
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = new TimeSpan(0, 0, 30);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(headers);
            //httpClient.DefaultRequestHeaders.Add("AUTHORIZATION: Api-Key", key);

            // CREATE JOB
            string createURL = $"https://digitaliser.getmarked.ai/api/v1.0/job/create_job/";
            //var headers = $"AUTHORIZATION: Api-Key {key}";
            // POST your file using multipart/form-data
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

                // GET JSON QUIZ VIA POLLING
                var quizJson = await httpClient.GetAsync(endpoint);
                quizJson.EnsureSuccessStatusCode();

                // POST JSON QUIZ TO GET QTI URL
                if (response.IsSuccessStatusCode)
                {
                    StringContent jsonQuizContent = new StringContent(quizJson.ToString(), Encoding.UTF8, "application/json");
                    string convertURL = $"https://digitaliser.getmarked.ai/api/v1.0/json_conversion/";
                    var quizUrl = await httpClient.PostAsync(convertURL, jsonQuizContent); // plus the json quiz how????
                    quizUrl.EnsureSuccessStatusCode();

                    // Parse response
                    if (quizUrl.IsSuccessStatusCode)
                    {
                        using var qtiStream = await quizUrl.Content.ReadAsStreamAsync();
                        JsonNode urlNode = JsonNode.Parse(qtiStream)!;
                        qtiUrl = urlNode!["data"]["url"]!.ToString();

                        // Return QTI quiz.zip file url
                        return qtiUrl;
                    }
                }
            }
            return null;
        }





    }
}
