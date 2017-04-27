
namespace Save_C
{

    /// <summary>
    /// Controller that handels the matter of saving
    /// </summary>
    public abstract class SaveControl : Controller
    {
        public static SaveControl main;
        public static ulong isoObject_id;

        /// <summary>
        /// Common constructor, sets some universal parameters
        /// </summary>
        protected SaveControl()
        {
            isoObject_id = 0;
            main = this;
        }

        /// <summary>
        /// This functions runs afer alll controller constructors have been ran
        /// </summary>
        public abstract void delayedConstruction();

        /// <summary>
        /// Destroy inhereting obejct
        /// </summary>
        protected abstract void destructor();

        /// <summary>
        /// Save current level
        /// </summary>
        protected abstract void save(string filename);

        /// <summary>
        /// Destroy this object
        /// </summary>
        public void destroy()
        {
            destructor();
        }
    }
}