using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class StatusEffectPrereqState : gamePrereqState
	{
		private CHandle<StatusEffectPrereqListener> _listener;

		[Ordinal(0)] 
		[RED("listener")] 
		public CHandle<StatusEffectPrereqListener> Listener
		{
			get => GetProperty(ref _listener);
			set => SetProperty(ref _listener, value);
		}

		public StatusEffectPrereqState(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
