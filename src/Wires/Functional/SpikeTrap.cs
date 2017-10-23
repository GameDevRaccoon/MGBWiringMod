using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.MGBWiringMod.src
{
    [EditorGroup("MGB|Online Wiring|Functional")]
    class SpikeTrap : WireDevice, IWireDevice
    {
        bool hasPulsed = false;
        float pulseDelay = 5.0f;
        float currentDelay = 0.0f;
        OnlineSpikes mySpikes;

        public SpikeTrap(int xpos, int ypos):
            base(xpos,ypos)
        {
            //TODO: Edit this to add orientation to the spikes

            this.graphic = new SpriteMap(Mod.GetPath<MGBWiringMod>("sprites/wirebutton"), 16, 19, true);
            this._editorName = "Online Spike Trap";
            this.center = new Vec2(8f, 11f);
            this.collisionSize = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
            this.depth = (Depth)0.5f;
            this.x = (float)xpos;
            this.y = (float)ypos;
           // mySpikes = new OnlineSpikes(this.x, this.y - 9f, this, 0);
        }

        public override void Initialize()
        {
            if (!(Level.current is Editor))
            {
          
                    this.mySpikes = new OnlineSpikes((float)this.x, (float)this.y - 9f, this, 0);
            }
            base.Initialize();
        }

        public override void Terminate()
        {
            if (hasPulsed)
                Level.Remove(mySpikes);
            base.Terminate();
        }

        public void Pulse()
        {
            if (this.frame != 0)
                return;
           // SFX.Play("explode", 1f, 0.0f, 0.0f, false); // TODO: change sound to something more ~shing~ like.
            this.frame = 1;
            Level.Add((Thing)mySpikes);
            hasPulsed = true;
        }

        public override void Emit(List<WireDevice> travelled)
        {
            if (hasPulsed)
                return;
            Pulse();
            base.Emit(travelled);
        }

        public override void Update()
        {
            UpdateConnectionState();
            if (hasPulsed)
            {
                if (pulseDelay <= currentDelay)
                {
                    hasPulsed = false;
                    currentDelay = 0.0f;
                    Level.Remove(mySpikes);
                    this.frame = 0;
                }
                else
                {
                    currentDelay += 0.0166666f;
                }
            }
            base.Update();
        }


    }
}
