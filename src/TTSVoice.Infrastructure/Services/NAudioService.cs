using NAudio.Wave;
using TTSVoice.Domain.Interfaces;

namespace TTSVoice.Infrastructure.Services
{
    public class NAudioService : IAudioService
    {
        public List<string> GetOutputDevices()
        {
            var devices = new List<string>();

            devices.Add("Default");

            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                var caps = WaveOut.GetCapabilities(i);
                devices.Add(caps.ProductName);
            }

            return devices;
        }

        public async Task PlayAsync(byte[] data, int deviceIndex, CancellationToken ct)
        {
            if (data == null || data.Length == 0) return;

            int systemDeviceId = deviceIndex - 1;

            await Task.Run(async () =>
            {
                using var ms = new MemoryStream(data);

                using var reader = new StreamMediaFoundationReader(ms);
                using var outputDevice = new WaveOutEvent { DeviceNumber = systemDeviceId };

                outputDevice.Init(reader);
                outputDevice.Play();

                using var registration = ct.Register(() => outputDevice.Stop());

                while (outputDevice.PlaybackState == PlaybackState.Playing && !ct.IsCancellationRequested)
                {
                    await Task.Delay(50, ct);
                }
            }, ct);
        }
    }
}
