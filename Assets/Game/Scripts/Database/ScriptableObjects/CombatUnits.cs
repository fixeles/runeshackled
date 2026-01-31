using Enum;
using FPS;
using UnityEngine;

namespace Database
{
	public class CombatUnits : ScriptableObject
	{
		[SerializeField] private SerializableDictionary<UnitId, CombatUnitConfig> _units;

		public CombatUnitConfig Get(UnitId id) => _units[id];
	}
}