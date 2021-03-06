using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class RootMotionCommandHandler : AICommandHandlerBase
	{
		private CHandle<AIArgumentMapping> _params;

		[Ordinal(1)] 
		[RED("params")] 
		public CHandle<AIArgumentMapping> Params
		{
			get => GetProperty(ref _params);
			set => SetProperty(ref _params, value);
		}

		public RootMotionCommandHandler(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
