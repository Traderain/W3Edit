using CP77.CR2W.Reflection;
using FastMember;
using static CP77.CR2W.Types.Enums;

namespace CP77.CR2W.Types
{
	[REDMeta]
	public class RegisterPoliceCaller : gameScriptableSystemRequest
	{
		[Ordinal(0)] [RED("caller")] public wCHandle<entEntity> Caller { get; set; }
		[Ordinal(1)] [RED("crimePosition")] public Vector4 CrimePosition { get; set; }

		public RegisterPoliceCaller(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
