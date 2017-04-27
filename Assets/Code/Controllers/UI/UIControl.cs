
using System;

namespace UI_C
{
    /// <summary>
    /// Controller that build and handels interaction with the ui 
    /// </summary>
    public abstract class UIControl : Controller
    {
        public static UIControl main;

        /// <summary>
        /// Constructor set some universal parameters
        /// </summary>
        protected UIControl()
        {
            main = this;
        }

        /// <summary>
        /// This functions runs after all controllers constructor has been ran.
        /// </summary>
        public abstract void delayedConstruction();

        protected static Action<string> onMouseClick;
        public static void registerOnMouseClick(Action<string> funct) { onMouseClick += funct; }
        public static void removeOnMouseClick(Action<string> funct) { onMouseClick -= funct; }

        /// <summary>
        /// Check if keys are being absorbed
        /// </summary>
        public abstract bool usesKeys();

        /// <summary>
        /// Destroy inhereting object
        /// </summary>
        protected abstract void destructor();

        /// <summary>
        /// Destroy this object
        /// </summary>
        public void destroy()
        {
            destructor();
        }
    }
}