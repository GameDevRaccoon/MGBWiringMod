using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.MGBWiringMod.src
{
    [EditorGroup("MGB|Online Wiring")]
    class OnlineWireTile : WireDevice
    {
        EditorProperty<bool> upConnection = new EditorProperty<bool>(false, (Thing)null, 0f, 1f, 1f, null, false, false);
        EditorProperty<bool> downConnection = new EditorProperty<bool>(false, (Thing)null, 0f, 1f, 1f, null, false, false);
        EditorProperty<bool> leftConnection = new EditorProperty<bool>(false, (Thing)null, 0f, 1f, 1f, null, false, false);
        EditorProperty<bool> rightConnection = new EditorProperty<bool>(false, (Thing)null, 0f, 1f, 1f, null, false, false);
        EditorProperty<int> frameNumber = new EditorProperty<int>(0, (Thing)null, 11, 0, 1, null, false, false);
        EditorProperty<float> xloc = new EditorProperty<float>(0, (Thing)null, -999, 999, 1, null, false, false);
        EditorProperty<float> yloc = new EditorProperty<float>(0, (Thing)null, -999, 999, 1, null, false, false);
        SpriteMap sprite;
        public override int frame
        {
            get
            {
                return base.frame;
            }
            set
            {
                base.frame = value;
                this.UpdateConnectionState();
            }
        }
        public OnlineWireTile(int xpos, int ypos)
            : base(xpos, ypos)
        {
            this.sprite = new SpriteMap(Mod.GetPath<MGBWiringMod>("sprites/wiretileset"), 16, 16, true);
            this.graphic = (Sprite)this.sprite;
            this.center = new Vec2(8f, 8f);
            this.collisionSize = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
            this.depth = (Depth)0.5f;
            this._canFlip = false;
            this._editorName = "Online Wire";
            this.layer = Layer.Foreground;
            this.x = (float)xpos;
            this.y = (float)ypos;
            this._solid = false;
        }


        public override void Initialize()
        {
            this.UpdateConnectionState();
            InitializeNeighbors();
            base.Initialize();
        }
        public override void InitializeNeighbors()
        {
            if (this._neighborsInitialized)
                return;
            this._leftBlock = Level.CheckPoint<Block>(this.left - 2f, this.position.y, (Thing)this, (Layer)null);
            this._rightBlock = Level.CheckPoint<Block>(this.right + 2f, this.position.y, (Thing)this, (Layer)null);
            this._upBlock = Level.CheckPoint<Block>(this.position.x, this.top - 2f, (Thing)this, (Layer)null);
            this._downBlock = Level.CheckPoint<Block>(this.position.x, this.bottom + 2f, (Thing)this, (Layer)null);
            this._neighborsInitialized = true;
            this.UpdateConnectionState();
        }

        public override void UpdateConnectionState()
        {
            this.upTile = Level.CheckPoint<Thing>(this.position.x, this.position.y - 16f, (Thing)this, (Layer)null);
            this.downTile = Level.CheckPoint<Thing>(this.position.x, this.position.y + 16f, (Thing)this, (Layer)null);
            this.leftTile = Level.CheckPoint<Thing>(this.position.x - 16f, this.position.y, (Thing)this, (Layer)null);
            this.rightTile = Level.CheckPoint<Thing>(this.position.x + 16f, this.position.y, (Thing)this, (Layer)null);
            this.connections.Clear();
            myConnectionState.ClearConnections();

            if (this.upTile is WireDevice)
            {
                myConnectionState.upConnection = true;
                connections.Add((WireDevice)upTile);
            }
            if (this.downTile is WireDevice)
            {
                myConnectionState.downConnection = true;
                connections.Add((WireDevice)downTile);
            }
            if (this.leftTile is WireDevice)
            {
                myConnectionState.leftConnection = true;
                connections.Add((WireDevice)leftTile);
            }
            if (this.rightTile is WireDevice)
            {
                myConnectionState.rightConnection = true;
                connections.Add((WireDevice)rightTile);
            }
        }

        public override void EditorUpdate()
        {
            UpdateConnectionState();
            upConnection = myConnectionState.upConnection;
            downConnection = myConnectionState.downConnection;
            leftConnection = myConnectionState.leftConnection;
            rightConnection = myConnectionState.rightConnection;
            this.frame = myConnectionState.GetState();
            frameNumber = this.frame;
            xloc = this.x;
            yloc = this.y;
            base.EditorUpdate();
        }

        public override void Update()
        {
            UpdateConnectionState();
            this.frame = myConnectionState.GetState();
            base.Update();
        }
    }
}
