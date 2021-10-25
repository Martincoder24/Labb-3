using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_3.Models
{
    public class Question
    {
        private string _statement;

        public string Statement
        {
            get => _statement; 
            
        }

        private string[] _answers;

        public string[] Answers
        {
            get => _answers; 
            
        }

        private readonly int _correctAnswer;

        public int CorrectAnswer
        {
            get => _correctAnswer; 
            
        }
        private string _theme;

        public string Theme
        {
            get => _theme;
            
        }


        public Question(string statement, string[] answers, int correctAnswer, string theme)
        {
            _statement = statement;
            _answers = answers;
            _correctAnswer = correctAnswer;
            _theme = theme;
        }

    }
}
