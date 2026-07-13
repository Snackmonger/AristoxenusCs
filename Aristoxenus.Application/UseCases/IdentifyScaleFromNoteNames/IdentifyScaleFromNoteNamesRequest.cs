using FluentResults;
using MediatR;

namespace Aristoxenus.Application.UseCases.IdentifyScaleFromNoteNames;

public record IdentifyScaleFromNoteNamesRequest(IEnumerable<string> NoteNameSymbols) : IRequest<Result<IdentifyScaleFromNoteNamesResponse>>;
