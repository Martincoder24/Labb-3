using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb_3.Managers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Labb_3.ViewModels
{
    public class StartViewModel : ObservableObject
    {
        //Todo Basically en meny av knappar med implementerad navigation
        private readonly QuizManager _quizManager;
        private readonly NavigationManager _navigationManager;
        public RelayCommand PlayButtonCommand { get; }
        public RelayCommand CreateButtonCommand { get; }
        public RelayCommand EditButtonCommand { get; }
        public StartViewModel(QuizManager quizManager, NavigationManager navigationManager)
        {
            _quizManager = quizManager;
            _navigationManager = navigationManager;
            PlayButtonCommand = new RelayCommand(PlayButton);
            CreateButtonCommand = new RelayCommand(CreateButton);
            EditButtonCommand = new RelayCommand(EditButton);
        }

        private void PlayButton()
        {
            _navigationManager.CurrentViewModel = new QuizToPlayViewModel(_quizManager, _navigationManager);
        }
        private void CreateButton()
        {
            _navigationManager.CurrentViewModel = new CreateViewModel(_quizManager, _navigationManager);
        }
        private void EditButton()
        {
            _navigationManager.CurrentViewModel = new EditViewModel(_quizManager, _navigationManager);
        }
    }
}
