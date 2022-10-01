using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Quizard.Data.Enum;
using Quizard.Interfaces;
using Quizard.Models;
using Quizard.ViewModels;

namespace Quizard.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IQuizParserService _quizParserService;
        private readonly ITakeQuizRepository _takeQuizRepository;
        private readonly IImageService _imageService;



        public QuizController(IQuizRepository quizRepository, IQuizParserService quizParserService, ITakeQuizRepository takeQuizRepository, IImageService imageService)
        {
            _quizRepository = quizRepository;
            _quizParserService = quizParserService;
            _takeQuizRepository = takeQuizRepository;
            _imageService = imageService;
        }


        [ActionName("Create")]
        public async Task<IActionResult> Create(int quizId)
        {
            var quizViewModel = await _quizParserService.GenerateQuizViewModel(quizId);

            return View(quizViewModel);
        }


        // Generates quiz sections in view
        [HttpPost]
        public ActionResult SectionPartialView(string sectionName, int quizId)
        {

            var section = new Section()
            {
                SectionName = sectionName,
                QuizId = quizId
            };
            _quizRepository.Add(section);

            CreateQuizViewModel quizViewModel = new CreateQuizViewModel();

            return PartialView("_Section", quizViewModel);
        }


        // Generate Modal Popup
        public async Task<IActionResult> ShowCaseStudyModal(int quizId)
        {
            IEnumerable<Section> sections = await _quizRepository.GetQuizSections(quizId);
            List<SelectListItem> listItems = new List<SelectListItem>();

            foreach (Section section in sections)
            {
                listItems.Add(new SelectListItem()
                {
                    Value = section.Id.ToString(),
                    Text = section.SectionName
                });
            }

            AddCaseStudyViewModel addCaseStudyViewModel = new AddCaseStudyViewModel();
            addCaseStudyViewModel.SectionList = listItems;
            addCaseStudyViewModel.QuizId = quizId;

            return PartialView("_AddCaseStudyModalPartial", addCaseStudyViewModel);
        }


        /// <summary>
        /// Adds a new case study or question group
        /// </summary>
        /// <param name="addCaseStudyViewModel"></param>
        public async Task<IActionResult> AddCaseStudy(AddCaseStudyViewModel addCaseStudyViewModel)
        {
            if(addCaseStudyViewModel.CaseStudyName != null)
            {
                Question question = new Question()
                {
                    QuestionTitle = addCaseStudyViewModel.CaseStudyName,
                    SectionId = Int32.Parse(addCaseStudyViewModel.SectionSelected),
                    QuestionType = QuestionType.GROUP
                };
                _quizRepository.Add(question);
            }

            var quizViewModel = await _quizParserService.GenerateQuizViewModel(addCaseStudyViewModel.QuizId);

            return PartialView("_Section", quizViewModel);
        }


        /// <summary>
        /// Adds a new section to the quiz
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="quizId"></param>
        [HttpPost]
        public async Task<IActionResult> AddSectionDB(string sectionName, int quizId)
        {
            if(sectionName != null)
            {
                var section = new Section()
                {
                    SectionName = sectionName,
                    QuizId = quizId
                };
                _quizRepository.Add(section);
            }

            var quizViewModel = await _quizParserService.GenerateQuizViewModel(quizId);

            return PartialView("_Section", quizViewModel);
        }


        /// <summary>
        /// Saves the position of questions in the quiz
        /// </summary>
        /// <param name="updates"> Question model mapped to a JSON class</param>
        [HttpPost]
        public async Task<IActionResult> SaveQuiz(List<QuestionJsonHelper> updates)
        {
            foreach (var item in updates)
            {
                Question question = await _quizRepository.GetQuestionById(Int32.Parse(item.Id));
                question.SectionId = Int32.Parse(item.SectionId);
                question.QuestionPosition = (item.QuestionPosition + 1);
                question.ParentId = item.ParentId;
                _quizRepository.Update(question);
            }

            Quiz quiz = await _quizRepository.GetQuizById(updates[0].QuizId);
            quiz.DateCreated = DateTime.Now;
            _quizRepository.Update(quiz);

            var message = "State saved";
            return Json(message);
        }


        /// <summary>
        /// Manually deletes a quiz as cascading delete constraint in the DB is disabled
        /// </summary>
        /// <param name="id"></param>
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var quiz = await _quizRepository.GetQuizById(id);
            IEnumerable<Answer> answers = await _quizRepository.GetSpecificAnswers(id);
            IEnumerable<Section> sections = await _quizRepository.GetQuizSections(id);
            IEnumerable<Question> questions = await _quizRepository.GetQuestionByQuizID(id);

            foreach (Question question in questions)
            {
                IEnumerable<UserQuestionResponse> responses = await _takeQuizRepository.GetSingleResponseByQuestionId(questions.First().Id);
                if (responses != null)
                {
                    foreach (UserQuestionResponse response in responses)
                    {
                        response.QuestionId = null;
                        response.AnswerId = null;
                        _takeQuizRepository.Update(response);
                    }
                }
                if (!string.IsNullOrEmpty(question.ContentUrl))
                {
                    _ = _imageService.DeleteImage(question.ContentUrl);
                }
            }

            IEnumerable<UserQuizAttempt> attempts = await _takeQuizRepository.GetAttemptsByQuizId(id);
            foreach(UserQuizAttempt attempt in attempts)
            {
                attempt.QuizId = null;
                _takeQuizRepository.Update(attempt);
            }

            if (quiz == null) return View("Error");

            _quizRepository.DeleteAns(answers);
            _quizRepository.DeleteQuestions(questions);
            _quizRepository.DeleteSections(sections);
            _quizRepository.Delete(quiz);

            return Json(new { redirectToUrl = Url.Action("Index", "Dashboard") });
        }


        /// <summary>
        /// Deletes a section including questions within the section
        /// </summary>
        /// <param name="sectionId"></param>
        [HttpPost]
        public async Task<IActionResult> DeleteSection(int sectionId)
        {
            var section = await _quizRepository.GetSectionById(sectionId);
            int quizId = section.QuizId;


            IEnumerable<Question> questions = await _quizRepository.GetQuestionBySectionID(sectionId);

            foreach (Question question in questions)
            {
                IEnumerable<UserQuestionResponse> responses = await _takeQuizRepository.GetSingleResponseByQuestionId(questions.First().Id);
                if (responses != null)
                {
                    foreach (UserQuestionResponse response in responses)
                    {
                        response.QuestionId = null;
                        response.AnswerId = null;
                        _takeQuizRepository.Update(response);
                    }
                }
            }

            if (questions != null)
            {
                foreach (Question item in questions)
                {
                    IEnumerable<Answer> answers = await _quizRepository.GetAnswersByQuestion(item.Id);
                    if (answers != null)
                    {
                        _quizRepository.DeleteAns(answers);
                    }
                    if (!string.IsNullOrEmpty(item.ContentUrl))
                    {
                        _ = _imageService.DeleteImage(item.ContentUrl);
                    }
                }
                _quizRepository.DeleteQuestions(questions);
            }
            _quizRepository.Delete(section);

            var quizViewModel = await _quizParserService.GenerateQuizViewModel(quizId);

            return PartialView("_Section", quizViewModel);
        }


        /// <summary>
        /// Deletes a question. If the question has question parts they are detached from the parent question
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="quizId"></param>
        [HttpPost]
        public async Task<IActionResult> DeleteQuestion(int questionId, int quizId)
        {
            Question question = await _quizRepository.GetQuestionById(questionId);
            var children = await _quizRepository.GetChildQuestions(questionId);

            if (!string.IsNullOrEmpty(question.ContentUrl))
            {
                _ = _imageService.DeleteImage(question.ContentUrl);
            }

            IEnumerable<UserQuestionResponse> responses = await _takeQuizRepository.GetSingleResponseByQuestionId(question.Id);
            if(responses != null)
            {
                foreach (UserQuestionResponse response in responses)
                {
                    response.QuestionId = null;
                    response.AnswerId = null;
                    _takeQuizRepository.Update(response);
                }
            }

            if (children.Any())
            {
                foreach (Question item in question.Children)
                {
                    item.ParentId = question.ParentId;
                    _quizRepository.Update(item);
                }
            }

            IEnumerable<Answer> answers = await _quizRepository.GetAnswersByQuestion(questionId);
            if (answers != null)
            {
                _quizRepository.DeleteAns(answers);
            }
            _quizRepository.Delete(question);

            var quizViewModel = await _quizParserService.GenerateQuizViewModel(quizId);
            return PartialView("_Section", quizViewModel);
        }


        // Generate Modal Popup
        [HttpGet]
        public async Task<IActionResult> ShowDeleteModal(int id)
        {
            Quiz quiz = await _quizRepository.GetQuizById(id);
            return PartialView("_DeleteModalPartial");
        }


        // Generate Modal Popup
        [HttpGet]
        public async Task<IActionResult> ShowEditModal(int id)
        {
            Question question = await _quizRepository.GetQuestionById(id);
            UpdateQuestionViewModel update = new UpdateQuestionViewModel();
            update.Question = question;
            //return PartialView("_EditModalPartial", question);
            return PartialView("_EditModalPartial", update);

        }


        /// <summary>
        /// Updates a questions attributes
        /// </summary>
        /// <param name="updatedQuestion"></param> 
        [HttpPost]
        public async Task<IActionResult> UpdateQuestion(UpdateQuestionViewModel updatedQuestion)
        {
            Question question = await _quizRepository.GetQuestionById(updatedQuestion.Question.Id);
            var section = await _quizRepository.GetSectionById(question.SectionId);

            if (question.QuestionType == QuestionType.GROUP)
            {
                if (updatedQuestion.file != null && updatedQuestion.file.Length > 0)
                {
                    var result = await _imageService.AddImage(updatedQuestion.file);
                    question.ContentUrl = result.Url.ToString();
                }
                question.QuestionTitle = updatedQuestion.Question.QuestionTitle;
            }
            else
            {
                question.QuestionTitle = updatedQuestion.Question.QuestionTitle;
                question.Mark = updatedQuestion.Question.Mark;
                question.NegativeMark = updatedQuestion.Question.NegativeMark;
                question.ErrorMargin = updatedQuestion.Question.ErrorMargin;
            }

            _quizRepository.Update(question);

            return RedirectToAction("Create", new { quizId = section.QuizId });
        }


        /// <summary>
        /// Delete an image from a case study
        /// </summary>
        /// <param name="questionId"></param>
        public async Task<IActionResult> DeleteImage(int questionId)
        {
            Question question = await _quizRepository.GetQuestionById(questionId);
            var section = await _quizRepository.GetSectionById(question.SectionId);

            await _imageService.DeleteImage(question.ContentUrl);
            question.ContentUrl = null;
            _quizRepository.Update(question);

            return RedirectToAction("Create", new { quizId = section.QuizId });
        }


        // Generate Modal Popup
        [HttpGet]
        public async Task<IActionResult> ShowEditSectionModal(int id)
        {
            Section section = await _quizRepository.GetSectionById(id);
            return PartialView("_EditSectionModalPartial", section);
        }


        /// <summary>
        /// Updates a sections attributes
        /// </summary>
        /// <param name="updatedSection"></param>
        [HttpPost]
        public async Task<IActionResult> UpdateSection(Section updatedSection)
        {
            Section section = await _quizRepository.GetSectionById(updatedSection.Id);
            if (updatedSection.SectionName != null)
            {
                section.SectionName = updatedSection.SectionName;
                section.RequiredQuestions = updatedSection.RequiredQuestions;

                _quizRepository.Update(section);
            }

            var quizViewModel = await _quizParserService.GenerateQuizViewModel(section.QuizId);
            return PartialView("_Section", quizViewModel);
        }


        // Generate Modal Popup
        [HttpGet]
        public async Task<IActionResult> ShowAddQuestionModal(int id, int sectionId)
        {
            var addQuestionViewModel = new AddQuestionViewModel();
            addQuestionViewModel.Quiz = await _quizRepository.GetQuizById(id);
            addQuestionViewModel.SectionId = sectionId;
            addQuestionViewModel.QuestionTypeList = await _quizParserService.GenerateQuestionTypes();
            return PartialView("_AddQuestionModalPartial", addQuestionViewModel);
        }


        /// <summary>
        /// Adds a new question to the quiz. Params are List<> due to multiple complex objects posted with AJAX
        /// </summary>
        /// <param name="questionBody"></param>
        /// <param name="answers"></param>
        /// <param name="answersCheck"></param>
        [ActionName("AddQuestion")]
        [HttpPost]
        public async Task<IActionResult> AddQuestion(List<string> questionBody, List<string> answers, List<string> answersCheck)
        {
            if(questionBody[0] != null)
            {
                Question question = new Question()
                {
                    SectionId = Int32.Parse(questionBody[5]),
                    QuestionTitle = questionBody[0],
                    QuestionType = Enum.Parse<QuestionType>(questionBody[6]),
                };
                if (questionBody[1] != null)
                {
                    question.Mark = Int32.Parse(questionBody[1]);
                }
                if (questionBody[2] != null)
                {
                    question.ErrorMargin = double.Parse(questionBody[2]);
                }
                if (questionBody[3] != null)
                {
                    question.NegativeMark = double.Parse(questionBody[3]);
                }

                _quizRepository.Add(question);

                for (int i = 0; i < answers.Count; i++)
                {
                    Answer answer = new Answer()
                    {
                        QuestionAnswer = answers[i],
                        QuestionId = question.Id,
                    };
                    if (answersCheck[i].Equals("true"))
                    {
                        answer.isCorrect = true;
                    }
                    _quizRepository.Add(answer);
                }
            }

            var quizViewModel = await _quizParserService.GenerateQuizViewModel(Int32.Parse(questionBody[4]));
            return PartialView("_Section", quizViewModel);
        }

    }
}
