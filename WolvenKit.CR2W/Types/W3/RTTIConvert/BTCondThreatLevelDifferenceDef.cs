using System.IO;
using System.Runtime.Serialization;
using WolvenKit.CR2W.Reflection;
using FastMember;
using static WolvenKit.CR2W.Types.Enums;


namespace WolvenKit.CR2W.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class BTCondThreatLevelDifferenceDef : IBehTreeConditionalTaskDefinition
	{
		[Ordinal(1)] [RED("operator")] 		public CEnum<EOperator> Operator { get; set;}

		[Ordinal(2)] [RED("value")] 		public CInt32 Value { get; set;}

		[Ordinal(3)] [RED("useCombatTarget")] 		public CBool UseCombatTarget { get; set;}

		[Ordinal(4)] [RED("useNamedTarget")] 		public CBehTreeValCName UseNamedTarget { get; set;}

		[Ordinal(5)] [RED("saveTargetOnGameplayEvent")] 		public CBehTreeValCName SaveTargetOnGameplayEvent { get; set;}

		public BTCondThreatLevelDifferenceDef(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new BTCondThreatLevelDifferenceDef(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}