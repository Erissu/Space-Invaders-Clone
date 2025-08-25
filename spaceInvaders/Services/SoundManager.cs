using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NAudio.Wave;

namespace SpaceInvaders.Services
{
    public class SoundManager
    {
        private readonly List<IDisposable> _activeSoundPlayers = new();
        private IWavePlayer? _marchSoundPlayer;
        private AudioFileReader? _marchAudioFile;

        public void Play(string key)
        {
            try {
                var fileName = key switch {
                    "shoot" => "shoot.mp3",
                    "invaderkilled" => "hit.mp3",
                    "explosion" => "hit.mp3",
                    "barrier" => "barrier.mp3",
                    _ => null
                };
                if (fileName == null) return;
                string fullPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Sounds", fileName);
                if (!File.Exists(fullPath)) return;
                var audioFile = new AudioFileReader(fullPath);
                var outputDevice = new WaveOutEvent();
                _activeSoundPlayers.Add(audioFile);
                _activeSoundPlayers.Add(outputDevice);
                outputDevice.PlaybackStopped += (sender, args) => {
                    _activeSoundPlayers.Remove(outputDevice);
                    _activeSoundPlayers.Remove(audioFile);
                    outputDevice.Dispose();
                    audioFile.Dispose();
                };
                outputDevice.Init(audioFile);
                outputDevice.Play();
            } catch (Exception ex) {
                Debug.WriteLine($"[NAudio] Erro ao tocar o som '{key}': {ex.Message}");
            }
        }

        public void StartMarch()
        {
            StopMarch();
            try {
                string fullPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Sounds", "march.mp3");
                if (!File.Exists(fullPath)) return;
                _marchAudioFile = new AudioFileReader(fullPath);
                var loopStream = new LoopStream(_marchAudioFile);
                _marchSoundPlayer = new WaveOutEvent();
                _marchSoundPlayer.Init(loopStream);
                _marchSoundPlayer.Play();
            } catch (Exception ex) {
                Debug.WriteLine($"[NAudio] Erro ao iniciar marcha: {ex.Message}");
            }
        }

        public void StopMarch()
        {
            _marchSoundPlayer?.Stop();
            _marchSoundPlayer?.Dispose();
            _marchAudioFile?.Dispose();
        }
    }
    
    // Classe auxiliar de Loop para NAudio
    public class LoopStream : WaveStream { private readonly WaveStream _sourceStream; public bool EnableLooping { get; set; } = true; public override WaveFormat WaveFormat => _sourceStream.WaveFormat; public override long Length => _sourceStream.Length; public override long Position { get => _sourceStream.Position; set => _sourceStream.Position = value; } public LoopStream(WaveStream sourceStream) { _sourceStream = sourceStream; } public override int Read(byte[] buffer, int offset, int count) { int totalBytesRead = 0; while (totalBytesRead < count) { int bytesRead = _sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead); if (bytesRead == 0) { if (_sourceStream.Position == 0 || !EnableLooping) break; _sourceStream.Position = 0; } totalBytesRead += bytesRead; } return totalBytesRead; } }
}
