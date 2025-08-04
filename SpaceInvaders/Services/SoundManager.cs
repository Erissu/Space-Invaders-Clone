using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NAudio.Wave; // A utilizar a nova biblioteca

namespace SpaceInvaders.Services
{
    public class SoundManager
    {
        // Lista para manter uma referência aos players ativos, evitando que o som seja cortado pelo Garbage Collector
        private readonly List<IDisposable> _activeSoundPlayers = new();

        public SoundManager()
        {
            // O construtor agora está limpo, o carregamento é feito no momento de tocar
        }

        public void Play(string key)
        {
            try
            {
                // Mapeia as nossas "chaves" de som para os nomes dos ficheiros
                var fileName = key switch
                {
                    "shoot" => "shoot.mp3",
                    "invaderkilled" => "hit.mp3",
                    "explosion" => "hit.mp3",
                    "barrier" => "barrier.mp3",
                    _ => null
                };

                if (fileName == null)
                {
                    Debug.WriteLine($"[NAudio] Som com a chave '{key}' não encontrado.");
                    return;
                }

                // Monta o caminho completo para o ficheiro de áudio na pasta de compilação
                string fullPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Sounds", fileName);

                if (!File.Exists(fullPath))
                {
                    Debug.WriteLine($"[NAudio] ERRO: Ficheiro de som NÃO ENCONTRADO em: '{fullPath}'");
                    return;
                }
                
                // Cria os objetos da NAudio para ler e tocar o ficheiro de som
                var audioFile = new AudioFileReader(fullPath);
                var outputDevice = new WaveOutEvent();

                // Adiciona os objetos à lista de players ativos para os proteger
                _activeSoundPlayers.Add(audioFile);
                _activeSoundPlayers.Add(outputDevice);

                // Configura um evento que será acionado quando o som terminar de tocar
                outputDevice.PlaybackStopped += (sender, args) =>
                {
                    // Remove os objetos da lista e liberta os recursos de forma segura
                    _activeSoundPlayers.Remove(outputDevice);
                    _activeSoundPlayers.Remove(audioFile);
                    outputDevice.Dispose();
                    audioFile.Dispose();
                };

                outputDevice.Init(audioFile);
                outputDevice.Play();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[NAudio] Erro ao tocar o som '{key}': {ex.Message}");
            }
        }
    }
}
