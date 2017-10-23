using DuckGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.MGBWiringMod.src
{
    [EditorGroup("MGB|Online Wiring|Emitters")]
    class OnlineWireButton : WireDevice, IWireDevice
    {
        EditorProperty<bool> upConnection = new EditorProperty<bool>(false, (Thing)null, 0f, 1f, 1f, null, false, false);
        EditorProperty<bool> downConnection = new EditorProperty<bool>(false, (Thing)null, 0f, 1f, 1f, null, false, false);
        EditorProperty<bool> leftConnection = new EditorProperty<bool>(false, (Thing)null, 0f, 1f, 1f, null, false, false);
        EditorProperty<bool> rightConnection = new EditorProperty<bool>(false, (Thing)null, 0f, 1f, 1f, null, false, false);
        public EditorProperty<int> orientation = new EditorProperty<int>(0, (Thing)null, 0.0f, 3f, 1f, null, false, false);
        private OnlineWireButtonTop _top;
        private bool hasPulsed = false;
        float pulseDelay = 5.0f;
        float currentDelay = 0.0f;

        public OnlineWireButton(int xpos, int ypos)
            : base(xpos, ypos)
        {
            this.graphic = new SpriteMap(Mod.GetPath<MGBWiringMod>("sprites/wirebutton"), 16, 19, true);
            this._editorName = "Online Wire Button";
            this.center = new Vec2(8f, 11f);
            this.collisionSize = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
            this.depth = (Depth)0.5f;
            this.x = (float)xpos;
            this.y = (float)ypos;
            this.red = new EditorProperty<bool>(true, (Thing)this, 0.0f, 4f, 1f, null, false, false);
            this.green = new EditorProperty<bool>(false, (Thing)this, 0.0f, 4f, 1f, null, false, false);
            this.blue = new EditorProperty<bool>(false, (Thing)this, 0.0f, 4f, 1f, null, false, false);
            this.yellow = new EditorProperty<bool>(false, (Thing)this, 0.0f, 4f, 1f, null, false, false);
            this.myColourState = new DeviceColourState(true, false, false, false);
        }

        public override void Initialize()
        {
            this.angleDegrees = (float)this.orientation.value * 90f;
            if (!(Level.current is Editor))
            {
                if (this.orientation.value == 0)
                    this._top = new OnlineWireButtonTop((float)this.x, (float)this.y - 9f, this, this.orientation.value);
                else if (this.orientation.value == 1)
                    this._top = new OnlineWireButtonTop((float)this.x + 9f, (float)this.y, this, this.orientation.value);
                else if (this.orientation.value == 2)
                    this._top = new OnlineWireButtonTop((float)this.x, (float)this.y + 9f, this, this.orientation.value);
                else if (this.orientation.value == 3)
                    this._top = new OnlineWireButtonTop((float)this.x - 9f, (float)this.y, this, this.orientation.value);
                Level.Add((Thing)this._top);
            }
            base.Initialize();
        }

        public override void EditorPropertyChanged(object property)
        {
            //TODO: Change to contextRadioButton

            DeviceColourState currentState = new DeviceColourState(this.red.value, this.green.value, this.blue.value, this.yellow.value);
            DeviceColourState newState;
            newState.r = (currentState.r && !this.myColourState.r) ? true : false;
            newState.g = (currentState.g && !this.myColourState.g) ? true : false;
            newState.b = (currentState.b && !this.myColourState.b) ? true : false;
            newState.y = (currentState.y && !this.myColourState.y) ? true : false;
            this.myColourState = newState;
            if (this.red.value != newState.r && (EditorProperty<bool>)property != this.red) this.red.value = newState.r;
            if (this.blue.value != newState.b && (EditorProperty<bool>)property != this.blue) this.blue.value = newState.b;
            if (this.green.value != newState.g && (EditorProperty<bool>)property != this.green) this.green.value = newState.g;
            if (this.yellow.value != newState.y && (EditorProperty<bool>)property != this.yellow) this.yellow.value = newState.y;
        }

        public override void Terminate()
        {
            Level.Remove((Thing)this._top);
            base.Terminate();
        }

        public void Pulse()
        {
            if (this.frame != 0)
                return;
            SFX.Play("click", 1f, 0.0f, 0.0f, false);
            this.frame = 1;
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
            if (this.frame == 1 && Level.CheckRect<PhysicsObject>(this._top.topLeft, this._top.bottomRight, (Thing)null) == null)
            {
                SFX.Play("click", 1f, 0.0f, 0.0f, false);
                this.frame = 0;
            }
            if (hasPulsed)
            {
                if (currentDelay == pulseDelay)
                {
                    hasPulsed = !hasPulsed;
                    currentDelay = 0.0f;
                }
                else
                {
                    currentDelay += 1.0f;
                }
            }
            base.Update();
        }
    }
}
