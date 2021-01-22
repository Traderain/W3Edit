using System.IO;
using CP77.CR2W.Reflection;
using FastMember;
using static CP77.CR2W.Types.Enums;

namespace CP77.CR2W.Types
{
	[REDMeta]
	public class aiscriptSharedVarFloat : CVariable
	{
		[Ordinal(0)]  [RED("varName")] public LibTreeSharedVarReferenceName VarName { get; set; }

		public aiscriptSharedVarFloat(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
