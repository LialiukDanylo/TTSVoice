using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TTSVoice.Domain.Interfaces;
using TTSVoice.Infrastructure.Services;

namespace TTSVoice.UI.ViewModels
{
    partial class MainWindowVM : ObservableObject
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SpeakCommand))]
        private string _inputText = string.Empty;

        private IAudioService _audioService;

        [ObservableProperty]
        private ObservableCollection<string> _devices = new();

        [ObservableProperty]
        private int _selectedDeviceIndex = -1;

        private CancellationTokenSource? _playCts;

        public MainWindowVM()
        {
            _audioService = new NAudioService();
            _devices = new ObservableCollection<string>(_audioService.GetOutputDevices());

            if (Devices.Count > 0) _selectedDeviceIndex = 0;
        }

        [RelayCommand(CanExecute = nameof(CanSpeak), AllowConcurrentExecutions = true)]
        private async Task Speak()
        {
            _playCts?.Cancel();
            _playCts = new CancellationTokenSource();

            try
            {
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private bool CanSpeak => !string.IsNullOrWhiteSpace(InputText);
    }
}
