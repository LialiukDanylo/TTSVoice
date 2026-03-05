using System.Windows;
using TTSVoice.UI.ViewModels;

namespace TTSVoice.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
            DataContext = new MainWindowVM();
        }
    }
}