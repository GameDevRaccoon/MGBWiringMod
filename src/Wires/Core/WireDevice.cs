using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    struct DeviceColourState
    {
        public bool r;
        public bool g;
        public bool b;
        public bool y;
        public DeviceColourState(bool setR, bool setG, bool setB, bool setY)
        {
            r = setR;
            g = setG;
            b = setB;
            y = setY;
        }
    }
    class WireDevice : Block
    {
        protected List<WireDevice> connections = new List<WireDevice>();
        protected DeviceConnectionState myConnectionState;
        protected DeviceColourState myColourState;
        protected Thing upTile;
        protected Thing downTile;
        protected Thing leftTile;
        protected Thing rightTile;
        public EditorProperty<bool> red;
        public EditorProperty<bool> green;
        public EditorProperty<bool> blue;
        public EditorProperty<bool> yellow;
        public enum DeviceColour {Red, Green, Blue, Yellow};
        protected DeviceColour myColour;
        public WireDevice(int xpos, int ypos)
            : base(xpos, ypos)
        {
            this.graphic = (Sprite)null;
            this._canFlip = false;
            myColour = DeviceColour.Red;
            this.red = new EditorProperty<bool>(true, (Thing)this, 0.0f, 4f, 1f, null, false, false);
            this.green = new EditorProperty<bool>(false, (Thing)this, 0.0f, 4f, 1f, null, false, false);
            this.blue = new EditorProperty<bool>(false, (Thing)this, 0.0f, 4f, 1f, null, false, false);
            this.yellow = new EditorProperty<bool>(false, (Thing)this, 0.0f, 4f, 1f, null, false, false);
        }

        public WireDevice(int xpos, int ypos, DeviceColour setColour)
            : base(xpos, ypos)
        {
            this.graphic = (Sprite)null;
            this._canFlip = false;
            myColour = setColour;
            this.red = new EditorProperty<bool>(true, (Thing)this, 0.0f, 4f, 1f, null, false, false);
            this.green = new EditorProperty<bool>(false, (Thing)this, 0.0f, 4f, 1f, null, false, false);
            this.blue = new EditorProperty<bool>(false, (Thing)this, 0.0f, 4f, 1f, null, false, false);
            this.yellow = new EditorProperty<bool>(false, (Thing)this, 0.0f, 4f, 1f, null, false, false);
            //TODO: Handle this.
        }

        DeviceColour Colour
        {
            get { return myColour; }
            set { myColour = value; }
        }
        public override void EditorPropertyChanged(object property)
        {
        }

        public virtual bool HasNoCollision()
        {
            return true;
        }

        public virtual void Emit()
        {
            Emit(new List<WireDevice>());
        }

        public virtual void Emit(List<WireDevice> travelled)
        {
            travelled.Add(this);
            for (int i = 0; i < connections.Count; i++)
            {
                if (!travelled.Contains(connections[i])) connections[i].Emit(travelled);
            }
        }

        public virtual void UpdateConnectionState()
        {
            this.upTile = Level.CheckPoint<Thing>(this.x, this.y - 16f, (Thing)this, (Layer)null);
            this.downTile = Level.CheckPoint<Thing>(this.x, this.y + 16f, (Thing)this, (Layer)null);
            this.leftTile = Level.CheckPoint<Thing>(this.x - 16f, this.y, (Thing)this, (Layer)null);
            this.rightTile = Level.CheckPoint<Thing>(this.x + 16f, this.y, (Thing)this, (Layer)null);
            this.connections.Clear();

            if (this.upTile is WireDevice && (((WireDevice)this.upTile).Colour == this.Colour))
            {
                myConnectionState.upConnection = true;
                connections.Add((WireDevice)upTile);
            }
            if (this.downTile is WireDevice && (((WireDevice)this.upTile).Colour == this.Colour))
            {
                myConnectionState.downConnection = true;
                connections.Add((WireDevice)downTile);
            }
            if (this.leftTile is WireDevice && (((WireDevice)this.upTile).Colour == this.Colour))
            {
                myConnectionState.leftConnection = true;
                connections.Add((WireDevice)leftTile);
            }
            if (this.rightTile is WireDevice && (((WireDevice)this.upTile).Colour == this.Colour))
            {
                myConnectionState.rightConnection = true;
                connections.Add((WireDevice)rightTile);
            }
        }
    }
}
