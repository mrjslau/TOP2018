using Android.Hardware.Camera2;

namespace ShopLens.Droid.Listeners
{
    public class CaptureStillPictureSessionCallback : CameraCaptureSession.CaptureCallback
    {
        private readonly Camera2Fragment owner;

        public CaptureStillPictureSessionCallback(Camera2Fragment owner)
        {
            this.owner = owner ?? throw new System.ArgumentNullException("owner");
        }

        public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
        {
            owner.ShowToast("Saved: " + owner.mFile);
            owner.UnlockFocus();
        }
    }
}