using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class animAnimNode_OnePoseInput : animAnimNode_Base
	{
		private animPoseLink _inputLink;

		[Ordinal(11)] 
		[RED("inputLink")] 
		public animPoseLink InputLink
		{
			get => GetProperty(ref _inputLink);
			set => SetProperty(ref _inputLink, value);
		}

		public animAnimNode_OnePoseInput(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
