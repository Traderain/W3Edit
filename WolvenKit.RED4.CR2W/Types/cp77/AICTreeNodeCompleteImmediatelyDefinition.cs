using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class AICTreeNodeCompleteImmediatelyDefinition : AICTreeNodeAtomicDefinition
	{
		private CBool _completeWithSuccess;

		[Ordinal(0)] 
		[RED("completeWithSuccess")] 
		public CBool CompleteWithSuccess
		{
			get => GetProperty(ref _completeWithSuccess);
			set => SetProperty(ref _completeWithSuccess, value);
		}

		public AICTreeNodeCompleteImmediatelyDefinition(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
