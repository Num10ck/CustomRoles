namespace Scp999
{
	using System.Collections.Generic;

	using Exiled.API.Features;
	using Exiled.API.Features.Core.UserSettings;

	using ProjectMER.Features.Objects;

	using Scp999.Features.Controller;

	using Scp999.Features.Manager;
	using Scp999.Interfaces;

	using UnityEngine;

	public abstract class Role
	{
		public SchematicObject Schematic { get; set; }

		public AudioPlayer Audio { get; set; }

		public Animator Animator { get; set; }

		public Speaker Speaker { get; set; }

		protected MovementController MovementController { get; set; }

		public virtual void AddRole(
			Player player, 
			SchematicObject schematic, 
			IEnumerable<IAbility> abilities, 
			int volume = 100)
		{
			Audio = AudioManager.AddAudioPlayer(player, volume);
			
			if (!Audio.TryGetSpeaker("scp999-speaker", out Speaker speaker))
			{
				Log.Error("Failed to get Speaker from custom role.");

				return;
			}

			Speaker = speaker;

			Animator = SchematicManager.GetAnimatorFromSchematic(schematic);

			HeaderSetting setting = new(
				"Custom role",
				"Abilities"
			);

			KeybindManager.RegisterKeybindsForPlayer(player, setting, abilities);
			HintManager.AddHint(player);
			InvisibleManager.MakeInvisible(player);

			MovementController = player.GameObject.AddComponent<MovementController>();

			MovementController.Init(
				Schematic, 
				Speaker, 
				Schematic.transform.localPosition);
		}

		public virtual void Remove(Player player)
		{
			GameObject.Destroy(MovementController);

			InvisibleManager.RemoveInvisible(player);
			KeybindManager.UnregisterKeybindsForPlayer(player);
			HintManager.RemoveHint(player);

			Audio.RemoveAllClips();
			Audio.Destroy();

			Schematic.Destroy();
		}
	}
}
