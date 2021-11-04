using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Labb_3.Models
{
    public class Question
    {
        private string _statement;

        public string Statement
        {
            get => _statement;
            set => _statement = value;
        }

        private string[] _answers;

        public string[] Answers
        {
            get => _answers;
            set => _answers = value;
        }

        private int _correctAnswer;

        public int CorrectAnswer
        {
            get => _correctAnswer;
            set => _correctAnswer = value;
        }
        private string _theme;

        public string Theme
        {
            get => _theme;
            set => _theme = value;
        }

        public bool IsAsked { get; set; }

        //Todo Gör en image property som skall länkas med varje Question
        private string _imagePath;

        public string ImagePath
        {
            get => _imagePath;
            set => _imagePath = value;
        }
    
        
        public Question(string statement, string[] answers, int correctAnswer, string theme, string imagePath)
        {
            _statement = statement;
            _answers = answers;
            _correctAnswer = correctAnswer;
            _theme = theme;
            _imagePath = imagePath;
        }

        
    }
}
