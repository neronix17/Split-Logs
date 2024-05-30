using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SplitLogs
{
    [BepInPlugin("neronix17.dinkum.splitlogs", "Split Logs", "1.0.0")]
    public class SplitLogs : BaseUnityPlugin
	{
		public const string pluginGuid = "neronix17.dinkum.splitlogs";

		public const string pluginName = "Split Logs";

		public const string pluginVersion = "1.0.0";

		public ConfigEntry<int> PlankQuantity;

		public ConfigEntry<bool> CanSplitMetal { get; set; }

		public ConfigEntry<int> SheetQuantity { get; set; }

		public void Awake()
		{
			Config.Bind("!Developer", "NexusID", 340, "Nexus Mod ID, automatically generated, changing it will do nothing.");
			PlankQuantity = Config.Bind("Mod General", "Planks Quantity", 2, "Quantity of planks to drop when cutting a log on the table saw (including the one dropped by default)");
			CanSplitMetal = Config.Bind("Mod General", "Can Split Metal", false, "If true, tin ingots can be split into multiple sheets.");
			SheetQuantity = Config.Bind("Mod General", "Sheets Quantity", 2, "Quantity of sheets to drop when cutting a tin ingot on the table saw (including the one dropped by default)");
			Logger.LogInfo(":: Split Logs :: Successfully Loaded ::");
			Harmony harmony = new Harmony("neronix17.dinkum.splitlogs");
			harmony.PatchAll();
			Patch_EjectItemOnCycle.PlankQuantity = PlankQuantity;
			Patch_EjectItemOnCycle.CanSplitMetal = CanSplitMetal;
			Patch_EjectItemOnCycle.SheetQuantity = SheetQuantity;
		}
	}
}
