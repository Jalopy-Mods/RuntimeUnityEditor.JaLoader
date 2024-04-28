using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using JaLoader;
using System.Runtime.CompilerServices;
using RuntimeUnityEditor;
using RuntimeUnityEditor.Core;
using RuntimeUnityEditor.Core.Utils.Abstractions;
using Console = JaLoader.Console;

namespace RuntimeUnityEditor.JaLoader
{
    public class RuntimeUnityEditor5 : Mod
    {
        public override string ModID => "RuntimeUnityEditor5";
        public override string ModName => "Runtime Unity Editor";
        public override string ModAuthor => "ManlyMarco, Leaxx";
        public override string ModDescription => "In-game inspector, editor and interactive console for applications made with Unity3D game engine. It's designed for debugging and modding Unity games, but can also be used as a universal trainer.\n\nOriginal by ManlyMarco\nPorted to JaLoader by Leaxx.";
        public override string ModVersion => "1.0.0";
        public override string GitHubLink => "https://github.com/Jalopy-Mods/RuntimeUnityEditor.JaLoader";
        public override WhenToInit WhenToInit => WhenToInit.InMenu;
        public override List<(string, string, string)> Dependencies => new List<(string, string, string)>()
        {
            ("JaLoader", "Leaxx", "3.0.0")
        };

        public override bool UseAssets => false;

        public static RuntimeUnityEditorCore Instance { get; private set; }

        public override void Start()
        {
            base.Start();

            Instance = new RuntimeUnityEditorCore(new JaLoaderInitSettings(this));
            Instance.ShowHotkey = GetPrimaryKeybind("ToggleInspector");
        }

        public override void SettingsDeclaration()
        {
            base.SettingsDeclaration();

            InstantiateSettings();

            AddToggle("LogEverything", "Log every Debug.Log and its variants into the Console", true);
            AddKeybind("ToggleInspector", "Toggle the Inspector", KeyCode.RightShift);
        }

        public override void OnEnable()
        {
            base.OnEnable();

            Console.Instance.LogEverythingFromDebug = GetToggleValue("LogEverything");
        }

        public override void OnDisable()
        {
            base.OnDisable();

            Console.Instance.LogEverythingFromDebug = false;
        }

        public override void Update()
        {
            base.Update();

            Instance.Update();
        }

        public void LateUpdate()
        {
            Instance.LateUpdate();
        }

        private void OnGUI()
        {
            Instance.OnGUI();
        }

        public sealed class JaLoaderInitSettings : InitSettings
        {
            private readonly RuntimeUnityEditor5 _instance;

            public override string ConfigPath => throw new NotImplementedException();

            public override ILoggerWrapper LoggerWrapper { get; }

            public override MonoBehaviour PluginMonoBehaviour
            {
                get
                {
                    return _instance;
                }
            }

            public override Action<T> RegisterSetting<T>(string category, string name, T defaultValue, string description, Action<T> onValueUpdated)
            {
                return null;
            }

            public JaLoaderInitSettings(RuntimeUnityEditor5 instance)
            {
                _instance = instance;
                LoggerWrapper = new Logger5();
            }
        }

        public sealed class Logger5 : ILoggerWrapper
        {
            public void Log(LogLevel logLevel, object content)
            {
                switch (logLevel)
                {
                    case LogLevel.Debug:
                        Console.LogDebug(content);
                        break;
                    case LogLevel.Info:
                        Console.LogDebug(content);
                        break;
                    case LogLevel.Warning:
                        Console.LogWarning(content);
                        break;
                    case LogLevel.Error:
                        Console.LogError(content);
                        break;
                }
            }
        }
    }
}
