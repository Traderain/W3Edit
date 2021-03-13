using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class VentilationEffector : ActivatedDeviceTransfromAnim
	{
		[Ordinal(94)] [RED("effectComponent")] public CHandle<entIPlacedComponent> EffectComponent { get; set; }

		public VentilationEffector(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
