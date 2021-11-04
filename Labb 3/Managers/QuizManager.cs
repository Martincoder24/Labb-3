using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Labb_3.Models;


namespace Labb_3.Managers
{
    public class QuizManager
    {
        private readonly string _filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        //Håller reda på alla quiz-listor
        private ObservableCollection<Quiz> _quizzes = new ObservableCollection<Quiz>();

        public ObservableCollection<Quiz> Quizzes
        {
            get => _quizzes; 
            set => _quizzes = value; 
        }
        //Spara ner alla Quiz i mitt program till Json-fil
        public async Task SaveQuizzes()
        {
            string fileName = "Quizzes.json";
            using FileStream createStream = File.Create(Path.Combine(_filePath, fileName));
            JsonSerializerOptions options = new() { WriteIndented = true };
            await JsonSerializer.SerializeAsync(createStream, Quizzes, options);
            await createStream.DisposeAsync();
        }
        //Ifall det redan finns skapta quiz så hämtar den dom först
        public async Task LoadQuizzes()
        {
            string fileName = "Quizzes.json";
            if (File.Exists(Path.Combine(_filePath, fileName)))
            {
                using FileStream openStream = File.OpenRead(Path.Combine(_filePath, fileName));
                Quizzes = await JsonSerializer.DeserializeAsync<ObservableCollection<Quiz>>(openStream);
            }
            else
            {
                AddDefaultQuiz();
            }
                
        }
        private void AddDefaultQuiz()
        {
            var questionList = new ObservableCollection<Question>();
            //Här addar jag frågorna
            questionList.Add(new Question("Who is this singer?", new string[3] { "Madonna", "Beyonce", "Britney Spears" }, 0, "music", null));
            questionList.Add(new Question("What is the capital city in Belgium?", new string[3] { "Brussel", "Paris", "Amsterdam" }, 0, "geography", null));
            questionList.Add(new Question("In which country can I find Mount Fiji?", new string[3] { "France", "Japan", "China" }, 1, "geography", null));
            questionList.Add(new Question("Which singer/group is responsible for the hit song - Somebody To You?", new string[3] { "Adele", "Nickelback", "Banners" }, 2, "music", null));
            questionList.Add(new Question("What is the male version of a cow?", new string[3] { "Man Cow", "Bill", "Bull" }, 2, "animals", null));
            Quizzes.Add(new Quiz(questionList, "DefaultQuiz"));
            foreach (var quiz in Quizzes)
            {
                foreach (var question in questionList)
                {
                    if (!quiz.Themes.Contains(question.Theme))
                    {
                        quiz.Themes.Add(question.Theme);
                    }
                }
            }
            SaveQuizzes();
        }
    }
}
