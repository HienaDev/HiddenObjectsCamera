using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor.Build.Reporting;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Debug = UnityEngine.Debug;
using System.Runtime.InteropServices;

public class Tools : MonoBehaviour
{
    //
    private void Start()
    {
        SetWindowTitle("Volley Cats");
    }

#if UNITY_STANDALONE_WIN
    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowText(IntPtr hWnd, string lpString);
    [DllImport("user32.dll", SetLastError = true)]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
    [DllImport("user32.dll")]
    static extern IntPtr EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    // Delegate to filter windows
    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
    private static IntPtr FindWindowByProcessId(uint processId)
    {
        IntPtr windowHandle = IntPtr.Zero;
        EnumWindows((hWnd, lParam) =>
        {
            uint windowProcessId;
            GetWindowThreadProcessId(hWnd, out windowProcessId);
            if (windowProcessId == processId)
            {
                windowHandle = hWnd;
                return false; // Found the window, stop enumerating
            }
            return true; // Continue enumerating
        }, IntPtr.Zero);
        return windowHandle;
    }
    static void SetWindowTitle(string title)
    {
#if !UNITY_EDITOR
        uint processId = (uint)Process.GetCurrentProcess().Id;
        IntPtr hWnd = FindWindowByProcessId(processId);
        if (hWnd != IntPtr.Zero)
        {
            SetWindowText(hWnd, title);
        }


        // Generate random position
        System.Random random = new System.Random();
        int randomX = random.Next(0, 1000);
        int randomY = random.Next(0, 400);

        // Move the window to the random position
        if (SetWindowPos(hWnd, IntPtr.Zero, randomX, randomY, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW))
        {
            Console.WriteLine($"Window moved to position ({randomX}, {randomY})");
        }
        else
        {
            Console.WriteLine("Failed to move the window.");
        }
#endif
    }
#else
        static void SetWindowTitle(string title)
        {
        }
#endif


#if UNITY_EDITOR
    [MenuItem("Tools/Build Windows (x64)", priority = 0)]
    public static bool BuildGame()
    {
        // Specify build options
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = EditorBuildSettings.scenes
          .Where(s => s.enabled)
          .Select(s => s.path)
          .ToArray();
        buildPlayerOptions.locationPathName = Path.Combine("Builds", "MPWyzard.exe");
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;
        // Perform the build
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        // Output the result of the build
        Debug.Log($"Build ended with status: {report.summary.result}");
        // Additional log on the build, looking at report.summary
        return report.summary.result == BuildResult.Succeeded;
    }

    [MenuItem("Tools/Build and Launch (Server)", priority = 10)]
    public static void BuildAndLaunch1()
    {
        CloseAll();
        if (BuildGame())
        {
            Launch1();
        }
    }
    [MenuItem("Tools/Build and Launch (Server + Client) _F11", priority = 20)]
    public static void BuildAndLaunch2()
    {
        CloseAll();
        if (BuildGame())
        {
            Launch2();
        }
    }
    [MenuItem("Tools/Launch (Server)", priority = 30)]
    public static void Launch1()
    {
        Run("Builds\\MPWyzard.exe", "--server");
    }
    [MenuItem("Tools/Launch (Server + 2 Clients) _F12", priority = 40)]
    public static void Launch2()
    {
        Run("Builds\\MPWyzard.exe", "--server");
        Run("Builds\\MPWyzard.exe", "");
        Run("Builds\\MPWyzard.exe", "");
    }

    [MenuItem("Tools/Close All _F10", priority = 100)]
    public static void CloseAll()
    {
        // Get all processes with the specified name
        Process[] processes = Process.GetProcessesByName("MPWyzard");
        foreach (var process in processes)
        {
            try
            {
                // Close the process
                process.Kill();
                // Wait for the process to exit
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                // Handle exceptions, if any
                // This could occur if the process has already exited or you don't have permission to kill it
                Debug.LogWarning($"Error trying to kill process {process.ProcessName}: {ex.Message}");
            }
        }
    }


    private static void Run(string path, string args)
    {
        // Start a new process
        Process process = new Process();
        // Configure the process using the StartInfo properties
        process.StartInfo.FileName = path;
        process.StartInfo.Arguments = args;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Normal; // Choose the window style: Hidden, Minimized, Maximized, Normal
        process.StartInfo.RedirectStandardOutput = false; // Set to true to redirect the output (so you can read it in Unity)
        process.StartInfo.UseShellExecute = true; // Set to false if you want to redirect the output
                                                  // Run the process
        process.Start();
    }

#endif
}
