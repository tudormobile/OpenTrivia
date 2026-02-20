using System.Windows;

namespace WpfSampleApp.Services;

internal class DialogService : IDialogService
{
    internal DialogService() { }

    public void ShowMessage(string message)
        => MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
}

public interface IDialogService
{
    public static IDialogService Create() => new DialogService();
    void ShowMessage(string message);
}
