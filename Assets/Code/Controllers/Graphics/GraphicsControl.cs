

using Transform = UnityEngine.Transform;
using GameObject = UnityEngine.GameObject;

namespace Graphics_C
{


    /// <summary>
    /// version aplha-1
    /// 
    /// Accessor functions for Grapics.
    /// 
    /// Robin Apollo Buitendijk
    /// Early March 2017
    /// </summary>
    public abstract class GraphicsControl : Controller
    {

        public static GraphicsControl main;
        public MouseHoverObject hover;

        protected Transform tileFolder;
        /// <summary>
        /// Sets common variable and registers callbacks
        /// </summary>
        public GraphicsControl(MouseHoverObject hover)
        {
            main = this;
            this.hover = hover;

            tileFolder = new GameObject() { name = "TileFolder" }.transform;

            Tile.registerOnCreate(onTileCreate);
            Tile.registerOnDestroy(onTileDestroy);

            Directions.currentDirection = Directions.dir.N;
        }

        /// <summary>
        /// To be run after all controller constructors have been run
        /// </summary>
        public abstract void delayedConstruction();

        /// <summary>
        /// Concider removing
        /// </summary>
        protected abstract void onTileCreate(Tile t);
        /// <summary>
        /// Concider removing
        /// </summary>
        protected abstract void onTileDestroy(Tile t);

        /// <summary>
        /// Destroy inhereting object, automatically called in base.destroy();
        /// </summary>
        protected abstract void destructor();

        /// <summary>
        /// Rotate map to direction dir
        /// </summary>
        public abstract void rotate(Directions.dir direction);

        /// <summary>
        /// Destroy the controller so that it can be overwritten in the runner
        /// </summary>
        public void destroy()
        {
            Tile.removeOnCreate(onTileCreate);
            Tile.removeOnDestroy(onTileDestroy);

            hover.destroy();
            destructor();
            GameObject.Destroy(tileFolder.gameObject);
            tileFolder = null; ;
        }
    }
}