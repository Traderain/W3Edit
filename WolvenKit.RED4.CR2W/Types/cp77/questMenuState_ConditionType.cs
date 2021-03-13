using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class questMenuState_ConditionType : questIUIConditionType
	{
		[Ordinal(0)] [RED("state")] public CEnum<questEUIMenuState> State { get; set; }

		public questMenuState_ConditionType(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
