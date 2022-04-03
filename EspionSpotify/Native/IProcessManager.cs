﻿using EspionSpotify.Native.Models;

namespace EspionSpotify.Native
{
    public interface IProcessManager
    {
        IProcess GetCurrentProcess();
        IProcess[] GetProcesses();
        IProcess[] GetProcessesByName(string processName);
        IProcess GetProcessById(int processId);

        IProcess Start(string fileName);
    }
}