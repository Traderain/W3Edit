using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class gameEffectExecutor_Ricochet : gameEffectExecutor
	{
		private gameEffectOutputParameter_Vector _outputRicochetVector;

		[Ordinal(1)] 
		[RED("outputRicochetVector")] 
		public gameEffectOutputParameter_Vector OutputRicochetVector
		{
			get => GetProperty(ref _outputRicochetVector);
			set => SetProperty(ref _outputRicochetVector, value);
		}

		public gameEffectExecutor_Ricochet(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
