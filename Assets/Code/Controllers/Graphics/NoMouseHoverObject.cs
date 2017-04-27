
namespace Graphics_C
{
    /// <summary>
    /// There is no mouse hover
    /// </summary>
    public class NoMouseHoverObject : MouseHoverObject
    {
        public void destroy()
        {
            return;
        }

        public Directions.dir getDirection()
        {
            return Directions.dir.N;
        }

        public Iso getOrigin()
        {
            return new Iso(0, 0, 0);
        }

        public void hide()
        {
            return;
        }

        public void rotate(Directions.dir new_direction)
        {
            return;
        }

        public void translate(Iso target)
        {
            return;
        }

        public void unhide()
        {
            return; ;
        }
    }
}