using UnityEngine;
using System.Collections;
using System.Linq;

public class SpawnVillagers : MonoBehaviour {
	public static int numberPerHut = 5;

	public static void AttemptSpawnVillager() {
		if(!AtVillagerCapacity()) {
			var hut = Hut.RandomHut();

			hut.SpawnVillager();
		}
	}

	public static int TotalVillagerCapacity() {
		return Hut.AllHuts().Count() * numberPerHut;
	}

	public static bool AtVillagerCapacity() {
		return Villager.AllVillagers().Count() >= TotalVillagerCapacity();
	}
}
