using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class inkPhotoModeCursorStateChangedEvent : redEvent
	{
		private CBool _cursorEnabled;

		[Ordinal(0)] 
		[RED("cursorEnabled")] 
		public CBool CursorEnabled
		{
			get => GetProperty(ref _cursorEnabled);
			set => SetProperty(ref _cursorEnabled, value);
		}

		public inkPhotoModeCursorStateChangedEvent(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
