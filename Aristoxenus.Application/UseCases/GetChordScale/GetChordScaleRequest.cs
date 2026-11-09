

using Aristoxenus.Domain.Core.Enums;
using FluentResults;
using MediatR;

namespace Aristoxenus.Application.UseCases.GetChordScale;

public record GetChordScaleRequest(
    string TonicNoteName, 
    ScaleName ScaleName, 
    int ModeNumber, 
    ChordNumberOfNotes NumberOfNotes,
    ChordStructure Structure
) : IRequest<Result<GetChordScaleResponse>>;

