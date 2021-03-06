using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class animVisualTagCondition : animIStaticCondition
	{
		private CName _visualTag;

		[Ordinal(0)] 
		[RED("visualTag")] 
		public CName VisualTag
		{
			get => GetProperty(ref _visualTag);
			set => SetProperty(ref _visualTag, value);
		}

		public animVisualTagCondition(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
