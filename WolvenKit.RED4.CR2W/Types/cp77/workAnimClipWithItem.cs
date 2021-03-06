using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class workAnimClipWithItem : workAnimClip
	{
		private CArray<CHandle<workIWorkspotItemAction>> _itemActions;

		[Ordinal(4)] 
		[RED("itemActions")] 
		public CArray<CHandle<workIWorkspotItemAction>> ItemActions
		{
			get => GetProperty(ref _itemActions);
			set => SetProperty(ref _itemActions, value);
		}

		public workAnimClipWithItem(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
