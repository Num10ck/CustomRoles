﻿namespace CustomRoles
{
	using System;
	using System.IO;
	using System.Linq;

	using Exiled.API.Features;
	using Exiled.CustomRoles.API;

	using HarmonyLib;

	using CustomRoles.Features.Managers;

	public class Plugin : Plugin<Config>
	{
		public override string Name => "Scp999";
		public override string Author => "RisottoMan";
		public override Version Version => new(1, 0, 2);
		public override Version RequiredExiledVersion => new(9, 6, 0);

		private Harmony _harmony;
		private EventHandler _eventHandler;
		public static Plugin Singleton;

		// Configs path
		public string BasePath { get; set; }
		public string SchematicPath { get; set; }
		public string AudioPath { get; set; }

		public override void OnEnabled()
		{
			Singleton = this;
			_eventHandler = new EventHandler(this);

			// Patch
			_harmony = new Harmony($"risottoman.scp999");
			_harmony.PatchAll();

			// Checking that the ProjectMER plugin is loaded on the server
			if (!AppDomain.CurrentDomain.GetAssemblies().Any(x => x.FullName.ToLower().Contains("projectmer")))
			{
				Log.Error("ProjectMER is not installed. Schematics can't spawn the SCP-999 game model.");
				return;
			}

			// Checking that the HintServiceMeow plugin is loaded on the server
			if (!AppDomain.CurrentDomain.GetAssemblies().Any(x => x.FullName.ToLower().Contains("hintservicemeow")))
			{
				Log.Error("HintServiceMeow is not installed. There is no way to give the player a hint.");
				return;
			}

			// Register the custom scp999 role
			Config.Scp999RoleConfig.Register();

			// Create config folders
			BasePath = Path.Combine(Paths.IndividualConfigs, this.Name.ToLower());
			SchematicPath = Path.Combine(BasePath, "Schematics");
			AudioPath = Path.Combine(BasePath, "Audio");
			this.CreatePluginDirectory(SchematicPath);
			this.CreatePluginDirectory(AudioPath);

			// Register the abilities
			AbilityRegistrator.RegisterAbilities();

			base.OnEnabled();
		}

		public override void OnDisabled()
		{
			AbilityRegistrator.UnregisterAbilities();

			Config.Scp999RoleConfig.Unregister();
			_harmony.UnpatchAll();

			_eventHandler = null;
			Singleton = null;

			base.OnDisabled();
		}

		private void CreatePluginDirectory(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}
	}
}