using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private NavigationManager _navigationManager;


        #region Propeties
        //Skapa en binding till quiz-namnet.
        private string _quizName;

        public string QuizName
        {
            get => _quizName;
            set
            {
                SetProperty(ref _quizName, value);
                
            } 
        }
        private string _statement;

        public string Statement
        {
            get => _statement;
            set => SetProperty(ref _statement, value);
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
            set => SetProperty(ref _theme, value);
        }
        private int _correctAnswer;

        public int CorrectAnswer
        {
            get => _correctAnswer;
            set => SetProperty(ref _correctAnswer, value);
        }
        private Quiz _createdQuiz = new Quiz(new List<Question>(), "TEMP");

        public Quiz CreatedQuiz
        {
            get => _createdQuiz;
            set
            {
                SetProperty(ref _createdQuiz, value);
                if (_currentQuizQuestions != null && _createdQuiz != null)
                {
                    _currentQuizQuestions.Clear();
                    foreach (var question in CreatedQuiz.Questions)
                    {
                        _currentQuizQuestions.Add(question);
                    }
                }
            } 
        }
        private ObservableCollection<Question> _currentQuizQuestions = new();

        public ObservableCollection<Question> CurrentQuizQuestions
        {
            get => _currentQuizQuestions;
            set => SetProperty(ref _currentQuizQuestions, value);
        }


        private ObservableCollection<string> _allAnswers = new ObservableCollection<string>() { "", "", "" };

        public ObservableCollection<string> AllAnswers
        {
            get => _allAnswers;
            set => OnPropertyChanged(nameof(AllAnswers));
        } 
        #endregion

        //Skapa en Command-bindning för "create quiz" knappen som skall skapa en quiz-list<Questions>
        public RelayCommand AddQuizCommand { get;}
        public RelayCommand AddQuestionCommand { get; }
        
        //Skapa ett nytt quiz som skall läggas till i min QuizManager
        public void CreateQuiz()
        {
            if (_quizName != null)
            {
                CreatedQuiz = new Quiz(new List<Question>(), _quizName);
                CreatedQuiz.Title = QuizName;
                _quizManager.Quizzes.Add(CreatedQuiz);
                //Ändrar så att när man skrivit in ett quiz o sparat så måste man meddela till RelayCommandet
                AddQuizCommand.NotifyCanExecuteChanged();

            }
        }
        //Lägg in en sats här för att kolla om en nuvarande lista heter likadan.
        public bool CanCreateQuiz()
        {

            if (_quizManager.Quizzes.FirstOrDefault(q => q.Title == QuizName) == null)
            {
                return !string.IsNullOrEmpty(QuizName);
            }
            else
            {
                return false;
            }

        }
        //Lägga till en fråga i det befintliga quizzet
        //_correctAnswer ska vara en selected item i min lista med allAnswers
        public void AddQuestionToQuiz()
        {
            var answers = new[] {_answer1, _answer2, _answer3};
            _question = new Question(_statement, answers, _correctAnswer, _theme);
            CreatedQuiz.Questions.Add(_question);

            //Uppdaterar min property
            var tempAdd = CreatedQuiz;
            CreatedQuiz = tempAdd;
        }
        //Todo Sätt en if-sats som kollar om alla fält för en question är ifyllda d.v.s != null eller samma som en befintlig fråga
        public bool CanAddQuestion()
        {
            if (_quizManager.Quizzes.FirstOrDefault(q=>q.Questions == CurrentQuizQuestions) == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CreateViewModel(QuizManager quizManager, NavigationManager navigationManager)
        {
            _quizManager = quizManager;
            AddQuizCommand = new(CreateQuiz, CanCreateQuiz);
            AddQuestionCommand = new (AddQuestionToQuiz, CanAddQuestion);
            PropertyChanged += OnViewModelPropertyChanged;
        }

        public void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CreatedQuiz) || e.PropertyName == nameof(QuizName))
            {
                AddQuizCommand.NotifyCanExecuteChanged();
                AddQuestionCommand.NotifyCanExecuteChanged();
            }
        }
        //När Quizet är skapat så skall submit-knappen bli utgråad och namnet på quiz-listan skall stå kvar.


        //Skapa en bindning till frågan och dess svar. Samt att markera vilket alternativ som är rätt svar.
        //Lägg sedan till en bindning för tema på frågan också
        //Skapa ett command  för en knapp som skapar frågan m.a.p bindningen till frågor och svar.
        //Vid en skapad fråga så poppar det upp en messagebox som meddelar att en fråga är tillagd.

        //Skapa en property med Theme, answers array, correct answer ska ju bindas till ett index av arrayen 0,1 eller 2.

    }
}
