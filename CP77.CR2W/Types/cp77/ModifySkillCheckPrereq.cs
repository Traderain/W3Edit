using System.IO;
using CP77.CR2W.Reflection;
using FastMember;
using static CP77.CR2W.Types.Enums;

namespace CP77.CR2W.Types
{
	[REDMeta]
	public class ModifySkillCheckPrereq : gamePlayerScriptableSystemRequest
	{
		[Ordinal(0)]  [RED("register")] public CBool Register { get; set; }
		[Ordinal(1)]  [RED("skillCheckState")] public CHandle<SkillCheckPrereqState> SkillCheckState { get; set; }

		public ModifySkillCheckPrereq(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
