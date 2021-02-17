using CP77.CR2W.Reflection;
using FastMember;
using static CP77.CR2W.Types.Enums;

namespace CP77.CR2W.Types
{
	[REDMeta]
	public class OpenShardNotificationAction : GenericNotificationBaseAction
	{
		[Ordinal(0)] [RED("eventDispatcher")] public CHandle<gameuiGameSystemUI> EventDispatcher { get; set; }

		public OpenShardNotificationAction(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
