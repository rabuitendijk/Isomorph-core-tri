
using System.Collections.Generic;

namespace AssetHandeling_AtlasBuilder
{
    /// <summary>
    /// A reprentation of image data of a single source image
    /// </summary>
    public class SplicingSource
    {

        public List<ProcessingImage[,,]> mips = new List<ProcessingImage[,,]>();
        public int width { get; protected set; }
        public int length { get; protected set; }
        public int height { get; protected set; }

        /// <summary>
        /// Common constructor
        /// </summary>
        public SplicingSource(int mips, int width, int length, int height)
        {
            this.width = width;
            this.length = length;
            this.height = height;


        }
    }
}