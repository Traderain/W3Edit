using System.IO;
using CP77.CR2W.Reflection;
using FastMember;
using static CP77.CR2W.Types.Enums;

namespace CP77.CR2W.Types
{
	[REDMeta]
	public class gameuiNewsFeedDataProvider : IScriptable
	{
		[Ordinal(0)]  [RED("newsTitleTweak")] public TweakDBID NewsTitleTweak { get; set; }
		[Ordinal(1)]  [RED("randomNewsFeedPack")] public TweakDBID RandomNewsFeedPack { get; set; }

		public gameuiNewsFeedDataProvider(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
