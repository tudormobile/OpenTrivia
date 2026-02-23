using System.Net.Http;
using System.Windows;
using Tudormobile.OpenTrivia;
using Tudormobile.OpenTrivia.UI.Services;
using WpfSampleApp.Services;
using WpfSampleApp.ViewModels;

namespace WpfSampleApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly HttpClient _client = new();
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow();
            var openTriviaService = IOpenTriviaService.Create(IOpenTriviaClient.Create(_client, autoDecode: true));
            var viewModel = new MainWindowViewModel(IGameService.Create(openTriviaService), IDialogService.Create())
            {
                Title = "WpfSampleApp - Hello World!",
                CategoriesModel = new CategoriesViewModel(openTriviaService)
            };

            MainWindow.DataContext = viewModel;
            MainWindow.Show();
        }
    }

}
