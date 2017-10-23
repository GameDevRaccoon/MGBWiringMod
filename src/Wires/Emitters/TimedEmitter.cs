using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.MGBWiringMod.src
{
    [EditorGroup("MGB|Online Wiring|Emitters")]
    class TimedEmitter : WireDevice
    {
        EditorProperty<float> timerSeconds = new EditorProperty<float>(1, (Thing)null, 0, 20, 1f, null, false, false);
        float currentTimeElapsed = 0;
        float timerSetting = 0;
        public TimedEmitter(int xpos, int ypos):
            base(xpos,ypos)
        {
            this.graphic = new SpriteMap(Mod.GetPath<MGBWiringMod>("sprites/emitter"), 16, 16, true);
            this.center = new Vec2(8f, 8f);
            this.collisionSize = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
            this.depth = (Depth)0.5f;
            this._canFlip = false;
            this._editorName = "Online Timed Emitter";
            this.layer = Layer.Foreground;
            this.x = (float)xpos;
            this.y = (float)ypos;
        }

        public override void EditorUpdate()
        {
            UpdateConnectionState();
            timerSetting = timerSeconds;
            base.EditorUpdate();
        }

        public override void Update()
        {
            UpdateConnectionState();
            timerSetting = timerSeconds;
            if (currentTimeElapsed >= timerSetting)
            {
                this.Emit();
                currentTimeElapsed = 0;
            }
            else
            {
                currentTimeElapsed += 0.0166666f;
            }
            base.Update();
        }
    }
}
