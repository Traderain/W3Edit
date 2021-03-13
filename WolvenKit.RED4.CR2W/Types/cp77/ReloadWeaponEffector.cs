using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class ReloadWeaponEffector : gameEffector
	{
		[Ordinal(0)] [RED("owner")] public wCHandle<gameObject> Owner { get; set; }

		public ReloadWeaponEffector(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
