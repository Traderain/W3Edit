using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class questAudioCharacterManagerNodeDefinition : questDisableableNodeDefinition
	{
		private CHandle<questIAudioCharacterManager_NodeType> _type;

		[Ordinal(2)] 
		[RED("type")] 
		public CHandle<questIAudioCharacterManager_NodeType> Type
		{
			get => GetProperty(ref _type);
			set => SetProperty(ref _type, value);
		}

		public questAudioCharacterManagerNodeDefinition(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
