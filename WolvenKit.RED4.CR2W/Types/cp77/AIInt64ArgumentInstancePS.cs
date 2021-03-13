using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class AIInt64ArgumentInstancePS : AIArgumentInstancePS
	{
		[Ordinal(1)] [RED("value")] public CInt64 Value { get; set; }

		public AIInt64ArgumentInstancePS(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
