using Microsoft.AspNetCore.Mvc;
using Quizard.Interfaces;
using Quizard.Models;
using Quizard.ViewModels;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.InkML;

namespace Quizard.Services
{
    public class QuizExportService : IQuizExportService
    {
        private readonly IQuizRepository _quizRepository;

        public QuizExportService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
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

        //public FileResult GenerateDocx(ExportQuizViewModel exportQuizViewModel)
        //{
        //    //string docPath = "~/Content/";
        //    //Byte[] file;

        //    //using (MemoryStream mem = new MemoryStream())
        //    //{
        //    //    using (WordprocessingDocument wordDocument =
        //    //        WordprocessingDocument.Create(docPath, WordprocessingDocumentType.Document))
        //    //    {
        //    //        // Insert other code here. 
        //    //    }
        //    //    file = mem.ToArray();
        //    //}

        //}




        public byte[] GenerateDocx(ExportQuizViewModel exportQuizViewModel)
        {
            //string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "TempUpload")).ToString();
            Run lineBreak = new Run(new Break());
            int sectionCount = 1, questionCount = 1, questionChildCount = 1, answerCount = 1;
            var alphabetLabel = Enumerable.Range('a', 'z' - 'a' + 1).Select(c => (char)c).ToList();

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
                    Run run = para.AppendChild(new Run());
                    run.AppendChild(new Text(exportQuizViewModel.Quiz.QuizName));

                    foreach(var section in exportQuizViewModel.Sections)
                    {
                        Paragraph paragraph = body.AppendChild(new Paragraph());
                        Run runName = paragraph.AppendChild(new Run());
                        runName.AppendChild(new Text($"Section {sectionCount}: {section.SectionName}"));

                        // question loop here
                        foreach(var questionParent in exportQuizViewModel.ParentQuestions.Where(i => i.SectionId == section.Id)){
                            Paragraph paragraphParent = body.AppendChild(new Paragraph());
                            Run runParent = paragraphParent.AppendChild(new Run());
                            runParent.AppendChild(new Text($"{questionCount}) {questionParent.QuestionTitle}"));
                            // recursion for children...

                            questionCount++;
                        }
                        questionCount = 1;
                        sectionCount++;
                    }


                    wordDocument.MainDocumentPart.Document.Save();
                    wordDocument.Close();
                    File.WriteAllBytes("C:\\data\\newFileName.docx", mem.ToArray());
                }
            }
            return null;
        }








    }
}
