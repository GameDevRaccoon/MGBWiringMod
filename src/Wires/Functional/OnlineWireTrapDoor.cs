using DuckGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.MGBWiringMod.src
{
    [EditorGroup("MGB|Online Wiring|Functional")]
    class OnlineWireTrapDoor : WireDevice, IWireDevice
    {
        private bool open = false;
        List<Duck> aboveMe = new List<Duck>();
        public OnlineWireTrapDoor(int xpos, int ypos)
            : base(xpos, ypos)
        {
            this.graphic = new SpriteMap(Mod.GetPath<MGBWiringMod>("sprites/wiretrap"), 16, 16, true);
            this.center = new Vec2(8f, 8f);
            this.collisionSize = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
            this.depth = (Depth)0.5f;
            this._canFlip = false;
            this._editorName = "Online Wire Trapdoor";
            this.layer = Layer.Foreground;
            this.x = (float)xpos;
            this.y = (float)ypos;
            this.physicsMaterial = PhysicsMaterial.Default;
        }

        public void Pulse()
        {
            open = !open;
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
            if (this.open)
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
