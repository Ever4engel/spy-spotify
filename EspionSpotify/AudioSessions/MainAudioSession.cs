﻿using EspionSpotify.Extensions;
using EspionSpotify.Native;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EspionSpotify.Spotify;

namespace EspionSpotify.AudioSessions
{
    public sealed class MainAudioSession : IMainAudioSession, IDisposable
    {
        private const int SLEEP_VALUE = 50;
        private const int NUMBER_OF_SAMPLES = 3;

        private bool _disposed = false;

        private readonly IProcessManager _processManager;
        private readonly int? _spytifyProcessId;
        private ICollection<int> _spotifyProcessesIds;

        public MMDeviceEnumerator AudioMMDevices { get; private set; }
        public AudioMMDevicesManager AudioMMDevicesManager { get; private set; }
        public int AudioDeviceVolume => (int)((AudioMMDevicesManager.AudioEndPointDevice?.AudioEndpointVolume?.MasterVolumeLevelScalar ?? 0f) * 100);

        public bool IsAudioEndPointDeviceIndexAvailable => AudioMMDevicesManager.AudioEndPointDeviceNames.IncludesKey(AudioMMDevicesManager.AudioEndPointDeviceID);

        public ICollection<AudioSessionControl> SpotifyAudioSessionControls { get; private set; } = new List<AudioSessionControl>();
        public void ClearSpotifyAudioSessionControls() => SpotifyAudioSessionControls = new List<AudioSessionControl>();

        private SessionCollection GetSessionsAudioEndPointDevice => AudioMMDevicesManager.GetAudioEndPointDeviceSessions;

        internal MainAudioSession(string audioEndPointDevice) :
            this(audioEndPointDevice, processManager: new ProcessManager())
        { }

        private MainAudioSession(string audioEndPointDeviceID, IProcessManager processManager)
        {
            _processManager = processManager;

            _spytifyProcessId = _processManager.GetCurrentProcess()?.Id;

            AudioMMDevices = new MMDeviceEnumerator();
            AudioMMDevicesManager = new AudioMMDevicesManager(AudioMMDevices, audioEndPointDeviceID);

            AudioMMDevices.RegisterEndpointNotificationCallback(AudioMMDevicesManager);
        }

        public void SetAudioDeviceVolume(int volume)
        {
            if (AudioMMDevicesManager.AudioEndPointDevice == null) return;
            if (AudioMMDevicesManager.VolumeNotificationEmitted)
            {
                AudioMMDevicesManager.VolumeNotificationEmitted = false;
                return;
            }
            if (float.TryParse(volume.ToString(), out var fNewVolume))
            {
                AudioMMDevicesManager.AudioEndPointDevice.AudioEndpointVolume.MasterVolumeLevelScalar = fNewVolume / 100;
            }
        }

        public async Task SleepWhileTheSongEnds()
        {
            for (var times = 1000; await IsSpotifyCurrentlyPlaying() && times > 0; times -= SLEEP_VALUE * NUMBER_OF_SAMPLES)
            {
                await Task.Delay(SLEEP_VALUE);
            }
        }

        #region AudioSession Spotify Playing
        public async Task<bool> IsSpotifyCurrentlyPlaying()
        {
            var samples = new List<double>();

            for (var sample = 0; sample < NUMBER_OF_SAMPLES; sample++)
            {
                var spotifySoundValue = 0.0;
                await Task.Delay(SLEEP_VALUE);

                var spotifyAudioSessionControls = new List<AudioSessionControl>(SpotifyAudioSessionControls).AsReadOnly();
                foreach (var audioSession in spotifyAudioSessionControls)
                {
                    var soundValue = Math.Round(audioSession.AudioMeterInformation.MasterPeakValue * 100.0, 1);
                    if (soundValue == 0.0) continue;

                    spotifySoundValue = soundValue;
                }

                samples.Add(spotifySoundValue);
            }

            return samples.DefaultIfEmpty().Average() > 1.0;
        }
        #endregion AudioSession Spotify Playing

        #region AudioSession Spotify Muter
        public void SetSpotifyToMute(bool mute)
        {
            var spotifyAudioSessionControlsLocked = new List<AudioSessionControl>(SpotifyAudioSessionControls).AsReadOnly();
            foreach (var audioSession in spotifyAudioSessionControlsLocked)
            {
                audioSession.SimpleAudioVolume.Mute = mute;
            }
        }
        #endregion AudioSession Spotify Muter

        #region AudioSession Wait Spotify
        public async Task<bool> WaitSpotifyAudioSessionToStart(bool running)
        {
            _spotifyProcessesIds = SpotifyProcess.GetSpotifyProcesses(_processManager).Select(x => x.Id).ToList();

            if (await IsSpotifyPlayingOutsideDefaultAudioEndPoint(running))
            {
                return false;
            }

            var sessionAudioEndPointDeviceLocked = GetSessionsAudioEndPointDevice;
            lock (sessionAudioEndPointDeviceLocked)
            {
                for (var i = 0; i < sessionAudioEndPointDeviceLocked.Count; i++)
                {
                    var currentAudioSessionControl = sessionAudioEndPointDeviceLocked[i];
                    var currentProcessId = (int)currentAudioSessionControl.GetProcessID;
                    if (!IsSpotifyAudioSessionControl(currentProcessId)) continue;

                    return true;
                }
            }

            return false;
        }
        #endregion AudioSession Wait Spotify

        #region AudioSession App Muter
        public void SetSpotifyVolumeToHighAndOthersToMute(bool mute)
        {
            var sessionAudioEndPointDeviceLocked = GetSessionsAudioEndPointDevice;

            lock (sessionAudioEndPointDeviceLocked)
            {
                for (var i = 0; i < sessionAudioEndPointDeviceLocked.Count; i++)
                {
                    var currentAudioSessionControl = sessionAudioEndPointDeviceLocked[i];
                    var currentProcessId = (int)currentAudioSessionControl.GetProcessID;

                    if (currentProcessId.Equals(_spytifyProcessId))
                    {
                        if (currentAudioSessionControl.SimpleAudioVolume.Volume == 1) continue;
                        currentAudioSessionControl.SimpleAudioVolume.Volume = 1;
                    }
                    else if (IsSpotifyAudioSessionControl(currentProcessId))
                    {
                        SpotifyAudioSessionControls.Add(currentAudioSessionControl);

                        if (currentAudioSessionControl.SimpleAudioVolume.Volume < 1)
                        {
                            currentAudioSessionControl.SimpleAudioVolume.Volume = 1;
                        }
                    }
                    else if (!currentAudioSessionControl.SimpleAudioVolume.Mute.Equals(mute))
                    {
                        currentAudioSessionControl.SimpleAudioVolume.Mute = mute;
                    }
                }
            }
        }
        #endregion AudioSession App Muter

        #region AudioSession Spotify outside of endpoint
        private async Task<bool> IsSpotifyPlayingOutsideDefaultAudioEndPoint(bool running)
        {
            int? spotifyAudioSessionProcessId = null;

            while (running && spotifyAudioSessionProcessId == null && _spotifyProcessesIds.Any())
            {
                var allSessionsAudioEndPointDevices = AudioMMDevices.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).Select(x => x.AudioSessionManager.Sessions).ToArray();

                lock (allSessionsAudioEndPointDevices)
                {
                    foreach (var sessionAudioEndPointDevice in allSessionsAudioEndPointDevices)
                    {
                        for (var i = 0; i < sessionAudioEndPointDevice.Count; i++)
                        {
                            var currentAudioSessionControl = sessionAudioEndPointDevice[i];
                            var currentProcessId = (int)currentAudioSessionControl.GetProcessID;
                            if (!IsSpotifyAudioSessionControl(currentProcessId)) continue;

                            spotifyAudioSessionProcessId = currentProcessId;
                            break;
                        }
                        if (spotifyAudioSessionProcessId.HasValue) break;
                    }
                }

                await Task.Delay(300);

                _spotifyProcessesIds = SpotifyProcess.GetSpotifyProcesses(_processManager).Select(x => x.Id).ToList();
            }

            var sessionAudioSelectedEndPointDevice = GetSessionsAudioEndPointDevice;

            for (var i = 0; i < sessionAudioSelectedEndPointDevice.Count; i++)
            {
                var currentAudioSessionControl = sessionAudioSelectedEndPointDevice[i];
                var currentProcessId = (int)currentAudioSessionControl.GetProcessID;
                if (currentProcessId != spotifyAudioSessionProcessId) continue;

                return false;
            }

            return true;
        }
        #endregion AudioSession Spotify outside of endpoint

        private bool IsSpotifyAudioSessionControl(int processId) => _spotifyProcessesIds.Any(x => x == processId);

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                AudioMMDevices.UnregisterEndpointNotificationCallback(AudioMMDevicesManager);
                AudioMMDevices.Dispose();
                AudioMMDevices = null;
            }

            _disposed = true;
        }
    }
}
