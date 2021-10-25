using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb_3.Models;

namespace Labb_3.Managers
{
    public class QuizManager
    {
        //Håller reda på alla quiz-listor
        private List<Quiz> _quizzes = new List<Quiz>();

        public List<Quiz> Quizzes
        {
            get => _quizzes; 
            set => _quizzes = value; 
        }
        private List<Question> _questions = new List<Question>();

        public List<Question> Questions
        {
            get => _questions; 
            set => _questions= value; 
        }

    }
}
