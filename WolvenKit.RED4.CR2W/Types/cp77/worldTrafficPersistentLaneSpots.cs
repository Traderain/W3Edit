using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class worldTrafficPersistentLaneSpots : CVariable
	{
		private CArray<CHandle<worldTrafficSpotCompiled>> _spots;

		[Ordinal(0)] 
		[RED("spots")] 
		public CArray<CHandle<worldTrafficSpotCompiled>> Spots
		{
			get => GetProperty(ref _spots);
			set => SetProperty(ref _spots, value);
		}

		public worldTrafficPersistentLaneSpots(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
