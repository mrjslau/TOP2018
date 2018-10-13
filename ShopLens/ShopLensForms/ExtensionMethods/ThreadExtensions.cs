using System;
using System.Windows.Forms;

namespace ShopLensApp.ExtensionMethods
{
    /// <summary>
    /// All Thread related extensions are in this class.
    /// </summary>
    static class ThreadExtensions
    {
        /// <summary> 
        /// Invokes void methods which have two parameters of type 'object' and 'EventArgs'
        /// on the GUI thread of a specific form. 
        /// </summary>
        /// <param name="formToBeInvokedOn">A Windows Form on which the method needs to be invoked.</param>
        /// <param name="methodToBeInvoked">The method that will be invoked.</param>
        /// <remarks>
        /// This if statement makes sure the method that must be executed when the specified command is recognized
        /// is called within the GUI thread. For information see https://stackoverflow.com/a/10170699.
        /// </remarks>
        public static void InvokeOnGUIThread_VoidObjEvArgs(this Form formToBeInvokedOn, 
            Action<object, EventArgs> methodToBeInvoked, object sender, EventArgs e)
        {
            if (formToBeInvokedOn.InvokeRequired)
            {
                formToBeInvokedOn.BeginInvoke(new MethodInvoker(() => methodToBeInvoked(sender, e)));
            }
            else
            {
                methodToBeInvoked(sender, e);
            }
        }
    }
}
