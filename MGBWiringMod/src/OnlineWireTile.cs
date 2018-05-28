using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DuckGame.MGBWiringMod.src.Core;

namespace DuckGame.MGBWiringMod.src
{
    struct DeviceConnectionState
    {
        public bool upConnection;
        public bool downConnection;
        public bool rightConnection;
        public bool leftConnection;
        public void ClearConnections()
        {
            upConnection = false;
            downConnection = false;
            rightConnection = false;
            leftConnection = false;
        }

        public int GetState()
        {
            if (!upConnection && !downConnection && !leftConnection && !rightConnection) return 0;
            if (!upConnection && !downConnection && (leftConnection || rightConnection)) return 1;
            if ((upConnection || downConnection) && !leftConnection && !rightConnection) return 2;
            if (upConnection && downConnection && leftConnection && rightConnection) return 3;
            if (upConnection && !downConnection && leftConnection && rightConnection) return 4;
            if (!upConnection && downConnection && leftConnection && rightConnection) return 5;
            if (upConnection && downConnection && leftConnection && !rightConnection) return 6;
            if (upConnection && downConnection && !leftConnection && rightConnection) return 7;
            if (upConnection && !downConnection && !leftConnection && rightConnection) return 8;
            if (!upConnection && downConnection && leftConnection && !rightConnection) return 9;
            if (upConnection && !downConnection && leftConnection && !rightConnection) return 10;
            if (!upConnection && downConnection && !leftConnection && rightConnection) return 11;
            return 0;
        }
    }

    [EditorGroup("MGB|Online Wiring")]
    class OnlineWireTile : Block, IConsumer, IEmitter
    {
        #region editorproperties
        EditorProperty<bool> upConnection = new EditorProperty<bool>(false, (Thing)null, 0f, 1f, 1f, null, false, false);
        EditorProperty<bool> downConnection = new EditorProperty<bool>(false, (Thing)null, 0f, 1f, 1f, null, false, false);
        EditorProperty<bool> leftConnection = new EditorProperty<bool>(false, (Thing)null, 0f, 1f, 1f, null, false, false);
        EditorProperty<bool> rightConnection = new EditorProperty<bool>(false, (Thing)null, 0f, 1f, 1f, null, false, false);
        EditorProperty<int> frameNumber = new EditorProperty<int>(0, (Thing)null, 11, 0, 1, null, false, false);
        EditorProperty<float> xloc = new EditorProperty<float>(0, (Thing)null, -999, 999, 1, null, false, false);
        EditorProperty<float> yloc = new EditorProperty<float>(0, (Thing)null, -999, 999, 1, null, false, false);

        #endregion
        SpriteMap sprite;
        protected DeviceConnectionState myConnectionState;
        public override void InitializeNeighbors()
        {
            if (this._neighborsInitialized)
                return;
            this._leftBlock = Level.CheckPoint<Block>(this.x - 16f, this.y, (Thing)this, (Layer)null);
            this._rightBlock = Level.CheckPoint<Block>(this.x + 16f, this.y, (Thing)this, (Layer)null);
            this._upBlock = Level.CheckPoint<Block>(this.x, this.y - 16f, (Thing)this, (Layer)null);
            this._downBlock = Level.CheckPoint<Block>(this.x, this.y + 16f, (Thing)this, (Layer)null);
            this._neighborsInitialized = true;
            this.UpdateConnectionState();
        }
        public DeviceConnectionState ConnectionState
        {
            get { return myConnectionState;}
        }

        List<OnlineWireTile> connections = new List<OnlineWireTile>();
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
            this.x = xpos;
            this.y = ypos;
            this._solid = false;
        }
        public virtual void UpdateConnectionState()
        {
           this.connections.Clear();
            myConnectionState.ClearConnections();
            int i = 0;
            if (this._upBlock is OnlineWireTile)
            {
                myConnectionState.upConnection = true;
                connections.Add((OnlineWireTile)_upBlock);
                connections[i].UpdateConnectionState();
                _upBlock.frame = connections[i++].ConnectionState.GetState();
            }
            if (this._downBlock is OnlineWireTile)
            {
                myConnectionState.downConnection = true;
                connections.Add((OnlineWireTile)_downBlock);
                connections[i].UpdateConnectionState();
                _downBlock.frame = connections[i++].ConnectionState.GetState();
            }
            if (this._leftBlock is OnlineWireTile)
            {
                myConnectionState.leftConnection = true;
                connections.Add((OnlineWireTile)_leftBlock);
                connections[i].UpdateConnectionState();
                _leftBlock.frame = connections[i++].ConnectionState.GetState();
            }
            if (this._rightBlock is OnlineWireTile)
            {
                myConnectionState.rightConnection = true;
                connections.Add((OnlineWireTile)_rightBlock);
                connections[i].UpdateConnectionState();
                _rightBlock.frame = connections[i++].ConnectionState.GetState();
            }
        } 
        public override void Initialize()
        {
            this.UpdateConnectionState();
            InitializeNeighbors();
            base.Initialize();
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

        public void EmitSignal()
        {
            EmitSignal(new List<IConsumer>());
        }

        public void EmitSignal(IList<IConsumer> travelled)
        {
            travelled.Add(this);
            for (int i = 0; i < connections.Count; i++)
            {
                if (!travelled.Contains(connections[i])) connections[i].ConsumeSignal(travelled);
            }
        }

        public void ConsumeSignal(IList<IConsumer> travelled)
        {
            EmitSignal(travelled);
        }
    }
}
