﻿using System;
using System.Windows.Forms;

namespace VoiceRecognition
{
    static class LaunchTest
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new VoiceRecognitionTest());
        }
    }
}
