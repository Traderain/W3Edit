using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class RecipientData : CVariable
	{
		[Ordinal(0)] [RED("fuseID")] public CInt32 FuseID { get; set; }
		[Ordinal(1)] [RED("entryID")] public CInt32 EntryID { get; set; }

		public RecipientData(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
