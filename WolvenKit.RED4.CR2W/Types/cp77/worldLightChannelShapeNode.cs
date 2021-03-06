using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class worldLightChannelShapeNode : worldGeometryShapeNode
	{
		private CEnum<rendLightChannel> _channels;
		private CFloat _streamingDistanceFactor;

		[Ordinal(6)] 
		[RED("channels")] 
		public CEnum<rendLightChannel> Channels
		{
			get => GetProperty(ref _channels);
			set => SetProperty(ref _channels, value);
		}

		[Ordinal(7)] 
		[RED("streamingDistanceFactor")] 
		public CFloat StreamingDistanceFactor
		{
			get => GetProperty(ref _streamingDistanceFactor);
			set => SetProperty(ref _streamingDistanceFactor, value);
		}

		public worldLightChannelShapeNode(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
