using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Labb_3.Managers;
using Labb_3.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;

namespace Labb_3.ViewModels
{
    public class EditViewModel : ObservableObject
    {
        private readonly NavigationManager _navigationManager;

        private readonly QuizManager _quizManager;


        #region Properties


        //Detta binds till ifrån min ComboBox från vyn
        private Quiz _currentQuiz;

        public Quiz CurrentQuiz
        {
            get => _currentQuiz;
            set
            {
                SetProperty(ref _currentQuiz, value);
                AddQuestionCommand.NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(CurrentQuiz.Questions));
                QuizQuestions = new ObservableCollection<Question>(CurrentQuiz.Questions);
            }
        }

        private ObservableCollection<Question> _quizQuestions = new ObservableCollection<Question>();

        public ObservableCollection<Question> QuizQuestions
        {
            get => _quizQuestions;
            set => SetProperty(ref _quizQuestions , value);
        }

        private Question _selectedQuestion;

        public Question SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                SetProperty(ref _selectedQuestion, value);
                RemoveQuestionCommand.NotifyCanExecuteChanged();
                //Set alla bindningsfält till SelectedQuestion
                if (SelectedQuestion != null)
                {
                    Statement = SelectedQuestion.Statement;
                    Answer1 = SelectedQuestion.Answers[0];
                    Answer2 = SelectedQuestion.Answers[1];
                    Answer3 = SelectedQuestion.Answers[2];
                    CorrectAnswer = SelectedQuestion.CorrectAnswer;
                    Theme = SelectedQuestion.Theme;
                    ImagePath = SelectedQuestion.ImagePath;
                }
                

            }
        }

        public ObservableCollection<Quiz> ListOfQuizzes => new(_quizManager.Quizzes);

        private string _imagePath;

        public string ImagePath
        {
            get => _imagePath;
            set => SetProperty(ref _imagePath, value);
        }

        //Todo lägg in alla properties för SelectedQuestion

        //statement
        private string _statement;

        public string Statement
        {
            get => _statement;
            set => SetProperty(ref _statement, value);
        }

        //answers
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

        private ObservableCollection<string> _allAnswers = new ObservableCollection<string>() { "", "", "" };

        public ObservableCollection<string> AllAnswers
        {
            get => _allAnswers;
            set => SetProperty(ref _allAnswers , value);
        }
      //theme
        private string _theme;

        public string Theme
        {
            get => _theme;
            set => SetProperty(ref _theme, value);
        }
        //correctAnswer
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
        #endregion

        public RelayCommand EditQuestionCommand { get; }
        public RelayCommand AddQuestionCommand { get; }
        public RelayCommand RemoveQuestionCommand { get; }
        public RelayCommand ReturnCommand { get; }
        public RelayCommand AddImageCommand { get; }

        public EditViewModel(QuizManager quizManager, NavigationManager navigationManager)
        {
            _quizManager = quizManager;
            _navigationManager = navigationManager;
            EditQuestionCommand = new RelayCommand(EditQuestion, CanEditQuestion);
            AddQuestionCommand = new RelayCommand(AddQuestion, CanAddQuestion);
            RemoveQuestionCommand = new RelayCommand(RemoveQuestion, CanRemoveQuestion);
            AddImageCommand = new RelayCommand(AddImageToQuestion);
            ReturnCommand = new RelayCommand(ReturnButton);
            //PropertyChanged += OnViewModelPropertyChanged;

        }
        //Edit-knappens Command-metoder
        #region EditCommand metoder
        private void EditQuestion()
        {
            SelectedQuestion.Statement = Statement;
            SelectedQuestion.Answers[0] = Answer1;
            SelectedQuestion.Answers[1] = Answer2;
            SelectedQuestion.Answers[2] = Answer3;
            SelectedQuestion.CorrectAnswer = CorrectAnswer;
            SelectedQuestion.Theme = Theme;
            SelectedQuestion.ImagePath = ImagePath;

            QuizQuestions = new ObservableCollection<Question>(CurrentQuiz.Questions);
            _quizManager.SaveQuizzes();
        }
        
        private bool CanEditQuestion()
        {
            
            return true;

        }
        #endregion

        //Add-knappens Command-metoder
        #region AddCommand metoder
        private void AddQuestion()
        {
            //Todo Fixa alla properties för så att man kan adda en fråga
            //Ifall currentquiz inte finns i _quizManager.Quizzes lägg tillquizzet först
            if (_quizManager.Quizzes.Contains(CurrentQuiz))
            {
                _quizManager.Quizzes.Add(CurrentQuiz);
            }
            var answers = new string[3] { Answer1, Answer2, Answer3 };
            CurrentQuiz.AddQuestion(Statement, answers, CorrectAnswer, Theme, ImagePath);

            var tempQuiz = CurrentQuiz;
            CurrentQuiz = tempQuiz;

            QuizQuestions = new ObservableCollection<Question>(CurrentQuiz.Questions);
            _quizManager.SaveQuizzes();
            AddQuestionCommand.NotifyCanExecuteChanged();
        }
        //Detta kollar ifall ett question-statement redan finns i det befintliga Quizzet.
        private bool CanAddQuestion()
        {
            if (CurrentQuiz != null)
            {

                if (CurrentQuiz.Questions.FirstOrDefault(q => q.Statement == Statement) == null)
                {
                    return !string.IsNullOrEmpty(Statement);
                }
            }

            return false;


        }
        #endregion


        #region Remove-knappens command-metoder
        //Ta reda på Frågans index i currentQuizz. skicka in det i metoden som finns i Quizklassen.
        private void RemoveQuestion()
        {
            //Tänk att du ska ha indexet på currentQuiz från _quizmanager.Quizzes

            var currentQuestionIndex = CurrentQuiz.Questions.ToList().IndexOf(SelectedQuestion);

            CurrentQuiz.RemoveQuestion(currentQuestionIndex);
            if (QuizQuestions != null)
            {
                QuizQuestions = new ObservableCollection<Question>(CurrentQuiz.Questions);
            }
            //Med null så försvinner den från ListViewen.
            //SelectedQuestion = null;
            
            _quizManager.SaveQuizzes();
        }

        private bool CanRemoveQuestion()
        {
            //if (CurrentQuiz != null)
            //{
            //    if (CurrentQuiz.Questions.ToList().Select(q => q = SelectedQuestion) == null)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //else
            //{
            //    return false;
            //}
            if (SelectedQuestion != null)
            {
                return true;
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
        public void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedQuestion.Statement) || e.PropertyName == nameof(SelectedQuestion.Answers)
               || e.PropertyName == nameof(SelectedQuestion.Theme) || e.PropertyName == nameof(SelectedQuestion.CorrectAnswer) ||
               e.PropertyName == nameof(SelectedQuestion) || e.PropertyName == nameof(CurrentQuiz))
            {
                // EditQuestionCommand.NotifyCanExecuteChanged();

            }
        }
    }
}
