﻿namespace CustomRoles.Features
{
	using System.Collections.Generic;

	using CustomPlayerEffects;

	using Exiled.API.Enums;
	using Exiled.API.Features;
	using Exiled.API.Features.Spawn;
	using Exiled.CustomRoles.API.Features;

	using MEC;

	using PlayerRoles;

	using CustomRoles.Features.Controller;

	using UnityEngine;

	public class Scp999Role : CustomRole
	{
		public override string Name { get; set; } = "SCP-999";
		public override string Description { get; set; } = "The tickle monster";
		public override string CustomInfo { get; set; } = "SCP-999";
		public override uint Id { get; set; } = 9999;
		public override int MaxHealth { get; set; } = 2000;
		public override SpawnProperties SpawnProperties { get; set; } = new()
		{
			Limit = 1,
			DynamicSpawnPoints = new List<DynamicSpawnPoint>()
			{
				new() { Chance = 25, Location = SpawnLocationType.Inside330Chamber },
				new() { Chance = 25, Location = SpawnLocationType.Inside914 },
				new() { Chance = 25, Location = SpawnLocationType.InsideGr18 },
				new() { Chance = 25, Location = SpawnLocationType.InsideLczWc }
			}
		};

		public override bool KeepPositionOnSpawn { get; set; } = true;
		public override bool KeepInventoryOnSpawn { get; set; } = false;
		public override bool RemovalKillsPlayer { get; set; } = true;
		public override bool KeepRoleOnDeath { get; set; } = false;
		public override bool IgnoreSpawnSystem { get; set; } = true;
		public override bool KeepRoleOnChangingRole { get; set; } = false;
		public override RoleTypeId Role { get; set; } = RoleTypeId.Tutorial;

		public override Exiled.API.Features.Broadcast Broadcast { get; set; } = new()
		{
			Show = true,
			Content =
				"<color=#ffa500>\ud83d\ude04 You are SCP-999 - The tickle monster! \ud83d\ude04\n" +
				"Heal Humans, dance and calm down SCPs in facility\n" +
				"Use abilities by clicking on the buttons</color>",
			Duration = 15
		};

		protected override void ShowMessage(Player player) { }

		public override string ConsoleMessage { get; set; } =
			"You are SCP-999 - The tickle monster!\n" +
			"You have a lot of abilities, for example, you can heal players or dance.\n" +
			"Configure your buttons in the settings. Remove the stars.";

		/// <summary>
		/// Adding the SCP-999 role to the player
		/// </summary>
		/// <param name="player">The player who should become SCP-999</param>
		public override void AddRole(Player player)
		{
			// Setup of a custom role
			base.AddRole(player);
			player.CustomName = this.Name;
			player.EnableEffect<Disabled>();
			player.EnableEffect<SilentWalk>();
			player.ChangeEffectIntensity<SilentWalk>(10);
			player.IsMuted = true;

			Timing.CallDelayed(0.1f, () =>
			{
				player.EnableEffect<Ghostly>();
			});

			// Register PlayerComponent for player
			player.GameObject.AddComponent<PlayerController2>();
		}

		/// <summary>
		/// Remove the role from the player
		/// </summary>
		/// <param name="player">A player who should become normal role</param>
		public override void RemoveRole(Player player)
		{
			// Remove a custom role
			base.RemoveRole(player);
			player.CustomName = null;
			player.IsMuted = false;

			// Unregister PlayerComponent for player
			Object.Destroy(player.GameObject.GetComponent<PlayerController2>());
		}
	}
}