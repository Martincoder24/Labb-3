using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Labb_3.Managers;
using Labb_3.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Labb_3.ViewModels
{
    public class PlayModeViewModel : ObservableObject
    {
        
        //Även en poängräknare
        //en bild länkad till varje fråga
        private readonly QuizManager _quizManager;
        private readonly NavigationManager _navigationManager;
        
        private Quiz _currentQuiz;

        public Quiz CurrentQuiz
        {
            get => _currentQuiz;
            set => SetProperty(ref _currentQuiz , value);
        }
        
        private Question _currentQuestion;

        public Question CurrentQuestion
        {
            get =>  _currentQuestion;
            set =>  SetProperty(ref _currentQuestion, value);
        }

        //Properties för mina counters; CorrectAnswers, TotalAnswers
        private int _correctAnswerCounter;

        public int CorrectAnswerCounter
        {
            get => _correctAnswerCounter;
            set => SetProperty(ref _correctAnswerCounter, value);
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
            CurrentQuestion = _currentQuiz.GetRandomQuestion();
            ExitQuizCommand = new RelayCommand(ExitQuiz);
            AnswerOneCommand = new RelayCommand(AnswerOneSelection);
            AnswerTwoCommand = new RelayCommand(AnswerTwoSelection);
            AnswerThreeCommand = new RelayCommand(AnswerThreeSelection);
        }

        private void AnswerOneSelection()
        {

            //IF man har valt korrekt svar så ska correctanswer countern plussas
            if (_currentQuestion.CorrectAnswer == 0)
            {
                CorrectAnswerCounter++;
            }
            //TotalAnswer countern ska plussas
            TotalAnswerCounter++;

            if (_currentQuiz.Questions.Count > TotalAnswerCounter)
            {
                CurrentQuestion =_currentQuiz.GetRandomQuestion();
            }
            else
            {
                //Messagebox ska komma upp o ge en sitt score o sen ska man slussas vidare till startvyn.
                if (MessageBox.Show($"Your score is {CorrectAnswerCounter}/{TotalAnswerCounter}!", "Results") ==
                    MessageBoxResult.OK)
                {
                    //När man klickar på OK så ska an tillbaka till startmenyn
                    ExitQuiz();
                }
            }
        }
        private void AnswerTwoSelection()
        {

            //Om man har valt korrekt svar så ska correctanswer countern plussas
            if (_currentQuestion.CorrectAnswer == 1)
            {
                CorrectAnswerCounter++;
            }
            //TotalAnswer countern ska plussas
            TotalAnswerCounter++;

            if (_currentQuiz.Questions.Count > TotalAnswerCounter)
            {
                CurrentQuestion = _currentQuiz.GetRandomQuestion();
            }
            else
            {
                //Messagebox ska komma upp o ge en sitt score o sen ska man slussas vidare till startvyn.
                if (MessageBox.Show($"Your score is {CorrectAnswerCounter}/{TotalAnswerCounter}!", "Results") ==
                    MessageBoxResult.OK)
                {
                    //När man klickar på OK så ska an tillbaka till startmenyn
                    ExitQuiz();
                }
            }
        }
        private void AnswerThreeSelection()
        {

            //IF man har valt korrekt svar så ska correctanswer countern plussas
            if (_currentQuestion.CorrectAnswer == 2)
            {
                CorrectAnswerCounter++;
            }
            //TotalAnswer countern ska plussas
            TotalAnswerCounter++;

            if (_currentQuiz.Questions.Count > TotalAnswerCounter)
            {
                CurrentQuestion = _currentQuiz.GetRandomQuestion();
            }
            else
            {
                //Messagebox ska komma upp o ge en sitt score o sen ska man slussas vidare till startvyn.
                if (MessageBox.Show($"Your score is {CorrectAnswerCounter}/{TotalAnswerCounter}!", "Results") ==
                    MessageBoxResult.OK)
                {
                    //När man klickar på OK så ska an tillbaka till startmenyn
                    ExitQuiz();
                }
            }
        }
        private void ExitQuiz()
        {
            _navigationManager.CurrentViewModel = new StartViewModel(_quizManager, _navigationManager);
        }
    }
}
