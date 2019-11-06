using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace Editor
{
    public class BuildWindow : EditorWindow
    {
        private enum EditorRole
        {
            Unused,
            Client
        }
        
        [Serializable]
        private class BuildWindowData
        {
            public bool allowDebugging;
            public int clientCount;
            public EditorRole editorRole;
            public List<QuickStartEntry> entries = new List<QuickStartEntry>();
        }

        [Serializable]
        private class QuickStartEntry
        {
            public bool runInEditor;

            public string GetArguments()
            {
                return "";
            }
        }

        private const float smallSpace = 5f;
        private const float largeSpace = 20f;
        private const string buildWindowDataKey = "BuildWindowData";
        private Vector2 scrollPosition;

        private BuildWindowData buildWindowData;
    
        [MenuItem("Tools/BuildWindow")]
        private static void ShowWindow()
        {
            var window = GetWindow<BuildWindow>();
            window.titleContent = new GUIContent("Build Window");
            window.Show();
        }
        
        private void OnEnable()
        {
            var str = EditorPrefs.GetString(buildWindowDataKey, "");
            buildWindowData = str != "" ? JsonUtility.FromJson<BuildWindowData>(str) : new BuildWindowData();
        }

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            EditorGUI.BeginChangeCheck();
            
            GUILayout.Space(smallSpace);
            
            DrawHeader();
            
            GUILayout.Space(largeSpace);

            DrawFolderOptions();
            
            GUILayout.Space(largeSpace);
            
            DrawBuildOptions();
            
            GUILayout.Space(largeSpace);
            
            DrawQuickStart();
            
            if (EditorGUI.EndChangeCheck())
            {
                var json = JsonUtility.ToJson(buildWindowData);
                EditorPrefs.SetString(buildWindowDataKey, json);
            }
            
            EditorGUILayout.EndScrollView();
        }

        private void DrawHeader()
        {
            EditorGUILayout.LabelField("Build Window", EditorStyles.boldLabel);
            GUILayout.Space(smallSpace);
        }

        private void DrawFolderOptions()
        {
            EditorGUILayout.LabelField("Folder options", EditorStyles.boldLabel);
            GUILayout.Space(smallSpace);
            
            var path = GetBuildPath(EditorUserBuildSettings.activeBuildTarget);
            var windowsPath = path.Replace("/", "\\");

            EditorGUILayout.TextField("Build folder", path);

            var folderExists = Directory.Exists(windowsPath);

            GUI.enabled = false;
            EditorGUILayout.Toggle("Folder exists", folderExists);
            GUI.enabled = folderExists;
            
            if (GUILayout.Button("Open build folder"))
            {
                if (folderExists)
                {
                    var p = new Process
                    {
                        StartInfo = new ProcessStartInfo("explorer.exe", windowsPath)
                    };
                    p.Start();
                }
                else
                {
                    EditorUtility.DisplayDialog("Folder missing", $"Folder {path} doesn't exist yet", "Ok");
                }
            }
            
            var defaultBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Delete build folder"))
            {
                if(folderExists)
                {
                    if (EditorUtility.DisplayDialog("Delete the folder ?",
                        $"Do you want to delete this folder : {path}", "Yes", "No"))
                        Directory.Delete(windowsPath, true);
                }
                else
                {
                    EditorUtility.DisplayDialog("Folder missing", $"Folder {path} doesn't exist yet", "Ok");
                }
            }
            GUI.backgroundColor = defaultBackgroundColor;

            GUI.enabled = true;
        }

        private void DrawBuildOptions()
        {
            EditorGUILayout.LabelField("Build options", EditorStyles.boldLabel);
            GUILayout.Space(smallSpace);
            
            buildWindowData.allowDebugging = EditorGUILayout.Toggle("Allow debugging", buildWindowData.allowDebugging);
            
            GUILayout.BeginHorizontal();

            var buildGame = false;
            var buildOnlyScripts = false;
            
            if (GUILayout.Button("Build game"))
            {
                buildGame = true;
            }
            if (GUILayout.Button("Build ONLY scripts"))
            {
                buildOnlyScripts = true;
            }

            if (buildGame || buildOnlyScripts)
            {
                var options = buildWindowData.allowDebugging ? BuildOptions.Development | BuildOptions.AllowDebugging : BuildOptions.None;
                if (buildOnlyScripts)
                    options |= BuildOptions.BuildScriptsOnly;

                var activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
                var playerOptions = new BuildPlayerOptions
                {
                    options = options,
                    target = activeBuildTarget,
                    scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
                    locationPathName = GetBuildExe()
                };
                
                BuildPipeline.BuildPlayer(playerOptions);
                GUIUtility.ExitGUI();   
            }

            GUILayout.EndHorizontal();
        }

        private void DrawQuickStart()
        {
            EditorGUILayout.LabelField("Quick start", EditorStyles.boldLabel);
            GUILayout.Space(smallSpace);
            
            var str = $"{Application.productName} - {buildWindowData.clientCount} client{(buildWindowData.clientCount > 1 ? "s" : "")}";
            if (buildWindowData.editorRole == EditorRole.Client)
            {
                str += "(1 in editor)";
            }
            EditorGUILayout.LabelField(str, EditorStyles.boldLabel);
            
            buildWindowData.clientCount = EditorGUILayout.IntField("Clients", buildWindowData.clientCount);
            if (buildWindowData.clientCount < 0) buildWindowData.clientCount = 0;
            buildWindowData.editorRole = (EditorRole) EditorGUILayout.EnumPopup("Use Editor as", buildWindowData.editorRole);

            var entryCount = buildWindowData.clientCount;
            var defaultGuiBackgroundColor = GUI.backgroundColor;
            
            GUILayout.BeginHorizontal();
            {
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Start"))
                {
                    for (var i = 0; i < entryCount; i++)
                    {
                        StartEntry(buildWindowData.entries[i]);
                    }
                }
                GUI.backgroundColor = defaultGuiBackgroundColor;

                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Stop All"))
                {
                    StopAll();
                }
                GUI.backgroundColor = defaultGuiBackgroundColor;
            }
            GUILayout.EndHorizontal();

            // Make sure we have enough entries
            var add = entryCount > buildWindowData.entries.Count;
            var min = !add ? entryCount : buildWindowData.entries.Count;
            var max = add ? entryCount : buildWindowData.entries.Count;
            if(entryCount == 0) buildWindowData.entries.Clear();
            if (entryCount > 0 && min != max)
            {
                if (add)
                {
                    buildWindowData.entries.AddRange(Enumerable.Range(min, max).Select(_ => new QuickStartEntry()));
                }
                else
                {
                    buildWindowData.entries.RemoveRange(min, max - min);
                }
            }
            
            if (entryCount > 0)
            {
                buildWindowData.entries[0].runInEditor = buildWindowData.editorRole == EditorRole.Client;
            }

            GUILayout.Label("Started processes:");
            foreach (var entry in buildWindowData.entries)
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(10);

                        GUILayout.Label(entry.runInEditor ? "Editor" : "S.Alone", GUILayout.Width(50));

                        EditorGUILayout.SelectableLabel(entry.GetArguments(), 
                            EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
        }
        
        private static void StartEntry(QuickStartEntry entry)
        {
            var args = entry.GetArguments();
            if (entry.runInEditor)
            {
                StartGameInEditor();
            }
            else
            {
                RunBuild(args);
            }
        }
        private static void StopAll()
        {
            KillAllProcesses();
            EditorApplication.isPlaying = false;
        }
        private static void RunBuild(string args)
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var buildPath = GetBuildPath(buildTarget);
            var buildExePath = GetBuildExePath(buildTarget);
            Debug.Log($"Starting {buildExePath} {args}");
            var process = new Process
            {
                StartInfo =
                {
                    UseShellExecute = args.Contains("-batchmode"),
                    FileName = $"{Application.dataPath}/../{buildExePath}",
                    Arguments = args,
                    WorkingDirectory = buildPath
                }
            };
            process.Start();
        }
        private static void KillAllProcesses()
        {
            var buildExe = GetBuildExe();

            var processName = Path.GetFileNameWithoutExtension(buildExe);
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                if (process.HasExited)
                    continue;

                try
                {
                    if (process.ProcessName == processName)
                    {
                        process.Kill();
                    }
                }
                catch (InvalidOperationException)
                {

                }
            }
        }
        private static void StartGameInEditor()
        {
            var path = EditorBuildSettings.scenes[0].path;
            EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            EditorApplication.EnterPlaymode();
        }

        private static string GetBuildPath(BuildTarget buildTarget)
        {
            return $"Builds/{buildTarget}";
        }
        private static string GetBuildExe()
        {
            return $"{Application.productName}.exe";
        }

        private static string GetBuildExePath(BuildTarget buildTarget)
        {
            return $"{GetBuildPath(buildTarget)}/{GetBuildExe()}";
        }
    }
}