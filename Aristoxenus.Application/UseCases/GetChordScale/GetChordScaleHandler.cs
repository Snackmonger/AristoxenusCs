using FluentResults;

using MediatR;

using Aristoxenus.Domain.Core.Data;
using Aristoxenus.Domain.Core.Models;


namespace Aristoxenus.Application.UseCases.GetChordScale;

public class GetChordScaleHandler : IRequestHandler<GetChordScaleRequest, Result<GetChordScaleResponse>>
{
    public Task<Result<GetChordScaleResponse>> Handle(GetChordScaleRequest request, CancellationToken cancellationToken)
    {
        if (RawScalePatterns.AllScales.TryGetValue(request.ScaleName, out int[]? value))
        {
            var intervals = IntervalStructure.CreateAsScale(value);
            
        }

        throw new NotImplementedException();

    }
}
