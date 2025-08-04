using Windows.Media.Core;
using Windows.Media.Playback;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO; // Importante para usar Path.Combine
using Microsoft.UI.Dispatching;

namespace SpaceInvaders.Services
{
    public class SoundManager
    {
        private readonly Dictionary<string, MediaSource> _soundSources = new();
        private readonly List<MediaPlayer> _activePlayers = new();
        private readonly DispatcherQueue _dispatcherQueue;

        public SoundManager(DispatcherQueue dispatcherQueue)
        {
            _dispatcherQueue = dispatcherQueue;
            Debug.WriteLine("[SoundManager v4] Iniciando...");
            LoadSound("shoot", "shoot.mp3");
            LoadSound("invaderkilled", "hit.mp3");
            LoadSound("explosion", "hit.mp3");
            LoadSound("barrier", "barrier.mp3");
            Debug.WriteLine("[SoundManager v4] Carga de fontes de som finalizada.");
        }

        private void LoadSound(string key, string fileName)
        {
            try
            {
                // CORRIGIDO: Monta o caminho completo para o arquivo na pasta de compilação
                string fullPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Sounds", fileName);
                
                // Verifica se o arquivo realmente existe antes de tentar carregar
                if (File.Exists(fullPath))
                {
                    var source = MediaSource.CreateFromUri(new Uri(fullPath));
                    _soundSources[key] = source;
                    Debug.WriteLine($"[SoundManager v4] SUCESSO ao carregar fonte de som: '{key}' de '{fullPath}'");
                }
                else
                {
                    Debug.WriteLine($"[SoundManager v4] ERRO FATAL: Arquivo de som NÃO ENCONTRADO em: '{fullPath}'");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SoundManager v4] ERRO ao carregar fonte de som: '{key}'. Mensagem: {ex.Message}");
            }
        }

        public void Play(string key)
        {
            if (_soundSources.TryGetValue(key, out var source))
            {
                var player = new MediaPlayer
                {
                    Source = source,
                    Volume = 1.0,
                    AudioCategory = MediaPlayerAudioCategory.GameEffects
                };

                _activePlayers.Add(player);

                player.MediaEnded += (sender, args) =>
                {
                    _dispatcherQueue.TryEnqueue(() => 
                    {
                        if (sender is MediaPlayer finishedPlayer)
                        {
                            _activePlayers.Remove(finishedPlayer);
                        }
                    });
                };

                player.Play();
            }
            else
            {
                Debug.WriteLine($"[SoundManager v4] AVISO: Fonte de som não encontrada no dicionário: '{key}'");
            }
        }
    }
}
