using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class questConditionItem : CVariable
	{
		private CHandle<questIBaseCondition> _condition;
		private CUInt32 _socketId;

		[Ordinal(0)] 
		[RED("condition")] 
		public CHandle<questIBaseCondition> Condition
		{
			get => GetProperty(ref _condition);
			set => SetProperty(ref _condition, value);
		}

		[Ordinal(1)] 
		[RED("socketId")] 
		public CUInt32 SocketId
		{
			get => GetProperty(ref _socketId);
			set => SetProperty(ref _socketId, value);
		}

		public questConditionItem(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
