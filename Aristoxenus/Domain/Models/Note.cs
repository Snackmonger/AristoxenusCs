
namespace Aristoxenus.Domain.Models
{

    public record Dynamics { }


    public class Note
    {
        public readonly Pitch Pitch;
        public readonly double Duration;
        public readonly Dynamics Dynamics;


    }
}
