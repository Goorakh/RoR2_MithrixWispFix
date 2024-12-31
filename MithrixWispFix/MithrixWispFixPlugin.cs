using BepInEx;
using RoR2;
using System.Diagnostics;
using UnityEngine;

namespace MithrixWispFix
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(R2API.R2API.PluginGUID)]
    public class MithrixWispFixPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Gorakh";
        public const string PluginName = "MithrixWispFix";
        public const string PluginVersion = "1.0.0";

        static MithrixWispFixPlugin _instance;
        internal static MithrixWispFixPlugin Instance => _instance;

        void Awake()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            SingletonHelper.Assign(ref _instance, this);

            Log.Init(Logger);

            SceneCatalog.onMostRecentSceneDefChanged += SceneCatalog_onMostRecentSceneDefChanged;

            stopwatch.Stop();
            Log.Message_NoCallerPrefix($"Initialized in {stopwatch.Elapsed.TotalMilliseconds:F0}ms");
        }

        void OnDestroy()
        {
            SingletonHelper.Unassign(ref _instance, this);
        }

        static void SceneCatalog_onMostRecentSceneDefChanged(SceneDef sceneDef)
        {
            if (sceneDef.cachedName == "moon2")
            {
                fixPhase2WispSpawnPosition();
            }
        }

        static void fixPhase2WispSpawnPosition()
        {
            SceneInfo sceneInfo = SceneInfo.instance;
            if (!sceneInfo)
            {
                Log.Error("No SceneInfo instance");
                return;
            }

            Transform brotherMissionController = sceneInfo.transform.Find("BrotherMissionController");
            if (!brotherMissionController)
            {
                Log.Error("Failed to find BrotherMissionController object");
                return;
            }

            ChildLocator brotherMissionControllerChildLocator = brotherMissionController.GetComponent<ChildLocator>();
            if (!brotherMissionControllerChildLocator)
            {
                Log.Error($"{brotherMissionController} is missing ChildLocator component");
                return;
            }

            Transform phase2Root = brotherMissionControllerChildLocator.FindChild("Phase2");
            if (!phase2Root)
            {
                Log.Error("Failed to find phase 2 root");
                return;
            }

            Transform wisp1SpawnPoint = phase2Root.Find("Lunar Wisp Spawn, 1");
            if (!wisp1SpawnPoint)
            {
                Log.Error("Failed to find wisp spawn point");
                return;
            }

            wisp1SpawnPoint.position = new Vector3(-94.6326f, 498.4218f, -85.32126f);
            Log.Debug("Fixed phase 2 wisp spawn point");
        }
    }
}
