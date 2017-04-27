
using System;
using System.Collections.Generic;

namespace Lighting_C
{

    /// <summary>
    /// Controls the lighting system
    /// </summary>
    public abstract class LightingControl : Controller
    {
        public static LightingControl main;
        public static ulong light_id;
        public Queue<string> print_queue = new Queue<string>();

        /// <summary>
        /// Default constructor, sets some universal values
        /// </summary>
        protected LightingControl()
        {
            light_id = 0;
            main = this;
        }

        /// <summary>
        /// This is ran after al controller constructors have been ran
        /// </summary>
        public abstract void delayedConstruction();

        protected static Action onLightingProcessed;
        public static void registerOnLightingProcessed(Action funct) { onLightingProcessed += funct; }
        public static void removeOnLightingProcessed(Action funct) { onLightingProcessed -= funct; }

        /// <summary>
        /// Destroy inhereting object
        /// </summary>
        protected abstract void destructor();

        /// <summary>
        /// Run once per frame
        /// </summary>
        public abstract void update();

        /// <summary>
        /// Froce lighting to be computed on main thread
        /// </summary>
        public abstract void runOnMainThread();

        /// <summary>
        /// Destroy this object
        /// </summary>
        public void destroy()
        {
            destructor();
        }
    }
}