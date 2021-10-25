using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Labb_3.Managers;
using Labb_3.ViewModels;

namespace Labb_3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationManager _navigationManager;
        private readonly QuizManager _quizManager;


        public App()
        {
            _quizManager = new QuizManager();
            _navigationManager = new NavigationManager();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //Vilken vy den ska starta på
            _navigationManager.CurrentViewModel = new CreateViewModel(_quizManager, _navigationManager);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_quizManager, _navigationManager )
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
