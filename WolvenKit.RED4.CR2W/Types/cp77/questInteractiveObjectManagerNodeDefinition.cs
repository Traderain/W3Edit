using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class questInteractiveObjectManagerNodeDefinition : questDisableableNodeDefinition
	{
		private CHandle<questIInteractiveObjectManagerNodeType> _type;

		[Ordinal(2)] 
		[RED("type")] 
		public CHandle<questIInteractiveObjectManagerNodeType> Type
		{
			get => GetProperty(ref _type);
			set => SetProperty(ref _type, value);
		}

		public questInteractiveObjectManagerNodeDefinition(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
