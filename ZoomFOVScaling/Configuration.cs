using BepInEx.Configuration;
using BepInEx;
using System.IO;
using GTFO.API.Utilities;
using System;

namespace ZoomFOVScaling
{
    internal static class Configuration
    {
        public static float BaseFOV { get; set; } = 70f;
        public static bool FOVAffectWeaponZoom { get; set; } = false;
        public static bool FOVAffectWeaponHip { get; set; } = false;

        private readonly static ConfigFile configFile;

        static Configuration()
        {
            configFile = new ConfigFile(Path.Combine(Paths.ConfigPath, EntryPoint.MODNAME + ".cfg"), saveOnInit: true);
            BindAll(configFile);
        }

        internal static void Init()
        {
            LiveEditListener listener = LiveEdit.CreateListener(Paths.ConfigPath, EntryPoint.MODNAME + ".cfg", false);
            listener.FileChanged += OnFileChanged;
        }

        private static void OnFileChanged(LiveEditEventArgs _)
        {
            string section = "Base Settings";
            configFile.Reload();
            BaseFOV = Math.Max((float)configFile[section, "Base Field of View"].BoxedValue, 1f);
            FOVAffectWeaponZoom = (bool)configFile[section, "Zoom FOV Affects Weapons"].BoxedValue;
            FOVAffectWeaponHip = (bool)configFile[section, "Hip FOV Affects Weapons"].BoxedValue;
        }

        private static void BindAll(ConfigFile config)
        {
            string section = "Base Settings";
            BaseFOV = config.Bind(section, "Base Field of View", BaseFOV, "The field of view considered as 1x zoom.").Value;
            FOVAffectWeaponZoom = config.Bind(section, "Zoom FOV Affects Weapons", FOVAffectWeaponZoom, "Field of view modifies weapon FOV while aiming.").Value;
            FOVAffectWeaponHip = config.Bind(section, "Hip FOV Affects Weapons", FOVAffectWeaponHip, "Field of view modifies weapon FOV while not aiming.").Value;
        }
    }
}
