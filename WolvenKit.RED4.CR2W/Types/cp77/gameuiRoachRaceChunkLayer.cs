using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class gameuiRoachRaceChunkLayer : CVariable
	{
		private CArray<gameuiRoachRaceChunk> _chunks;

		[Ordinal(0)] 
		[RED("chunks")] 
		public CArray<gameuiRoachRaceChunk> Chunks
		{
			get => GetProperty(ref _chunks);
			set => SetProperty(ref _chunks, value);
		}

		public gameuiRoachRaceChunkLayer(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
