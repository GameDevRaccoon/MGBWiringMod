using System.Collections.Generic;

namespace DuckGame.MGBWiringMod.src.Core
{
    public interface IConsumer
    {
        void ConsumeSignal(IList<IConsumer> travelled);
        void UpdateConnectionState();
    }
}
