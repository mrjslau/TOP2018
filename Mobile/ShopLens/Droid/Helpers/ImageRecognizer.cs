using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Gms.Vision;
using Android.Gms.Vision.Texts;
using Android.Graphics;
using Android.Speech.Tts;
using Camera;
using ImageRecognitionMobile;
using ImageRecognitionMobile.Classificators;
using ShopLens.Droid.Helpers;
using ShopLens.Droid.Notifications;
using ShopLens.Droid.Source;
using ShopLens.Extensions;
using Unity;
using PCLAppConfig;
using Android.App;

namespace ShopLens.Droid
{
    class ImageRecognizer
    {
        TextToSpeech tts;
        bool isTtsInitialized = false;
        ShopLensTextToSpeech sltts;
        TextRecognizer textRecognizer;
        Context context;
        Activity activity;
        Bitmap bitmap;
        Camera2Fragment owner;
        private readonly string prefsName = ConfigurationManager.AppSettings["ShopCartPrefs"];

        public ImageRecognizer (Context context, Activity activity)
        {
            this.activity = activity;
            this.context = context;
            textRecognizer = new TextRecognizer.Builder(context).Build();
            sltts = new ShopLensTextToSpeech(context, OnTextToSpeechInit, OnTextToSpeechEndOfSpeech);
            tts = new TextToSpeech(context, sltts);
        }

        private void OnTextToSpeechInit(object sender, EventArgs e)
        {
            isTtsInitialized = true;
        }

        private void OnTextToSpeechEndOfSpeech(object sender, UtteranceIdArgs id)
        {

        }

        public void RecognizeImage(Bitmap bitmap, Activity activity, Camera2Fragment owner)
        {
            this.bitmap = bitmap;
            this.owner = owner;
            activity.RunOnUiThread(Recognise);
        }

        private void Recognise()
        {
            int maxWebClassifierImageSize = int.Parse(ConfigurationManager.AppSettings["webClassifierImgSize"]);
            //progressBar.Visibility = ViewStates.Visible;

            var image = GetResizedImageForClassification(bitmap);
            var classifyImageTask = GetClassifyImageTask(image);
            if (!textRecognizer.IsOperational)
            {
                System.Diagnostics.Debug.WriteLine("[Warning]: Text recognizer not operational.");
                var Predictions = classifyImageTask.Result; };
            }

        private Bitmap GetResizedImageForClassification(Bitmap bitmap)
        {
            var maxWebClassifierImageSize = int.Parse(ConfigurationManager.AppSettings["webClassifierImgSize"]);
            bitmap = BitmapHelper.ScaleDown(bitmap, maxWebClassifierImageSize);
            return bitmap;
        }

        private void AddToShoppingCart(string guess)
        {
            var preferences = new ActivityPreferences(context, prefsName);
            preferences.AddString(guess.FirstCharToUpper());
            //shoppingCartItemAddedMessageBar.Show();
        }

        public ErrorDialogCreator GetShoppingCartAddItemDialog(RecognitionResult recognitionResult)
        {
            return new ErrorDialogCreator(context, context.Resources.GetString(Resource.String.shoppingCart), //Hint: nezinau ar cia tas pats, bet turetu veikt.
                context.Resources.GetString(Resource.String.shoppingCart),
                context.Resources.GetString(Resource.String.shoppingCart),
                context.Resources.GetString(Resource.String.shoppingCart),
                () => AddToShoppingCart(recognitionResult.BestPrediction), delegate { });
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
                    var item = (TextBlock)items.ValueAt(i);
                    ocrResultBuilder.Append(item.Value);
                    ocrResultBuilder.Append("|");
                }
                var ocrResult = ocrResultBuilder.ToString();
                return ocrResult;
            });
            return await task;
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
}
}