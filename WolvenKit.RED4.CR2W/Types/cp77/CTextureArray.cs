using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class CTextureArray : ITexture
	{
		private STextureGroupSetup _setup;
		private CHandle<IRenderResourceBlob> _renderResourceBlob;
		private rendRenderTextureResource _renderTextureResource;

		[Ordinal(1)] 
		[RED("setup")] 
		public STextureGroupSetup Setup
		{
			get => GetProperty(ref _setup);
			set => SetProperty(ref _setup, value);
		}

		[Ordinal(2)] 
		[RED("renderResourceBlob")] 
		public CHandle<IRenderResourceBlob> RenderResourceBlob
		{
			get => GetProperty(ref _renderResourceBlob);
			set => SetProperty(ref _renderResourceBlob, value);
		}

		[Ordinal(3)] 
		[RED("renderTextureResource")] 
		public rendRenderTextureResource RenderTextureResource
		{
			get => GetProperty(ref _renderTextureResource);
			set => SetProperty(ref _renderTextureResource, value);
		}

		public CTextureArray(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
