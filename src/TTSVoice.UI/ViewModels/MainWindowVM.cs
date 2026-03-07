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
        private int _selectedOutputIndex = -1;

        [ObservableProperty]
        private bool _isMonitoringEnabled = false;

        [ObservableProperty]
        private int _selectedMonitorIndex = -1;

        private CancellationTokenSource? _playCts;

        private ITtsService _ttsService;
        public MainWindowVM()
        {
            _audioService = new NAudioService();
            _devices = new ObservableCollection<string>(_audioService.GetOutputDevices());
            _ttsService = new GoogleTtsService();

            if (Devices.Count > 0) SelectedOutputIndex = 0; SelectedMonitorIndex = 0;
        }

        [RelayCommand(CanExecute = nameof(CanSpeak), AllowConcurrentExecutions = true)]
        private async Task Speak()
        {
            _playCts?.Cancel();
            _playCts = new CancellationTokenSource();
            var ct = _playCts.Token;

            try
            {
                byte[] audioData = await _ttsService.SynthesizeAsync(InputText, ct);

                if (audioData == null || audioData.Length == 0) return;

                var playbackTasks = new List<Task>();

                playbackTasks.Add(_audioService.PlayAsync(audioData, SelectedOutputIndex, ct));

                if (IsMonitoringEnabled)
                {
                    playbackTasks.Add(_audioService.PlayAsync(audioData, SelectedMonitorIndex, ct));
                }

                await Task.WhenAll(playbackTasks);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private bool CanSpeak => !string.IsNullOrWhiteSpace(InputText);
    }
}
