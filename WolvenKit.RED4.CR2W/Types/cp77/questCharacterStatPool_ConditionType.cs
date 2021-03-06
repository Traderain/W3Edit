using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class questCharacterStatPool_ConditionType : questICharacterConditionType
	{
		private gameEntityReference _objectRef;
		private CBool _isPlayer;
		private CFloat _percent;
		private CEnum<EComparisonType> _comparisonType;
		private CEnum<gamedataStatPoolType> _statPoolType;

		[Ordinal(0)] 
		[RED("objectRef")] 
		public gameEntityReference ObjectRef
		{
			get => GetProperty(ref _objectRef);
			set => SetProperty(ref _objectRef, value);
		}

		[Ordinal(1)] 
		[RED("isPlayer")] 
		public CBool IsPlayer
		{
			get => GetProperty(ref _isPlayer);
			set => SetProperty(ref _isPlayer, value);
		}

		[Ordinal(2)] 
		[RED("percent")] 
		public CFloat Percent
		{
			get => GetProperty(ref _percent);
			set => SetProperty(ref _percent, value);
		}

		[Ordinal(3)] 
		[RED("comparisonType")] 
		public CEnum<EComparisonType> ComparisonType
		{
			get => GetProperty(ref _comparisonType);
			set => SetProperty(ref _comparisonType, value);
		}

		[Ordinal(4)] 
		[RED("statPoolType")] 
		public CEnum<gamedataStatPoolType> StatPoolType
		{
			get => GetProperty(ref _statPoolType);
			set => SetProperty(ref _statPoolType, value);
		}

		public questCharacterStatPool_ConditionType(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
