using CP77.CR2W.Reflection;

namespace CP77.CR2W.Types
{
	[REDMeta]
	public class SceneCustomData_Selection : ISceneStorageCustomData
	{
		public SceneCustomData_Selection(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
