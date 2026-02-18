using WpfSampleApp.ViewModels;

namespace WpfSampleApp.Views;

public partial class GameView
{
    public GameView()
    {
        InitializeComponent();
        this.Loaded += (s, e) => (DataContext as GameViewModel)?.LoadGameCommand.Execute(null);
    }

}
