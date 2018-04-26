using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.MGBWiringMod.src
{
    [EditorGroup("MGB|Online Wiring|Functional")]
    class FireVent : WireDevice, IWireDevice
    {
        private bool operating = false;
        public FireVent(int xpos, int ypos) 
            : base(xpos, ypos)
        {

        }

        public void Pulse()
        {
            operating = true;
            SFX.Play("click", 1f, 0.0f, 0.0f, false);
        }

        public override void Emit(List<WireDevice> travelled)
        {
            Pulse();
            base.Emit(travelled);
        }

        public override void Update()
        {
            UpdateConnectionState();
            if (operating)
            {
                this._solid = false;
                foreach (PhysicsObject physicsObject in Level.CheckRectAll<PhysicsObject>(this.topLeft + new Vec2(0.0f, -8f), this.bottomRight))
                    physicsObject.sleeping = false;
            }
            else
            {
                this._solid = true;
            }
        }
    }
}
