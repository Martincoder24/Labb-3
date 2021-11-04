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
            }
        }

        private Question _selectedQuestion;

        public Question SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                SetProperty(ref _selectedQuestion, value);
            }
        }
        public ObservableCollection<Quiz> ListOfQuizzes => new(_quizManager.Quizzes);

        
       // public ObservableCollection<Question> CurrentQuizQuestions => new(CurrentQuiz.Questions);

        #endregion

        public RelayCommand EditQuestionCommand { get; }
        public RelayCommand AddQuestionCommand { get; }
        public RelayCommand RemoveQuestionCommand { get; }
        public RelayCommand ReturnCommand { get; }

        public EditViewModel(QuizManager quizManager, NavigationManager navigationManager)
        {
            _quizManager = quizManager;
            _navigationManager = navigationManager;
            EditQuestionCommand = new RelayCommand(EditQuestion, CanEditQuestion);
            AddQuestionCommand = new RelayCommand(AddQuestion, CanAddQuestion);
            RemoveQuestionCommand = new RelayCommand(RemoveQuestion, CanRemoveQuestion);
            ReturnCommand = new RelayCommand(ReturnButton);
            //PropertyChanged += OnViewModelPropertyChanged;

        }
        //Edit-knappens Command-metoder
        #region EditCommand metoder
        private void EditQuestion()
        {

            _selectedQuestion = new Question(SelectedQuestion.Statement, SelectedQuestion.Answers, SelectedQuestion.CorrectAnswer, SelectedQuestion.Theme);
            _quizManager.SaveQuizzes();
            EditQuestionCommand.NotifyCanExecuteChanged();
        }
        //När ska knappen vara tryckbar?
        //När man har ändrat en property kopplat till den valda frågan dvs. statement, ett svarsalternativ eller temat && Statement != statement av nån annan fråga i det kopplade Quizzet.
        private bool CanEditQuestion()
        {
            if (CurrentQuiz != null)
            {
                if (CurrentQuiz.Questions.FirstOrDefault(q => q.Statement == SelectedQuestion.Statement) == null)
                {
                    return !string.IsNullOrEmpty(SelectedQuestion.Statement);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        #endregion

        //Add-knappens Command-metoder
        #region AddCommand metoder
        private void AddQuestion()
        {
            CurrentQuiz.AddQuestion(SelectedQuestion.Statement, SelectedQuestion.Answers, SelectedQuestion.CorrectAnswer, SelectedQuestion.Theme);
            _quizManager.SaveQuizzes();
            AddQuestionCommand.NotifyCanExecuteChanged();
        }
        //Detta kollar ifall ett question-statement redan finns i det befintliga Quizzet.
        private bool CanAddQuestion()
        {
            if (CurrentQuiz != null)
            {

                if (CurrentQuiz.Questions.FirstOrDefault(q => q.Statement == SelectedQuestion.Statement) == null)
                {
                    return !string.IsNullOrEmpty(SelectedQuestion.Statement);
                }
                else
                {
                    return false;
                }
            }

            else
            {
                return false;
            }

        }
        #endregion


        #region Remove-knappens command-metoder
        //Ta reda på Frågans index i currentQuizz. skicka in det i metoden som finns i Quizklassen.
        private void RemoveQuestion()
        {
            var currentQuestionIndex = CurrentQuiz.Questions.ToList().FindIndex(q => q == SelectedQuestion);
            CurrentQuiz.RemoveQuestion(currentQuestionIndex);
            _quizManager.SaveQuizzes();

            //Uppdaterar min property
            var tempAdd = CurrentQuiz;
            CurrentQuiz = tempAdd;
        }

        private bool CanRemoveQuestion()
        {
            if (CurrentQuiz != null)
            {
                if (CurrentQuiz.Questions.ToList().Select(q => q = SelectedQuestion) == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        #endregion

        //Todo Lägg till Return-knappens commands
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
