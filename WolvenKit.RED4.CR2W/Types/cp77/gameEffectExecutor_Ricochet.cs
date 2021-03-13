using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class gameEffectExecutor_Ricochet : gameEffectExecutor
	{
		[Ordinal(1)] [RED("outputRicochetVector")] public gameEffectOutputParameter_Vector OutputRicochetVector { get; set; }

		public gameEffectExecutor_Ricochet(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
