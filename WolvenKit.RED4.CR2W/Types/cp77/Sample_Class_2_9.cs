using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class Sample_Class_2_9 : CVariable
	{
		private CEnum<Sample_Enum_As_Bitfield_2_9> _bitField;

		[Ordinal(0)] 
		[RED("bitField")] 
		public CEnum<Sample_Enum_As_Bitfield_2_9> BitField
		{
			get => GetProperty(ref _bitField);
			set => SetProperty(ref _bitField, value);
		}

		public Sample_Class_2_9(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
