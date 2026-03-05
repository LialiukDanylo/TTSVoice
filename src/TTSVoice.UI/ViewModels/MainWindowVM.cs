using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace TTSVoice.UI.ViewModels
{
    partial class MainWindowVM : ObservableObject
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SpeakCommand))]
        private string _inputText = string.Empty;

        [RelayCommand(CanExecute = nameof(CanSpeak))]
        private async Task Speak()
        {
            Debug.WriteLine(InputText);
        }
        private bool CanSpeak => !string.IsNullOrWhiteSpace(InputText);
    }
}
