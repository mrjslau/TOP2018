using System;

namespace VoiceRecognitionWithTextVoicer.VoiceRecognizers
{
    /// <summary>
    /// Interface that defines the methods needed for a voice recognizer.
    /// </summary>
    public interface IVoiceRecognizer
    {
        /// <summary>
        /// Starts the voice recognition process.
        /// </summary>
        void StartVoiceRecognition();

        /// <summary>
        /// Stops the voice recognition process.
        /// </summary>
        void StopVoiceRecognition();

        /// <summary>
        /// Adds a command to be recognized by the voice recognizer.
        /// </summary>
        /// <param name="command">The string of a command to be recognized.</param>
        /// <param name="CommandRecognized">The method to be called when the 
        /// passed command is recognized.</param>
        /// <remarks>
        /// The passed method must have 'object' and 'EventArgs' type parameters.
        /// </remarks>
        void AddCommand(string command, Action<object, EventArgs> CommandRecognized);
    }
}
