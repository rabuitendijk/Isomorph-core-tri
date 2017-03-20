
using System.Collections.Generic;
using UnityEngine;

public interface MouseHoverObject  {

    void translate(Iso target);
    void rotate(Directions.dir new_direction);
    void hide();
    void unhide();
    void destroy();
    Iso getOrigin();
    Directions.dir getDirection();
}
