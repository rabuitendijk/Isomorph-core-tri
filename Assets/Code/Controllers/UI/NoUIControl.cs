﻿

namespace UI_C
{

    /// <summary>
    /// Controller for disabled UI
    /// </summary>
    public class NoUIControl : UIControl
    {
        public NoUIControl() : base()
        {
            return;
        }

        public override void delayedConstruction()
        {
            return;
        }

        public override bool usesKeys()
        {
            return false;
        }

        protected override void destructor()
        {
            return;
        }
    }
}