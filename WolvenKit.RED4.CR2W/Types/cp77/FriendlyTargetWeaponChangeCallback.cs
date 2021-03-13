using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class FriendlyTargetWeaponChangeCallback : gameAttachmentSlotsScriptCallback
	{
		[Ordinal(2)] [RED("followerRole")] public CHandle<AIFollowerRole> FollowerRole { get; set; }

		public FriendlyTargetWeaponChangeCallback(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
