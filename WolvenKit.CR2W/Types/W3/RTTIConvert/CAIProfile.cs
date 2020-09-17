using System.IO;
using System.Runtime.Serialization;
using WolvenKit.CR2W.Reflection;
using FastMember;
using static WolvenKit.CR2W.Types.Enums;


namespace WolvenKit.CR2W.Types
{
	[DataContract(Namespace = "")]
	[REDMeta]
	public class CAIProfile : CEntityTemplateParam
	{
		[Ordinal(1)] [RED("reactions", 2,0)] 		public CArray<CHandle<CAIReaction>> Reactions { get; set;}

		[Ordinal(2)] [RED("senseVisionParams")] 		public CPtr<CAISenseParams> SenseVisionParams { get; set;}

		[Ordinal(3)] [RED("senseAbsoluteParams")] 		public CPtr<CAISenseParams> SenseAbsoluteParams { get; set;}

		[Ordinal(4)] [RED("attitudeGroup")] 		public CName AttitudeGroup { get; set;}

		[Ordinal(5)] [RED("minigameParams")] 		public SAIMinigameParams MinigameParams { get; set;}

		[Ordinal(6)] [RED("aiWizardRes")] 		public CHandle<CResource> AiWizardRes { get; set;}

		public CAIProfile(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name){ }

		public static new CVariable Create(CR2WFile cr2w, CVariable parent, string name) => new CAIProfile(cr2w, parent, name);

		public override void Read(BinaryReader file, uint size) => base.Read(file, size);

		public override void Write(BinaryWriter file) => base.Write(file);

	}
}