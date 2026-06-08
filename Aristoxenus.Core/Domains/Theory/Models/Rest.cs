using Aristoxenus.Core.Domains.Theory.Interfaces;

namespace Aristoxenus.Core.Domains.Theory.Models;

public class Rest : IHasDuration
{
    public Duration Duration { get; init; }

    public Rest(Duration duration)
    {
        Duration = duration;
    }

}


