using System.Collections.Generic;
namespace DuckGame.MGBWiringMod.src.Core
{
    public interface IEmitter
    {
         void EmitSignal();
         void EmitSignal(IList<IConsumer> travelled);
    }
}