

using FluentResults;
using MediatR;

namespace Aristoxenus.Application.UseCases.GetChordScale;

public record GetChordScaleRequest(
    string NoteName, 
    string ScaleName, 
    int ModeNumber, 
    int NumberOfChordTones
) : IRequest<Result<GetChordScaleResponse>>;

