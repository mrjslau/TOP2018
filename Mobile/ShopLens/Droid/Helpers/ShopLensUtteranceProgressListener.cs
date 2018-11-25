using Android.Speech.Tts;
using System;

namespace ShopLens.Droid.Helpers
{
    class ShopLensUtteranceProgressListener : UtteranceProgressListener
    {
        public event EventHandler<UtteranceIdArgs> OnEndOfSpeaking;

        public ShopLensUtteranceProgressListener(Action<object, UtteranceIdArgs> reactToEndOfSpeaking)
        {
            OnEndOfSpeaking += (obj, eargs) => reactToEndOfSpeaking(obj, eargs);
        }

        public override void OnDone(string utteranceId)
        {
            OnEndOfSpeaking?.Invoke(this, new UtteranceIdArgs(utteranceId));
        }

        #region Unimplemented UtteranceProgressListener methods

        public override void OnError(string utteranceId) { }

        public override void OnStart(string utteranceId) { }

        #endregion
    }

    public class UtteranceIdArgs : EventArgs
    {
        public string UtteranceId { get; set; }

        public UtteranceIdArgs(string utteranceId)
        {
            UtteranceId = utteranceId;
        }
    }
}