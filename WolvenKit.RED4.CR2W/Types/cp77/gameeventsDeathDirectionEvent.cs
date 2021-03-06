using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class gameeventsDeathDirectionEvent : redEvent
	{
		private CEnum<gameeventsDeathDirection> _direction;

		[Ordinal(0)] 
		[RED("direction")] 
		public CEnum<gameeventsDeathDirection> Direction
		{
			get => GetProperty(ref _direction);
			set => SetProperty(ref _direction, value);
		}

		public gameeventsDeathDirectionEvent(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
