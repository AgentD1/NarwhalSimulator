using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class DamageLayers {
	public static Dictionary<string, Dictionary<string, bool>> damageLayers = new Dictionary<string, Dictionary<string, bool>>() {
		{ "Friendly", new Dictionary<string, bool> { { "Friendly", false }, { "Unfriendly", true } } },
		{ "Unfriendly", new Dictionary<string, bool> { { "Friendly", true }, { "Unfriendly", true } } }
	}; // Ugh. This is so ugly to initialize, but it looks nice to access. Maybe change it to look better?

	public static bool CanDamage(string damager, string damagee) {
		return damageLayers[damager][damagee];
	}
}
