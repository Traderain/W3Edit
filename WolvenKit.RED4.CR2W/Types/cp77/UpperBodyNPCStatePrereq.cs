using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class UpperBodyNPCStatePrereq : NPCStatePrereq
	{
		private CEnum<gamedataNPCUpperBodyState> _valueToListen;

		[Ordinal(3)] 
		[RED("valueToListen")] 
		public CEnum<gamedataNPCUpperBodyState> ValueToListen
		{
			get => GetProperty(ref _valueToListen);
			set => SetProperty(ref _valueToListen, value);
		}

		public UpperBodyNPCStatePrereq(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
