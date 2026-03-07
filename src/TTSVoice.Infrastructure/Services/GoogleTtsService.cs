using System.Diagnostics;
using TTSVoice.Domain.Interfaces;

namespace TTSVoice.Infrastructure.Services
{
    public class GoogleTtsService : ITtsService
    {
        public readonly HttpClient _httpClient;

        public GoogleTtsService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
        }

        public async Task<byte[]> SynthesizeAsync(string text, CancellationToken ct)
        {
            if(string.IsNullOrWhiteSpace(text)) return Array.Empty<byte>();

            string url = $"https://translate.google.com/translate_tts?ie=UTF-8&q={Uri.EscapeDataString(text)}&tl=en&client=tw-ob";

            try
            {
                var response = await _httpClient.GetAsync(url, ct);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsByteArrayAsync(ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Array.Empty<byte>();
            }
        }
    }
}
