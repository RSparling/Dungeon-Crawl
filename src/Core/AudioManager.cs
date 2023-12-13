using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Runtime.InteropServices.WindowsRuntime;
using Dungeon_Crawl.Properties;
using NAudio.Wave;

public class AudioManager : IDisposable
{
    private static AudioManager instance;

#pragma warning disable CS0649 // Field 'AudioManager.currentMusicFile' is never assigned to, and will always have its default value null
    private string currentMusicFile;
#pragma warning restore CS0649 // Field 'AudioManager.currentMusicFile' is never assigned to, and will always have its default value null
    private WaveOutEvent musicPlayer = new WaveOutEvent();
    private WaveStream waveStream;
    private readonly Dictionary<string, SoundPlayer> soundPlayers = new Dictionary<string, SoundPlayer>();

    public static AudioManager Get
    {
        get
        {
            if (instance == null)
                instance = new AudioManager();
            return instance;
        }
    }

    public AudioManager()
    {
        // Initialize the music player
        musicPlayer.PlaybackStopped += OnPlaybackStopped;
    }

    public void PlaySoundEffect(SoundLibrary sound)
    {
        Stream soundStream = GetSoundStream(sound);
        if (soundStream != null)
        {
            if (soundPlayers.TryGetValue(sound.ToString(), out var soundPlayer))
            {
                soundPlayer.Stream = soundStream;
                soundPlayer.Play();
            }
            else
            {
                soundPlayer = new SoundPlayer(soundStream);
                soundPlayers[sound.ToString()] = soundPlayer;
                soundPlayer.Play();
            }
        }
    }

    private Stream GetSoundStream(SoundLibrary sound)
    {
        switch (sound)
        {
            case SoundLibrary.sfx_step:
                return Resources.sfx_FootSteps;

            case SoundLibrary.music_exploration:
                return Resources.Music_Exploration;

            case SoundLibrary.music_combat:
                return Resources.Music_Fight;

            case SoundLibrary.sfx_StartFight:
                return Resources.sfx_CombatStart;

            case SoundLibrary.sfx_hit1:
                return Resources.sfx_hit1;

            case SoundLibrary.sfx_hit2:
                return Resources.sfx_hit2;

            case SoundLibrary.sfx_hit3:
                return Resources.sfx_hit3;

            case SoundLibrary.sfx_miss:
                return Resources.sfx_miss;

            case SoundLibrary.sfx_spell:
                return Resources.sfx_spell;

            case SoundLibrary.sfx_fizzle:
                return Resources.sfx_fizzle;

            case SoundLibrary.sfx_WinFanFare:
                return Resources.sfx_WinFanFare;

            case SoundLibrary.sfx_RunAway:
                return Resources.sfx_RunAway;

            case SoundLibrary.music_TitleScreen:
                return Resources.music_Title;

            case SoundLibrary.music_GameOver:
                return Resources.music_GameOver;

            default:
                return null; // Replace with actual retrieval method
        }
    }

    public void PlayMusic(SoundLibrary sound)
    {
        StopMusic();

        Stream audioStream = GetSoundStream(sound);
        if (audioStream != null)
        {
            // Dispose of the old WaveStream if necessary
            waveStream?.Dispose();

            // Create a new WaveStream for the audio stream
            waveStream = new WaveFileReader(audioStream); // or an appropriate reader for the stream format
            musicPlayer.Init(waveStream);
            musicPlayer.Play();
        }
    }

    private void OnPlaybackStopped(object sender, StoppedEventArgs args)
    {
        if (currentMusicFile != null)
        {
            var reader = new AudioFileReader(currentMusicFile);
            musicPlayer.Init(reader);
            musicPlayer.Play();
        }
    }

    public void StopMusic()
    {
        if (musicPlayer != null && musicPlayer.PlaybackState == PlaybackState.Playing)
        {
            musicPlayer.Stop();
            waveStream?.Dispose();
            waveStream = null;
        }
    }

    public void Dispose()
    {
        StopMusic(); // Stop the music before disposing

        // Iterate through each sound player in the dictionary
        foreach (var soundPlayer in soundPlayers.Values)
        {
            soundPlayer.Stop(); // Stop the sound player
            soundPlayer.Dispose(); // Dispose of the sound player
        }

        soundPlayers.Clear(); // Clear the dictionary of sound players
    }

    public enum SoundLibrary
    {
        sfx_step,
        sfx_hit1,
        sfx_hit2,
        sfx_hit3,
        sfx_miss,
        sfx_spell,
        sfx_fizzle,
        sfx_StartFight,
        sfx_WinFanFare,
        sfx_RunAway,
        music_exploration,
        music_combat,
        music_TitleScreen,
        music_GameOver
    }
}