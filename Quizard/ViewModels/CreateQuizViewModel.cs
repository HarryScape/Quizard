﻿using Quizard.Data.Enum;
using Quizard.Models;

namespace Quizard.ViewModels
{
    public class CreateQuizViewModel
    {
        public Quiz Quiz { get; set; }
        public IEnumerable<Section> Sections { get; set; }
        public string SectionName { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public IEnumerable<Question> ParentQuestions { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
        public QuestionType QuestionType { get; set; }

    }
}

