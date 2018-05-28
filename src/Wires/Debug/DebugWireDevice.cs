using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.MGBWiringMod.src
{
    [EditorGroup("MGB|Online Wiring|Debug")]
    class DebugWireDevice : WireDevice, IWireDevice
    {
        public EditorProperty<int> orientation = new EditorProperty<int>(0, (Thing)null, 0.0f, 3f, 1f, (string)null, false, false);

        public DebugWireDevice(int xpos, int ypos, string tileset)
            : base(xpos, ypos)
        {
            this.graphic = new SpriteMap(Mod.GetPath<MGBWiringMod>("sprites/wirebutton"), 16, 16, true);
            this.center = new Vec2(8f, 8f);
            this._editorName = "Debug Wire";
        }

        public void Pulse()
        {
            if (this.frame != 0)
                return;
            SFX.Play("explode", 1f, 0.0f, 0.0f, false);
            this.frame = 1;
        }

        public override void Emit(List<WireDevice> travelled)
        {
            Pulse();
            base.Emit(travelled);
        }

        public override void Update()
        {
            if (this.frame == 1)
            {
                this.frame = 0;
            }
            base.Update();
        }
    }
}
