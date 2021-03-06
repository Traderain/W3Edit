using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class HitFlagPrereq : GenericHitPrereq
	{
		private CEnum<hitFlag> _flag;

		[Ordinal(5)] 
		[RED("flag")] 
		public CEnum<hitFlag> Flag
		{
			get => GetProperty(ref _flag);
			set => SetProperty(ref _flag, value);
		}

		public HitFlagPrereq(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
