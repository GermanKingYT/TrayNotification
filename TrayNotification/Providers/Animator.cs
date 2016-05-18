using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TrayNotification.Providers
{
    public sealed class Animator
    {
        private const int AW_HIDE = 0x10000;
        private const int AW_ACTIVATE = 0x20000;

        /// <summary>
        /// Animation style used to show and hide the target form.
        /// </summary>
        public Style Animation { get; set; }

        /// <summary>
        /// Direction in which the animation is performed.
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// Number of milliseconds over the animation.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Form to be animated.
        /// </summary>
        public Form Target { get; }

        [DllImport("user32")]
        private extern static bool AnimateWindow(IntPtr hWnd, int dwTime, int dwFlags);

        /// <summary>
        /// Creates a new <b>FormAnimator</b> object for the specified form.
        /// </summary>
        /// <param name="form">
        /// The form to be animated.
        /// </param>
        public Animator(Form form)
        {
            Target = form;
            var flags = (int)Animation | (int)Direction;

            Target.Load += (o, e) =>
                AnimateWindow(Target.Handle, Duration, AW_ACTIVATE | flags); 

            Target.Closing += (o, e) =>
                AnimateWindow(Target.Handle, Duration, AW_HIDE | flags);

            Target.VisibleChanged += (o, e) =>
            {

                if (Target.Visible)
                    flags |= AW_ACTIVATE;
                else
                    flags |= AW_HIDE;

                AnimateWindow(Target.Handle, Duration, flags);
            };
        }
    }
}
