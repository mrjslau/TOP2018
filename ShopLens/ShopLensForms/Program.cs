using ShopLensForms.Controllers;
using System;
using System.Windows.Forms;

namespace ShopLensForms
{
    static class Program
    {
        static void Main()
        {
            MainController mainController = new MainController();
            mainController.StartApp();
        }
    }
}
