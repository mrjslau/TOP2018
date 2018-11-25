using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Vision;
using Android.Gms.Vision.Texts;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Speech;
using Camera;
using ImageRecognitionMobile;
using ImageRecognitionMobile.Classificators;
using PCLAppConfig;
using ShopLens.Droid.Helpers;
using ShopLens.Droid.Notifications;
using Unity;

// ReSharper disable once CheckNamespace
namespace ShopLens.Droid
{
    public partial class CameraActivity
    {
        private bool IsThereAnAppToTakePictures()
        {
            var intent = new Intent(MediaStore.ActionImageCapture);
            var availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private ErrorDialogCreator GetShoppingCartAddItemDialog(RecognitionResult recognitionResult)
        {
            return new ErrorDialogCreator(this, Resources.GetString(Resource.String.shoppingCart),
                Resources.GetString(Resource.String.shoppingCartQuestion),
                Resources.GetString(Resource.String.positiveMessage),
                Resources.GetString(Resource.String.negativeMessage),
                () => AddToShoppingCart(recognitionResult.BestPrediction), delegate { });
        }

        private Bitmap GetResizedImageForClassification(Uri uri)
        {
            var maxWebClassifierImageSize = int.Parse(ConfigurationManager.AppSettings["webClassifierImgSize"]);
            var image = MediaStore.Images.Media.GetBitmap(ContentResolver, uri);
            image = BitmapHelper.ScaleDown(image, maxWebClassifierImageSize);
            return image;
        }

        private static Task<Dictionary<string, float>> GetClassifyImageTask(Bitmap image)
        {
            var stream = new MemoryStream();
            // 0 because compression quality is not applicable to .png
            image.Compress(Bitmap.CompressFormat.Png, 0, stream);

            var classifier = DependencyInjection.Container.Resolve<IAsyncImageClassificator>();
            var classifyImageTask = Task.Run(
                async () =>
                {
                    var results = await classifier.ClassifyImageAsync(stream.ToArray());
                    stream.Close();
                    return results;
                });
            return classifyImageTask;
        }

        private async Task<string> GetTextFromImageAsync(Bitmap image)
        {
            var task = Task.Run(() =>
            {
                var frame = new Frame.Builder().SetBitmap(image).Build();
                var items = textRecognizer.Detect(frame);
                var ocrResultBuilder = new StringBuilder();
                for (var i = 0; i < items.Size(); i++)
                {
                    var item = (TextBlock) items.ValueAt(i);
                    ocrResultBuilder.Append(item.Value);
                    ocrResultBuilder.Append("/");
                }

                var ocrResult = ocrResultBuilder.ToString();
                return ocrResult;
            });

            return await task;
        }
        
        #region Unimplemented Speech Recognizer Methods

        // When the user starts to speak.
        public void OnBeginningOfSpeech() { }

        // After the user stops speaking.
        public void OnEndOfSpeech() { }

        // When a network or recognition error occurs.
        public void OnError([GeneratedEnum] SpeechRecognizerError error) { }

        // When the app is ready for the user to start speaking.
        public void OnReadyForSpeech(Bundle @params) { }

        // This method is reserved for adding future events.
        public void OnEvent(int eventType, Bundle @params) { }

        // When more sound has been received.
        public void OnBufferReceived(byte[] buffer) { }

        // When the sound level of the voice input stream has changed.
        public void OnRmsChanged(float rmsdB) { }

        // When partial recognition results are available.
        public void OnPartialResults(Bundle partialResults) { }

        #endregion
    }
}