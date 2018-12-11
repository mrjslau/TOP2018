using Android.Hardware.Camera2;
using Java.Lang;

namespace ShopLens.Droid.Listeners
{
    public class CaptureListener : CameraCaptureSession.CaptureCallback
    {
        private readonly Camera2Fragment owner;

        public CaptureListener(Camera2Fragment owner)
        {
            this.owner = owner ?? throw new System.ArgumentNullException("owner");
        }

        public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
        {
            Process(result);
        }

        public override void OnCaptureProgressed(CameraCaptureSession session, CaptureRequest request, CaptureResult partialResult)
        {
            Process(partialResult);
        }

        private void Process(CaptureResult result)
        {
            switch (owner.mState)
            {
                case Camera2Fragment.STATE_WAITING_LOCK:
                    {
                        Integer afState = (Integer)result.Get(CaptureResult.ControlAfState);
                        if (afState == null)
                        {
                            owner.CaptureStillPicture();
                        }

                        else if ((((int)ControlAFState.FocusedLocked) == afState.IntValue()) ||
                                   (((int)ControlAFState.NotFocusedLocked) == afState.IntValue()))
                        {
                            // ControlAeState can be null on some devices
                            Integer aeState = (Integer)result.Get(CaptureResult.ControlAeState);
                            if (aeState == null ||
                                    aeState.IntValue() == ((int)ControlAEState.Converged))
                            {
                                owner.mState = Camera2Fragment.STATE_PICTURE_TAKEN;
                                owner.CaptureStillPicture();
                            }
                            else
                            {
                                owner.RunPrecaptureSequence();
                            }
                        }
                        break;
                    }
                case Camera2Fragment.STATE_WAITING_PRECAPTURE:
                    {
                        // ControlAeState can be null on some devices
                        Integer aeState = (Integer)result.Get(CaptureResult.ControlAeState);
                        if (aeState == null ||
                                aeState.IntValue() == ((int)ControlAEState.Precapture) ||
                                aeState.IntValue() == ((int)ControlAEState.FlashRequired))
                        {
                            owner.mState = Camera2Fragment.STATE_WAITING_NON_PRECAPTURE;
                        }
                        break;
                    }
                case Camera2Fragment.STATE_WAITING_NON_PRECAPTURE:
                    {
                        // ControlAeState can be null on some devices
                        Integer aeState = (Integer)result.Get(CaptureResult.ControlAeState);
                        if (aeState == null || aeState.IntValue() != ((int)ControlAEState.Precapture))
                        {
                            owner.mState = Camera2Fragment.STATE_PICTURE_TAKEN;
                            owner.CaptureStillPicture();
                        }
                        break;
                    }
            }
        }
    }
}