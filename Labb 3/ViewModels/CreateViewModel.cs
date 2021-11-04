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
using Microsoft.Win32;

namespace Labb_3.ViewModels
{
    public class CreateViewModel : ObservableObject
    {
        private readonly QuizManager _quizManager;
        private readonly NavigationManager _navigationManager;


        #region Propeties
        //Skapa en binding till quiz-namnet.
        private string _quizName;

        public string QuizName
        {
            get => _quizName;
            set
            {
                SetProperty(ref _quizName, value);
                AddQuizCommand.NotifyCanExecuteChanged();

            }
        }
        private string _statement;

        public string Statement
        {
            get => _statement;
            set
            {
                SetProperty(ref _statement, value);
                AddQuestionCommand.NotifyCanExecuteChanged();
            }
        }

        private string _imagePath;

        public string ImagePath
        {
            get => _imagePath;
            set => SetProperty(ref _imagePath, value);
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
            set
            {
                SetProperty(ref _correctAnswer, value);
                AddQuestionCommand.NotifyCanExecuteChanged();
            }

        }
        private Quiz _currentQuiz = new Quiz(new ObservableCollection<Question>(), "TEMP");

        public Quiz CurrentQuiz
        {
            get => _currentQuiz;
            set
            {
                SetProperty(ref _currentQuiz, value);
            }
        }

        private ObservableCollection<string> _allAnswers = new ObservableCollection<string>() { "", "", "" };

        public ObservableCollection<string> AllAnswers
        {
            get => _allAnswers;
            set => OnPropertyChanged(nameof(AllAnswers));
        }


        #endregion

        //Skapa en Command-bindning för "create quiz" knappen som skall skapa en quiz-list<Questions>
        public RelayCommand AddQuizCommand { get; }
        public RelayCommand AddQuestionCommand { get; }
        public RelayCommand AddImageCommand { get; }

        public RelayCommand ReturnCommand { get; }
        public CreateViewModel(QuizManager quizManager, NavigationManager navigationManager)
        {
            _quizManager = quizManager;
            _navigationManager = navigationManager;
            AddQuizCommand = new(CreateQuiz, CanCreateQuiz);
            AddQuestionCommand = new(AddQuestionToQuiz, CanAddQuestion);
            AddImageCommand = new RelayCommand(AddImageToQuestion);
            ReturnCommand = new RelayCommand(ReturnButton);
            PropertyChanged += OnViewModelPropertyChanged;
        }

        //Skapa ett nytt quiz som skall läggas till i min QuizManager
        #region CreateQuiz Command-metoder
        private void CreateQuiz()
        {
            if (_quizName != null)
            {
                CurrentQuiz = new Quiz(new ObservableCollection<Question>(), _quizName);
                _quizManager.Quizzes.Add(CurrentQuiz);
                //Här skall jag spara min fil asynkront
                _quizManager.SaveQuizzes();
                //Ändrar så att när man skrivit in ett quiz o sparat så måste man meddela till RelayCommandet
                AddQuizCommand.NotifyCanExecuteChanged();

            }
        }
        //Lägg in en sats här för att kolla om en nuvarande lista heter likadan.
        private bool CanCreateQuiz()
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
        #endregion
        //Lägga till en fråga i det befintliga quizzet
        //_correctAnswer ska vara en selected item i min lista med allAnswers
        #region AddQuestion Command-metoder
        private void AddQuestionToQuiz()
        {
            var answers = new[] { _answer1, _answer2, _answer3 };
            var question = new Question(_statement, answers, _correctAnswer, _theme, _imagePath);
            CurrentQuiz.AddQuestion(Statement, answers, CorrectAnswer, Theme, ImagePath);

            //CreatedQuiz.Questions.Add(question);
            //Addera temat i en lista av teman för quizzet
            if (!CurrentQuiz.Themes.Contains(question.Theme))
            {
                CurrentQuiz.Themes.Add(question.Theme);
            }

            _quizManager.SaveQuizzes();
            //Uppdaterar min property
            var tempAdd = CurrentQuiz;
            CurrentQuiz = tempAdd;
            AddQuestionCommand.NotifyCanExecuteChanged();
        }

        //Denna skall kolla alla statements i CreatedQUizQuestions och jämföra om ett är = Statement
        private bool CanAddQuestion()
        {
            if (CurrentQuiz.Questions.FirstOrDefault(q => q.Statement == Statement) == null)
            {
                return !string.IsNullOrEmpty(Statement);
            }
            return false;


        }
        #endregion

        private void AddImageToQuestion()
        {
            OpenFileDialog openFileImageDialog = new OpenFileDialog();

            openFileImageDialog.Title = "Choose Image";
            openFileImageDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;|All files (*.*)|*.*";
            openFileImageDialog.FilterIndex = 1;

            if (openFileImageDialog.ShowDialog() == true)
            {
                ImagePath = openFileImageDialog.FileName;
            }
        }
        private void ReturnButton()
        {
            _navigationManager.CurrentViewModel = new StartViewModel(_quizManager, _navigationManager);
        }





        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentQuiz) || e.PropertyName == nameof(QuizName)
               )
            {
                AddQuizCommand.NotifyCanExecuteChanged();
            }
            if (e.PropertyName == nameof(CurrentQuiz) || e.PropertyName == nameof(QuizName)
            )
            {
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
