using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class gameuiPhotoModeUIVisibilityEvent : redEvent
	{
		private CBool _visible;

		[Ordinal(0)] 
		[RED("visible")] 
		public CBool Visible
		{
			get => GetProperty(ref _visible);
			set => SetProperty(ref _visible, value);
		}

		public gameuiPhotoModeUIVisibilityEvent(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
