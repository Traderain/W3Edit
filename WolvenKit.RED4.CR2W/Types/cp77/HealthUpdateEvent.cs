using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class HealthUpdateEvent : redEvent
	{
		private CFloat _value;
		private CFloat _healthDifference;

		[Ordinal(0)] 
		[RED("value")] 
		public CFloat Value
		{
			get => GetProperty(ref _value);
			set => SetProperty(ref _value, value);
		}

		[Ordinal(1)] 
		[RED("healthDifference")] 
		public CFloat HealthDifference
		{
			get => GetProperty(ref _healthDifference);
			set => SetProperty(ref _healthDifference, value);
		}

		public HealthUpdateEvent(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
