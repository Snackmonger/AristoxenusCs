using System;
using System.Collections.Generic;
using System.Text;

namespace Aristoxenus.Application.UseCases.GetChordScale;

public record GetChordScaleResponse(List<string> ScaleNoteNames, List<string> ScaleIntervalNames, List<ChordDto> ChordScale);