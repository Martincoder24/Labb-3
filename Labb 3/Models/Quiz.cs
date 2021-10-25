using System;
using System.Collections;
using System.Collections.Generic;
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
        }
        private string _title;

        public string Title
        {
            get => _title;
        }

        public Quiz(ICollection<Question> questions, string title)
        {
            _questions = new List<Question>();
            _title = title;
        }

        //public Question GetRandomQuestion()
        //{
        //    return new Question();
        //}

        public void AddQuestion(string statement, string[] answer, int correctAnswer)
        {

        }

        public void RemoveQuestion(int index)
        {

        }

        public static implicit operator Quiz(List<Question> v)
        {
            throw new NotImplementedException();
        }
    }
}
