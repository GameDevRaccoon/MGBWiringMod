using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.MGBWiringMod.src
{
    class OnlineSpikes : MaterialThing
    {
        private SpikeTrap _trap;
        private int _orientation;
        public OnlineSpikes(float xpos, float ypos, SpikeTrap b, int orientation) :
            base(xpos, ypos)
        {
            this._trap = b;
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
            if (with is TV)
                return;
            if (this._orientation == 0 && (double)with.vSpeed > -0.100000001490116)
                with.Destroy((DestroyType)new DTImpale((Thing)this));
            bool flag = true;
            if (with is PhysicsObject)
            {
                PhysicsObject physicsObject = with as PhysicsObject;
                Vec2 bottomRight = with.bottomRight;
                Vec2 bottomLeft = with.bottomLeft;
                Vec2 vec2_1 = new Vec2(with.x, with.bottom);
                Vec2 p2 = vec2_1;
                Vec2 vec2_2 = physicsObject.lastPosition - physicsObject.position;
                Vec2 p1_1 = bottomRight + vec2_2;
                Vec2 p1_2 = bottomLeft + vec2_2;
                Vec2 p1_3 = vec2_1 + vec2_2;
                flag = false;
                if (Collision.LineIntersect(p1_3, p2, this.topLeft, this.topRight) || Collision.LineIntersect(p1_2, with.bottomLeft, this.topLeft, this.topRight) || Collision.LineIntersect(p1_1, with.bottomRight, this.topLeft, this.topRight))
                    flag = true;
            }
            if (flag)
                with.Destroy((DestroyType)new DTImpale((Thing)this));
            else if (this._orientation == 1 && (double)with.hSpeed < 0.100000001490116)
                with.Destroy((DestroyType)new DTImpale((Thing)this));
            else if (this._orientation == 2 && (double)with.vSpeed < 0.100000001490116)
                with.Destroy((DestroyType)new DTImpale((Thing)this));
            else if (this._orientation == 3 && (double)with.hSpeed > -0.100000001490116)
                with.Destroy((DestroyType)new DTImpale((Thing)this));
            base.OnSoftImpact(with, from);
        }
    }
}
