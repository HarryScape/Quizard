﻿using Microsoft.AspNetCore.Mvc;
using Quizard.Interfaces;
using Quizard.Models;
using Quizard.ViewModels;

namespace Quizard.Controllers
{
    public class TakeQuizController : Controller
    {
        private readonly IQuizRepository _quizRepository;
        private readonly ITakeQuizRepository _takeQuizRepository;
        private readonly IQuizParserService _quizParserService;
        private readonly IHttpContextAccessor _contextAccessor;

        public TakeQuizController(IQuizRepository quizRepository, IQuizParserService quizParserService,
            IHttpContextAccessor contextAccessor, ITakeQuizRepository takeQuizRepository)
        {
            _quizRepository = quizRepository;
            _quizParserService = quizParserService;
            _contextAccessor = contextAccessor;
            _takeQuizRepository = takeQuizRepository;
        }


        public async Task<IActionResult> Index(int quizId)
        {
            var quizViewModel = await _quizParserService.GenerateQuizViewModel(quizId);

            return View(quizViewModel);
        }

        public async Task<IActionResult> Completed(int quizId)
        {
            var quizViewModel = await _quizParserService.GenerateQuizViewModel(quizId);

            return View(quizViewModel);
        }


        /// <summary>
        /// Passes a quiz to the view and adds a new quiz attempt to the DB
        /// </summary>
        /// <param name="quizId"></param>
        public async Task<IActionResult> BeginQuiz(int quizId)
        {
            var currentUser = _contextAccessor.HttpContext.User.GetUserId();
            var userQuizAttempt = new UserQuizAttempt()
            {
                QuizId = quizId,
                UserId = currentUser,
                TimeStarted = DateTime.Now,
                IsMarked = false,
                ReleaseFeedback = false,
            };
            _takeQuizRepository.Add(userQuizAttempt);

            TakeQuizViewModel takeQuizViewModel = await _quizParserService.GenerateTakeQuizViewModel(quizId);
            takeQuizViewModel.AttemptId = userQuizAttempt.Id;
            takeQuizViewModel.QuestionResponses = await _takeQuizRepository.GetResponsesbyAttemptId(userQuizAttempt.Id);

            return PartialView("_TakeSectionPartial", takeQuizViewModel);
        }


        // Navigate sections
        public async Task<IActionResult> NextSectionNavigation(int quizId, int index, int attemptId)
        {
            TakeQuizViewModel takeQuizViewModel = await _quizParserService.GenerateTakeQuizViewModel(quizId);
            List<Section> sections = (List<Section>)takeQuizViewModel.Sections;
            if (index < sections.Count - 1)
            {
                index++;
                takeQuizViewModel.Section = sections[index];
            }
            takeQuizViewModel.AttemptId = attemptId;
            takeQuizViewModel.QuestionResponses = await _takeQuizRepository.GetResponsesbyAttemptId(attemptId);

            return PartialView("_TakeSectionPartial", takeQuizViewModel);
        }


        // Navigate sections
        public async Task<IActionResult> PreviousSectionNavigation(int quizId, int index, int attemptId)
        {
            TakeQuizViewModel takeQuizViewModel = await _quizParserService.GenerateTakeQuizViewModel(quizId);
            List<Section> sections = (List<Section>)takeQuizViewModel.Sections;
            if (index > 0)
            {
                index--;
                takeQuizViewModel.Section = sections[index];
            }
            takeQuizViewModel.AttemptId = attemptId;
            takeQuizViewModel.QuestionResponses = await _takeQuizRepository.GetResponsesbyAttemptId(attemptId);


            return PartialView("_TakeSectionPartial", takeQuizViewModel);
        }


        /// <summary>
        /// Saves question responses. Params are List<> due to AJAX post unable to pass multiple complex objects
        /// </summary>
        /// <param name="questionTextIdList"></param>
        /// <param name="textResponseList"></param>
        /// <param name="questionCheckboxIdList"></param>
        /// <param name="ansText"></param>
        /// <param name="ansCorrect"></param>
        /// <param name="answerIdList"></param>
        /// <param name="attemptId"></param>
        public async Task<IActionResult> SubmitResponse(List<string> questionTextIdList, List<string> textResponseList,
            List<string> questionCheckboxIdList, List<string> ansText, List<string> ansCorrect, List<string> answerIdList, int attemptId)
        {
            if (textResponseList.Any())
            {
                for (int i = 0; i < textResponseList.Count; i++)
                {
                    int questionId = Convert.ToInt32(questionTextIdList[i]);
                    UserQuestionResponse response = await _takeQuizRepository.GetSingleResponseByQuestion(attemptId, questionId);
                    if (response == null)
                    {
                        UserQuestionResponse newResponse = new UserQuestionResponse()
                        {
                            QuestionId = questionId,
                            UserQuizAttemptId = Convert.ToInt32(attemptId),
                            AnswerResponse = textResponseList[i],
                        };
                        _takeQuizRepository.Add(newResponse);
                    }
                    else
                    {
                        response.AnswerResponse = textResponseList[i];
                        _takeQuizRepository.Update(response);
                    }
                }
            }


            // Clear Checkbox answers
            for (int i = 0; i < ansText.Count; i++)
            {
                int questionId = Convert.ToInt32(questionCheckboxIdList[i]);
                var responses = await _takeQuizRepository.GetResponsesbyQuestion(attemptId, questionId);
                foreach (var response in responses)
                {
                    _takeQuizRepository.Delete(response);
                }
            }


            // Adds new checkbox answers
            if (ansText.Any())
            {
                for (int i = 0; i < ansCorrect.Count; i++)
                {
                    int questionId = Convert.ToInt32(questionCheckboxIdList[i]);

                    if (ansCorrect[i].Equals("true"))
                    {
                        UserQuestionResponse newResponse = new UserQuestionResponse()
                        {
                            QuestionId = Convert.ToInt32(questionCheckboxIdList[i]),
                            UserQuizAttemptId = Convert.ToInt32(attemptId),
                            AnswerResponse = ansText[i],
                            AnswerId = Convert.ToInt32(answerIdList[i])
                        };
                        _takeQuizRepository.Add(newResponse);
                    }
                }
            }
            return null;
        }


        public async Task<IActionResult> CompleteQuiz(int attemptId)
        {
            UserQuizAttempt attempt = await _takeQuizRepository.GetAttemptById(attemptId);
            attempt.TimeCompleted = DateTime.Now;
            _takeQuizRepository.Update(attempt);

            return RedirectToAction("Completed", "TakeQuiz");
        }


    }
}


