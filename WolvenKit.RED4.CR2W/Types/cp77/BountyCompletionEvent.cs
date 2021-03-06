using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class BountyCompletionEvent : redEvent
	{
		private CInt32 _streetCredAwarded;
		private CInt32 _moneyAwarded;

		[Ordinal(0)] 
		[RED("streetCredAwarded")] 
		public CInt32 StreetCredAwarded
		{
			get => GetProperty(ref _streetCredAwarded);
			set => SetProperty(ref _streetCredAwarded, value);
		}

		[Ordinal(1)] 
		[RED("moneyAwarded")] 
		public CInt32 MoneyAwarded
		{
			get => GetProperty(ref _moneyAwarded);
			set => SetProperty(ref _moneyAwarded, value);
		}

		public BountyCompletionEvent(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
