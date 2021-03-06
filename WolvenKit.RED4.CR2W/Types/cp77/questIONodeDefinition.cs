using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class questIONodeDefinition : questDisableableNodeDefinition
	{
		private CName _socketName;

		[Ordinal(2)] 
		[RED("socketName")] 
		public CName SocketName
		{
			get => GetProperty(ref _socketName);
			set => SetProperty(ref _socketName, value);
		}

		public questIONodeDefinition(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
