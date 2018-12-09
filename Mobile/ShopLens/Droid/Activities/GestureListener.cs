using System;
using Android.Views;

namespace ShopLens.Droid.Activities
{
    public class GestureListener: Java.Lang.Object, GestureDetector.IOnGestureListener
    {
        public event Action LeftEvent;
        public event Action RightEvent;
        static int SWIPE_MAX_OFF_PATH = 250;
        static int SWIPE_MIN_DISTANCE = 120;
        static int SWIPE_THRESHOLD_VELOCITY = 200;

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            if (Math.Abs(e1.GetY() - e2.GetY()) > SWIPE_MAX_OFF_PATH)
                return false;
            // right to left swipe
            if (e1.GetX() - e2.GetX() > SWIPE_MIN_DISTANCE && Math.Abs(velocityX) > SWIPE_THRESHOLD_VELOCITY && LeftEvent != null)
                LeftEvent();
            else if (e2.GetX() - e1.GetX() > SWIPE_MIN_DISTANCE && Math.Abs(velocityX) > SWIPE_THRESHOLD_VELOCITY && RightEvent != null)
                RightEvent();

            return false;
        }

        public bool OnDown(MotionEvent e) { return true; }
        public void OnLongPress(MotionEvent e) {}
        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY) { return true; }
        public void OnShowPress(MotionEvent e) {}
        public bool OnSingleTapUp(MotionEvent e) { return true; }   
    }
}