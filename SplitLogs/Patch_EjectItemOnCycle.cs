using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using HarmonyLib;
using BepInEx.Configuration;

namespace SplitLogs
{
    [HarmonyPatch(typeof(ItemDepositAndChanger), "ejectItemOnCycle")]
    public static class Patch_EjectItemOnCycle
    {
		public static ConfigEntry<int> PlankQuantity { get; set; }

		public static ConfigEntry<bool> CanSplitMetal { get; set; }

		public static ConfigEntry<int> SheetQuantity { get; set; }

		public static void Postfix(ItemDepositAndChanger __instance, int xPos, int yPos, HouseDetails inside = null)
		{
			int num = PlankQuantity.Value;
			if (num < 0)
			{
				num = 2;
			}
			num--;
			ItemChange itemChange = Inventory.Instance.allItems[(inside != null) ? inside.houseMapOnTileStatus[xPos, yPos] : WorldManager.Instance.onTileStatusMap[xPos, yPos]].itemChange;
			ItemChangeType[] changesAndTheirChanger = itemChange.changesAndTheirChanger;
			int num2 = ((inside != null) ? inside.houseMapOnTile[xPos, yPos] : WorldManager.Instance.onTileMap[xPos, yPos]);
			ItemChangeType[] array = changesAndTheirChanger;
			foreach (ItemChangeType val in array)
			{
				if (val.depositInto.tileObjectId == num2)
				{
					InventoryItem item = Inventory.Instance.allItems[inside != null ? inside.houseMapOnTileStatus[xPos, yPos] : WorldManager.Instance.onTileStatusMap[xPos, yPos]];
					if (val.taskType == DailyTaskGenerator.genericTaskType.CutPlanks)
					{
						NetworkMapSharer.Instance.spawnAServerDropToSave(item.itemChange.getChangerResultId(inside != null ? inside.houseMapOnTile[xPos, yPos] : WorldManager.Instance.onTileMap[xPos, yPos]), num, __instance.ejectPos.position, inside, false, -1);
						DailyTaskGenerator.generate.doATask(DailyTaskGenerator.genericTaskType.CutPlanks, num);
						break;
					}
					else if (CanSplitMetal.Value && item.itemName == "Tin Bar")
					{
						NetworkMapSharer.Instance.spawnAServerDropToSave(item.itemChange.getChangerResultId(inside != null ? inside.houseMapOnTile[xPos, yPos] : WorldManager.Instance.onTileMap[xPos, yPos]), num, __instance.ejectPos.position, inside, false, -1);
						break;
					}
				}
			}
		}
	}
}
