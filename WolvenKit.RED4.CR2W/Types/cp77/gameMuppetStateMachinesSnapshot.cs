using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class gameMuppetStateMachinesSnapshot : CVariable
	{
		private CArray<gameMuppetStateMachineSnapshot> _stateMachines;

		[Ordinal(0)] 
		[RED("stateMachines")] 
		public CArray<gameMuppetStateMachineSnapshot> StateMachines
		{
			get => GetProperty(ref _stateMachines);
			set => SetProperty(ref _stateMachines, value);
		}

		public gameMuppetStateMachinesSnapshot(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
