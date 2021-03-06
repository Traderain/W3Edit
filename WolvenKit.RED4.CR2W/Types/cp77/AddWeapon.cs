using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class AddWeapon : AIbehaviortaskScript
	{
		private CEnum<EquipmentPriority> _weapon;

		[Ordinal(0)] 
		[RED("weapon")] 
		public CEnum<EquipmentPriority> Weapon
		{
			get => GetProperty(ref _weapon);
			set => SetProperty(ref _weapon, value);
		}

		public AddWeapon(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
