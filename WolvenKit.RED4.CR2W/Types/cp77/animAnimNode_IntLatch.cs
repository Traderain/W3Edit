using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class animAnimNode_IntLatch : animAnimNode_IntValue
	{
		[Ordinal(11)] [RED("input")] public animIntLink Input { get; set; }

		public animAnimNode_IntLatch(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
