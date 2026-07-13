using System;
using System.Collections.Generic;
using System.Text;

namespace Aristoxenus.Application.UseCases.GetChordScale
{
    public class ChordDto
    {
        public string ChordSymbol { get; set; }
        public List<string> NoteNames {  get; set; }
        public List<string> IntervalNames { get; set; }
    }
}
