using System.Windows.Controls;
using WpfSampleApp.ViewModels;

namespace WpfSampleApp.Views
{
    /// <summary>
    /// Interaction logic for CategoriesView.xaml
    /// </summary>
    public partial class CategoriesView : UserControl
    {
        public CategoriesView()
        {
            InitializeComponent();
            this.Loaded += (s, e) => (DataContext as CategoriesViewModel)?.LoadCategoriesCommand.Execute(null);
        }
    }
}
