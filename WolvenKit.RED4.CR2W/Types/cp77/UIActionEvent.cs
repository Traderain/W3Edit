using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class UIActionEvent : redEvent
	{
		private CHandle<gamedeviceAction> _action;
		private wCHandle<gameObject> _executor;

		[Ordinal(0)] 
		[RED("action")] 
		public CHandle<gamedeviceAction> Action
		{
			get => GetProperty(ref _action);
			set => SetProperty(ref _action, value);
		}

		[Ordinal(1)] 
		[RED("executor")] 
		public wCHandle<gameObject> Executor
		{
			get => GetProperty(ref _executor);
			set => SetProperty(ref _executor, value);
		}

		public UIActionEvent(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
