using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.CustomRoles.API.Features;
using ProjectMER.Events.Handlers;
using ProjectMER.Features.Objects;
using Scp999.Features.Controller;

using Scp999.Features.Manager;

using Scp999.Interfaces;

using UnityEngine;

using YamlDotNet.Core.Tokens;

namespace Scp999
{
	public abstract class AbilityRole : CustomRole
	{
		public virtual SchematicObject Schematic { get; set; }

		public virtual AudioPlayer Audio { get; set; }

		public virtual Animator Animator { get; set; }

		public override void AddRole(Player player)
		{
			base.AddRole(player);
		}

		public virtual void AddRole(Player player, SchematicObject schematic, int volume = 100)
		{
			AddRole(player);

			KeybindManager.RegisterKeybindsForPlayer(player);
			HintManager.AddHint(player);
			InvisibleManager.MakeInvisible(player);

			Audio = AudioPlayer.Create()
			Animator = SchematicManager.GetAnimatorFromSchematic(schematic);
		}

		public override void RemoveRole(Player player)
		{


			base.RemoveRole(player);
		}
	}
}
