namespace TTSVoice.Domain.Interfaces
{
    public interface ITtsService
    {
        Task<byte[]> SynthesizeAsync(string text, CancellationToken ct);
    }
}
