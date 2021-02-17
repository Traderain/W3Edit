using CP77.CR2W.Reflection;
using FastMember;
using static CP77.CR2W.Types.Enums;

namespace CP77.CR2W.Types
{
	[REDMeta]
	public class audioVoiceTagAppearanceMapping : audioAudioMetadata
	{
		[Ordinal(1)] [RED("mappings")] public CArray<audioVoiceTagAppearanceGroup> Mappings { get; set; }

		public audioVoiceTagAppearanceMapping(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
