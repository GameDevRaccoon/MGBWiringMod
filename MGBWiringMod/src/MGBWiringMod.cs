using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DuckGame.MGBWiringMod.src
{
    public class MGBWiringMod : Mod
    {
        // The mod's priority; this property controls the load order of the mod.
        public override Priority priority
        {
            get { return base.priority; }
        }

        // This function is run before all mods are finished loading.
        protected override void OnPreInitialize()
        {

            base.OnPreInitialize();
        }

        // This function is run after all mods are loaded.
        protected override void OnPostInitialize()
        {
            base.OnPostInitialize();
        }
    }
}
