using System.IO;
using System.Runtime.Serialization;
using WolvenKit.CR2W.Reflection;
using FastMember;
using static WolvenKit.CR2W.Types.Enums;


namespace WolvenKit.CR2W.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CAIDynamicWanderParams : CAINpcWanderParams
	{
		[Ordinal(1)] [RED("dynamicWanderArea")] 		public EntityHandle DynamicWanderArea { get; set;}

		[Ordinal(2)] [RED("dynamicWanderIdleDuration")] 		public CFloat DynamicWanderIdleDuration { get; set;}

		[Ordinal(3)] [RED("dynamicWanderIdleChance")] 		public CFloat DynamicWanderIdleChance { get; set;}

		[Ordinal(4)] [RED("dynamicWanderMoveDuration")] 		public CFloat DynamicWanderMoveDuration { get; set;}

		[Ordinal(5)] [RED("dynamicWanderMoveChance")] 		public CFloat DynamicWanderMoveChance { get; set;}

		public CAIDynamicWanderParams(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CAIDynamicWanderParams(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}