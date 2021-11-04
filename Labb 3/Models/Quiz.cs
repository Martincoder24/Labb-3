using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_3.Models
{
    public class Quiz
    {
        private ICollection<Question> _questions;

        public ICollection<Question> Questions
        {
            get =>  _questions;
            set => _questions = value;
        }
        private string _title;

        public string Title
        {
            get => _title;
            set => _title = value;
            
        }

        private ObservableCollection<string> _themes;

        public ObservableCollection<string> Themes
        {
            get => _themes;
            set => _themes = value;
        }

        public Quiz(ICollection<Question> questions, string title)
        {
            _questions = questions;
            _title = title;
            _themes = new ObservableCollection<string>();
        }

        //Todo Gör denna i min PlayView
        public Question GetRandomQuestion()
        {
            //Ger mig indexet på antal frågor i mitt quiz
            int maxIndex = Questions.Count();
            //randomgeneratorn ska ge mig ett index i quizzet
            var randQ = new Random();
            var randIndex = randQ.Next(0, maxIndex);
            List<Question> questionList = new List<Question>(Questions);

            return questionList[randIndex];
        }

        //Här lägger jag in en ny fråga i AddQuestionCommandet
        public void AddQuestion(string statement, string[] answer, int correctAnswer, string theme)
        {
            var question = new Question(statement, answer, correctAnswer, theme);
            Questions.Add(question);

        }
        //Jag anropar denna metoden för att Ta bort indexet av frågan jag skickar in.
        public void RemoveQuestion(int index)
        {

            Questions.ToList().RemoveAt(index);
        }

       
    }
}
