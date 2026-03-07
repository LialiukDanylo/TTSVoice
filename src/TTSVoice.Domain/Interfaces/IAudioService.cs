namespace TTSVoice.Domain.Interfaces
{
    public interface IAudioService
    {
        List<string> GetOutputDevices();
        Task PlayAsync(byte[] data, int deviceIndex, int volume, CancellationToken ct);
    }
}
