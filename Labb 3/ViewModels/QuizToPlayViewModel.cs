using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Labb_3.Managers;
using Labb_3.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Labb_3.ViewModels
{
    public class QuizToPlayViewModel : ObservableObject
    {
        private QuizManager _quizManager;
        private NavigationManager _navigationManager;


        private Quiz _selectedQuiz;

        public Quiz SelectedQuiz
        {
            get => _selectedQuiz;
            set
            {
                SetProperty(ref _selectedQuiz, value);
                LetsPlayCommand.NotifyCanExecuteChanged();

            }
        }

        private string _selectedTheme;

        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                SetProperty(ref _selectedTheme, value);
                AddThemeCommand.NotifyCanExecuteChanged();
                RemoveThemeCommand.NotifyCanExecuteChanged();
            }

        }
        //För att hämta min aktiva lista av Quiz


        public ObservableCollection<Quiz> ListOfQuizzes => new(_quizManager.Quizzes);


        private ObservableCollection<string> _themesOfChoice;

        public ObservableCollection<string> ThemesOfChoice
        {
            get => _themesOfChoice;
            set => SetProperty(ref _themesOfChoice, value);
        }

        public RelayCommand ReturnCommand { get; }
        public RelayCommand LetsPlayCommand { get; }
        public RelayCommand AddThemeCommand { get; }
        public RelayCommand RemoveThemeCommand { get; }
        public QuizToPlayViewModel(QuizManager quizManager, NavigationManager navigationManager)
        {
            _quizManager = quizManager;
            _navigationManager = navigationManager;
            _themesOfChoice = new ObservableCollection<string>();
            ReturnCommand = new RelayCommand(ReturnButton);
            LetsPlayCommand = new RelayCommand(LetsPlay, CanPressPlay);
            AddThemeCommand = new RelayCommand(AddTheme, CanAddTheme);
            RemoveThemeCommand = new RelayCommand(RemoveTheme, CanRemoveTheme);
        }

        private void ReturnButton()
        {
            _navigationManager.CurrentViewModel = new StartViewModel(_quizManager, _navigationManager);
        }

        #region Add-Themes-Command-Methods
        private void AddTheme()
        {
            if (!_themesOfChoice.Contains(SelectedTheme))
            {
                _themesOfChoice.Add(SelectedTheme);
            }

        }
        //Kolla om Något teme är adderat till min temalista
        private bool CanAddTheme()
        {

            if (_themesOfChoice.FirstOrDefault(q => q == SelectedTheme) == null)
            {
                return !string.IsNullOrEmpty(SelectedTheme);
            }
            else
            {
                return false;
            }
        }
        #endregion
        private void RemoveTheme()
        {
            if (_themesOfChoice != null)
            {
                _themesOfChoice.Remove(SelectedTheme);
            }

        }
        //Jag ska inte kunna klicka på denna ifall det inte finns nån quiz att remove eller när jag har ett quiz i comboboxen som inte finns i _themeOfChoice
        private bool CanRemoveTheme()
        {

            if (SelectedTheme == null)
            {
                return false;
            }
            else
            {
                return true;
            }


        }
        //Här skapar jag ett quiz som har sållat på valda teman.
        private void LetsPlay()
        {
            //Kontrollerar varje fråga om det matchar nåt tema i _themesOfChoice och om det inte matchar så ta bort dom til dett uppdaterade quizzet 

            var questionList = SelectedQuiz.Questions.ToList().Where(q => !_themesOfChoice.Any(t => t == q.Theme));
            var updatedQuiz = new Quiz(questionList.ToList(), SelectedQuiz.Title);

            _navigationManager.CurrentViewModel = new PlayModeViewModel(_quizManager, _navigationManager, updatedQuiz);
        }

        private bool CanPressPlay()
        {
            if (SelectedQuiz != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
