using WolvenKit.RED4.CR2W.Reflection;
using FastMember;
using static WolvenKit.RED4.CR2W.Types.Enums;

namespace WolvenKit.RED4.CR2W.Types
{
	[REDMeta]
	public class audioUiSpecificControlSettingsMap : audioAudioMetadata
	{
		private CArray<audioUiSpecificControlSettingsMapItem> _specificControlSettingsMatrix;

		[Ordinal(1)] 
		[RED("specificControlSettingsMatrix")] 
		public CArray<audioUiSpecificControlSettingsMapItem> SpecificControlSettingsMatrix
		{
			get => GetProperty(ref _specificControlSettingsMatrix);
			set => SetProperty(ref _specificControlSettingsMatrix, value);
		}

		public audioUiSpecificControlSettingsMap(IRed4EngineFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }
	}
}
