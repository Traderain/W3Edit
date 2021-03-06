using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class gameCursorInterpolationOverrides : inkUserData
	{
		private Vector2 _minSpeed;
		private CFloat _enterTime;

		[Ordinal(0)] 
		[RED("minSpeed")] 
		public Vector2 MinSpeed
		{
			get => GetProperty(ref _minSpeed);
			set => SetProperty(ref _minSpeed, value);
		}

		[Ordinal(1)] 
		[RED("enterTime")] 
		public CFloat EnterTime
		{
			get => GetProperty(ref _enterTime);
			set => SetProperty(ref _enterTime, value);
		}

		public gameCursorInterpolationOverrides(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
