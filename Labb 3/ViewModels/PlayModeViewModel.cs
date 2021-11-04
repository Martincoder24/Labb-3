using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labb_3.Managers;
using Labb_3.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Labb_3.ViewModels
{
    public class PlayModeViewModel : ObservableObject
    {
        //Todo Denna vymodell skall innehålla frågan och alla svarsalternativ samt en return-knapp.
        //Även en poängräknare
        //en bild länkad till varje fråga
        private readonly QuizManager _quizManager;
        private readonly NavigationManager _navigationManager;

        private readonly Quiz _currentQuiz;
        private readonly Question _currentQuestion;

        //Properties för mina counters; CorrectAnswers, TotalAnswers
        private int _correctAnswerCounter;

        public int CorrectAnswerCounter
        {
            get => _correctAnswerCounter;
            set => SetProperty(ref _correctAnswerCounter , value);
        }
        private int _totalAnswerCounter;
        //Tanke: antingen så har man att man ser totala antalet frågor från början eller
        //så är det progressivt per svarad fråga
        public int TotalAnswerCounter
        {
            get => _totalAnswerCounter;
            set => SetProperty(ref _totalAnswerCounter, value);
        }

        //Properties som bestämmer min första fråga och som sedan kan sätta mina kommande frågor
        //private string _statement = _currentQuestion.Statement;

        //public string Statement
        //{
        //    get => _statement;
        //    set => SetProperty(ref _statement , value);
        //}
        //Mina RelayCommands:
        public RelayCommand ExitQuizCommand { get; }
        public RelayCommand AnswerOneCommand { get; }
        public RelayCommand AnswerTwoCommand { get; }
        public RelayCommand AnswerThreeCommand { get; }

        public PlayModeViewModel(QuizManager quizManager, NavigationManager navigationManager, Quiz updatedQuiz)
        {
            _quizManager = quizManager;
            _navigationManager = navigationManager;
            _currentQuiz = updatedQuiz;
            _currentQuestion = updatedQuiz.GetRandomQuestion();
            //Här skall jag ta bort den befintliga frågan genom att få reda på indexet av frågan i mitt quiz
            RemoveAlreadyAskedQuestion();
            //Sätt currentQuestion efter att ha använt metoden RandomQuestion på _currentQuiz

            ExitQuizCommand = new RelayCommand(ExitQuiz);
            //AnswerOneCommand = new RelayCommand();
            //AnswerTwoCommand = new RelayCommand();
            //AnswerThreeCommand = new RelayCommand();
        }

        private void AnswerOneSelection()
        {
            //TotalAnswer countern ska plussas
            TotalAnswerCounter++;
            //IF man har valt korrekt svar så ska correctanswer countern plussas
            if ( _currentQuestion.CorrectAnswer == 0)
            {
                CorrectAnswerCounter++;
            }
            //Ta bort bort den nuvarande frågan och randomgenera en till
            RemoveAlreadyAskedQuestion();
            if (_currentQuiz.Questions.Count > 0)
            {
                _currentQuiz.GetRandomQuestion();
            }
            else
            {

            }
            
            //Slumpa fram nästa fråga.
        }

        private void RemoveAlreadyAskedQuestion()
        {
            var index = _currentQuiz.Questions.ToList().IndexOf(_currentQuestion);
            _currentQuiz.RemoveQuestion(index);
        }
        
        //Todo Jag måste ha en metod som ändrar frågan till nästa fråga med sina resp. svarsalternativ.

        //Todo Jag måste se till att första frågan från Quizzet är på plats vid tryck av Lets play sen innan.

        private void ExitQuiz()
        {
            _navigationManager.CurrentViewModel = new StartViewModel(_quizManager, _navigationManager);
        }
    }
}
