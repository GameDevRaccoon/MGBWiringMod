using DuckGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DuckGame.MGBWiringMod.src.Core;
namespace DuckGame.MGBWiringMod.src
{
    class OnlineWireButtonTop : MaterialThing
    {
        private OnlineWireButton _button;
        private int _orientation;

        public OnlineWireButtonTop(float xpos, float ypos, OnlineWireButton b, int orientation)
            : base(xpos, ypos)
        {
            this._button = b;
            this._orientation = orientation;
            if (orientation == 0)
            {
                this.collisionSize = new Vec2(12f, 4f);
                this.collisionOffset = new Vec2(-6f, -2f);
            }
            else if (orientation == 1)
            {
                this.collisionSize = new Vec2(4f, 12f);
                this.collisionOffset = new Vec2(-2f, -6f);
            }
            else if (orientation == 2)
            {
                this.collisionSize = new Vec2(12f, 4f);
                this.collisionOffset = new Vec2(-6f, -2f);
            }
            else
            {
                if (orientation != 3)
                    return;
                this.collisionSize = new Vec2(4f, 12f);
                this.collisionOffset = new Vec2(-2f, -6f);
            }
        }

        public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
        {
            if (this._orientation == 0 && (double)with.vSpeed > -0.100000001490116)
                this._button.EmitSignal();
            else if (this._orientation == 1 && (double)with.hSpeed < 0.100000001490116)
                this._button.EmitSignal();
            else if (this._orientation == 2 && (double)with.vSpeed < 0.100000001490116)
                this._button.EmitSignal();
            else if (this._orientation == 3 && (double)with.hSpeed > -0.100000001490116)
                this._button.EmitSignal();
            base.OnSoftImpact(with, from);
        }
    }
}
