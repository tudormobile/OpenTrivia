using System.Windows;
using WpfSampleApp.Services;
using WpfSampleApp.ViewModels;

namespace WpfSampleApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow();
            var openTriviaService = IOpenTriviaService.Create();
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
