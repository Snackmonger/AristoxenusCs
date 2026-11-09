

using Aristoxenus.Domain.Core.Models;
using FluentResults;
using MediatR;

namespace Aristoxenus.Application.UseCases.GetChordScale;

public record GetChordScaleRequest(
    string TonicNoteName, 
    string ScaleName, 
    int ModeNumber, 
    ChordNumberOfNotes NumberOfNotes,
    ChordStructure Structure
) : IRequest<Result<GetChordScaleResponse>>;

