using CP77.CR2W.Reflection;
using FastMember;
using static CP77.CR2W.Types.Enums;

namespace CP77.CR2W.Types
{
	[REDMeta]
	public class CustomActionOperationsTriggers : DeviceOperationsTrigger
	{
		[Ordinal(0)] [RED("triggerData")] public CHandle<CustomActionOperationTriggerData> TriggerData { get; set; }

		public CustomActionOperationsTriggers(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
