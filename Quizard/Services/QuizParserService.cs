using System.IO;
using System.Globalization;
using System.Linq;
using Quizard.Models;
using Quizard.Interfaces;
using Quizard.Data.Enum;

namespace Quizard.Services
{
    public class QuizParserService : IQuizParserService
    {


        // need isValidQustion bools. 
        // public async Task<bool> isValidUpload()
        // public async Task<bool> isValidQuiz()
        // .txt, .csv, xml,.zip. answers[] <= 100, must have correct questiontype.

        public async Task<string> GetQuizLMS(IFormFile file)
        {
            string lms = "";

            using (StreamReader fileReader = new StreamReader(file.OpenReadStream()))
            {
                string line = fileReader.ReadLine();

                if (char.IsDigit(line[0]))
                {
                    lms = "Canvas";
                }
                else if (line.StartsWith("::"))
                {
                    lms = "Moodle";
                }
                else if (char.IsUpper(line[0]) && char.IsUpper(line[1])){
                    lms = "Blackboard";
                }
            }
            return lms;
        }


    }
}
