using CsvHelper;
using System.IO;
using System.Globalization;
using CsvHelper.Configuration;
using System.Linq;
using Quizard.Models;
using Quizard.Mappers;
using Quizard.Interfaces;

namespace Quizard.Services
{
    public class QuizParserService : IQuizParserService
    {


        // need isValidQustion bools. 
        // public async Task<bool> isValidUpload()
        // public async Task<bool> isValidQuiz()
        // .txt, xml,.zip. answers[] <= 100, must have correct questiontype.











        // input file location
        // list of correct answers
        // list of wrong answers
        // Q type
        // Q title
        // TODO: ParseXML()


        public async Task<bool> ParseCSV()
        {
            // file location
            using (var streamReader = new StreamReader("Data/UploadDirectory/HarrysTestQuiz.txt"))
            {
                // constructs a new reader. InvariantCulture helps with number and date formatting.
                // passes in settings to configure the csvreader
                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = "\t",
                    HasHeaderRecord = false,
                    
            };

                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                    // We can now read the csv into a collection of dynamic objects
                {
                    csvReader.Context.RegisterClassMap<QuestionMap>();
                    csvReader.Context.RegisterClassMap<AnswerMap>();
                    csvReader.Context.RegisterClassMap<QuestionMap>();
                    // Quiz may be redundant. Move to the SQL function. 
                    var Quizzes = csvReader.GetRecords<Quiz>().ToList();
                    var Questions = csvReader.GetRecords<Question>().ToList();
                    var Answers = csvReader.GetRecords<Answer>().ToList();
                }
            }
            return true;
        }


    }
}
