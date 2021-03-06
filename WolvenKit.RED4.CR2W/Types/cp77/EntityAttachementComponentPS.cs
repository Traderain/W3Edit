using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class EntityAttachementComponentPS : gameComponentPS
	{
		private CArray<EntityAttachementData> _pendingChildAttachements;

		[Ordinal(0)] 
		[RED("pendingChildAttachements")] 
		public CArray<EntityAttachementData> PendingChildAttachements
		{
			get => GetProperty(ref _pendingChildAttachements);
			set => SetProperty(ref _pendingChildAttachements, value);
		}

		public EntityAttachementComponentPS(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
