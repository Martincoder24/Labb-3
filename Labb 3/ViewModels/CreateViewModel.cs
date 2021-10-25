using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Labb_3.Managers;
using Labb_3.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Labb_3.ViewModels
{
    public class CreateViewModel: ObservableObject
    {
        private QuizManager _quizManager;
        private Question _question;
        

        //Skapa en binding till quiz-namnet.
        private string _quizName;

        public string QuizName
        {
            get => _quizName;
            set { SetProperty(ref _quizName, value); }
        }
        private string _statement;

        public string Statement
        {
            get => _statement;
            set { SetProperty(ref _statement, value); }
        }

        private string _answer1;

        public string Answer1
        {
            get => _answer1;
            set
            {
                SetProperty(ref _answer1, value);
                AllAnswers[0] = value;
            }
        }
        private string _answer2;

        public string Answer2
        {
            get => _answer2;
            set
            {
                SetProperty(ref _answer2, value);
                AllAnswers[1] = _answer2;
            }
        }
        private string _answer3;

        public string Answer3
        {
            get => _answer3;
            set
            {
                SetProperty(ref _answer3, value);
                AllAnswers[2] = _answer3;
            }
        }
        //Tema för varje fråga så att man kan sålla på det inför playMode
        private string _theme;

        public string Theme
        {
            get => _theme;
            set { SetProperty(ref _theme, value); }
        }
        private int _correctAnswer;

        public int CorrectAnswer
        {
            get => _correctAnswer;
            set { SetProperty(ref _correctAnswer, value); }
        }
        private Quiz _createdQuiz = new Quiz(new List<Question>(), "TEMP");

        public Quiz CreatedQuiz
        {
            get => _createdQuiz;
            set { SetProperty(ref _createdQuiz, value); }
        }

        private ObservableCollection<string> _allAnswers = new ObservableCollection<string>() {"", "", ""};

        public ObservableCollection<string> AllAnswers
        {
            get => _allAnswers;
            set
            {
                OnPropertyChanged(nameof(AllAnswers));
            }
            
        }
        
        //Skapa en Command-bindning för "create quiz" knappen som skall skapa en quiz-list<Questions>
        public RelayCommand AddQuizCommand => new( CreateQuiz);
        public RelayCommand AddQuestionCommand => new(AddQuestionToQuiz);
        
        //Skapa ett nytt quiz som skall läggas till i min QuizManager
        public void CreateQuiz()
        {
            if (_quizName != null)
            {
                CreatedQuiz = new Quiz(new List<Question>(), _quizName);
                _quizManager.Quizzes.Add(CreatedQuiz);

            }
        }
        //Lägga till en fråga i det befintliga quizzet
        //_correctAnswer ska vara en selected item i min lista med allanswers
        public void AddQuestionToQuiz()
        {
            var answers = new[] {_answer1, _answer2, _answer3};
            _question = new Question(_statement, answers, _correctAnswer, _theme);
            CreatedQuiz.Questions.Add(_question);
        }

        public CreateViewModel(QuizManager quizManager, NavigationManager navigationManager)
        {
            _quizManager = quizManager;
        }
        
        //När Quizet är skapat så skall submit-knappen bli utgråad och namnet på quiz-listan skall stå kvar.


        //Skapa en bindning till frågan och dess svar. Samt att markera vilket alternativ som är rätt svar.
        //Lägg sedan till en bindning för tema på frågan också
        //Skapa ett command  för en knapp som skapar frågan m.a.p bindningen till frågor och svar.
        //Vid en skapad fråga så poppar det upp en messagebox som meddelar att en fråga är tillagd.

        //Skapa en property med Theme, answers array, correct answer ska ju bindas till ett index av arrayen 0,1 eller 2.

    }
}
