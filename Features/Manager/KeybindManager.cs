using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;

using Scp999.Interfaces;

namespace Scp999.Features.Manager;
public static class KeybindManager
{
	private readonly static Dictionary<Player, List<SettingBase>> _settings;

    public static void RegisterKeybindsForPlayer(
		Player player, 
		HeaderSetting header, 
		IEnumerable<IAbility> abilities)
    {
		List<SettingBase> settings =[header];

		_settings.Add(player, []);

		foreach (IAbility ability in abilities)
		{
			KeybindSetting setting = new(
				ability.KeyId,
				ability.Name,
				ability.KeyCode,
				false,
				false,
				ability.Description
			);

			_settings[player].Add(setting);
		}

		SettingBase.Register(player, settings);
    }

    public static void UnregisterKeybindsForPlayer(Player player)
    {
        SettingBase.Unregister(player, _settings[player]);
    }

	static KeybindManager()
	{
		_settings =[];
	}
}