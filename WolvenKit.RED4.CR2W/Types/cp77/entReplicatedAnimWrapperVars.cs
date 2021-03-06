using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class entReplicatedAnimWrapperVars : CVariable
	{
		private netTime _serverReplicatedTime;
		private CArray<entReplicatedVariableValue> _data;

		[Ordinal(0)] 
		[RED("serverReplicatedTime")] 
		public netTime ServerReplicatedTime
		{
			get => GetProperty(ref _serverReplicatedTime);
			set => SetProperty(ref _serverReplicatedTime, value);
		}

		[Ordinal(1)] 
		[RED("data")] 
		public CArray<entReplicatedVariableValue> Data
		{
			get => GetProperty(ref _data);
			set => SetProperty(ref _data, value);
		}

		public entReplicatedAnimWrapperVars(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
