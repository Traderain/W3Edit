using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class EffectExecutor_GameObjectOutline : gameEffectExecutor_Scripted
	{
		private CEnum<EOutlineType> _outlineType;

		[Ordinal(1)] 
		[RED("outlineType")] 
		public CEnum<EOutlineType> OutlineType
		{
			get => GetProperty(ref _outlineType);
			set => SetProperty(ref _outlineType, value);
		}

		public EffectExecutor_GameObjectOutline(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
