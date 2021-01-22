using System.IO;
using CP77.CR2W.Reflection;
using FastMember;
using static CP77.CR2W.Types.Enums;

namespace CP77.CR2W.Types
{
	[REDMeta]
	public class AnimFeature_Stamina : animAnimFeature
	{
		[Ordinal(0)]  [RED("staminaValue")] public CFloat StaminaValue { get; set; }
		[Ordinal(1)]  [RED("tiredness")] public CFloat Tiredness { get; set; }

		public AnimFeature_Stamina(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
