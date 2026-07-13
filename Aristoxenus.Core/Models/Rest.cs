using Aristoxenus.Core.Interfaces;

namespace Aristoxenus.Core.Models;

public class Rest : IHasDuration
{
    public Duration Duration { get; init; }

    public Rest(Duration duration)
    {
        Duration = duration;
    }

}


