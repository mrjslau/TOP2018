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
using ImageRecognitionMobile.OCR;
using System.Linq;

namespace ShopLens.Droid
{
    class ImageRecognizer
    {
        TextToSpeech tts;
        bool isTtsInitialized = false;
        ShopLensTextToSpeech sltts;
        TextRecognizer textRecognizer;
        ActivityPreferences prefs;
        Context context;
        Activity activity;
        Bitmap bitmap;
        Camera2Fragment owner;
        private readonly string prefsName = ConfigurationManager.AppSettings["ShopCartPrefs"];
        readonly string shopName = ConfigurationManager.AppSettings["ShopName"];

        public ImageRecognizer(Context context, Activity activity)
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
            Task.Run(async () =>
            {
                var image = GetResizedImageForClassification(bitmap);
                var classifyImageTask = GetClassifyImageTask(image);
                if (!textRecognizer.IsOperational)
                {
                    System.Diagnostics.Debug.WriteLine("[Warning]: Text recognizer not operational.");
                    return new RecognitionResult { Predictions = await classifyImageTask };
                }
                var recognizeTextTask = GetTextFromImageAsync(image);
                await Task.WhenAll(classifyImageTask, recognizeTextTask);
                var ocrResult = recognizeTextTask.Result;
                var weightString = new RegexMetricWeightSubstringFinder().FindWeightSpecifier(ocrResult);
                var recognitionResult = new RecognitionResult
                {
                    RawOcrResult = ocrResult,
                    Predictions = classifyImageTask.Result,
                    WeightSpecifier = weightString
                };
                return recognitionResult;
            })
                .ContinueWith(task =>
                {
                    //progressBar.Visibility = ViewStates.Gone;
                    if (task.IsFaulted)
                    {
                        System.Diagnostics.Debug.WriteLine(task.Exception);
                        return;
                    }
                    var recognitionResult = task.Result;
                    string productName = recognitionResult.BestPrediction;

                    var thingsToSay = new List<string>
                        {$"This is {productName}", recognitionResult.WeightSpecifier};
                    tts.Speak(
                        string.Join(". ", thingsToSay),
                        QueueMode.Flush,
                        null,
                        null);
                    var ocrResult = recognitionResult.RawOcrResult;
                    decimal price = 0;

                    var product =
                            MainActivity.shopLensDbContext.Products
                                .FirstOrDefault(p => p.Name == productName && p.Shop.Name == shopName);

                    if (product != null)
                    {
                        price = product.FullPrice;
                    }

                    //if (!string.IsNullOrEmpty(ocrResult))
                        //new MessageBarCreator(rootView, $"OCR: {ocrResult}").Show();
                    GetShoppingCartAddItemDialog(recognitionResult, price).Show();
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private Bitmap GetResizedImageForClassification(Bitmap bitmap)
        {
            var maxWebClassifierImageSize = int.Parse(ConfigurationManager.AppSettings["webClassifierImgSize"]);
            bitmap = BitmapHelper.ScaleDown(bitmap, maxWebClassifierImageSize);
            return bitmap;
        }

        private void AddToShoppingCart(string guess, decimal price, int quantity = 1)
        {
            var preferences = new ActivityPreferences(context, prefsName);
            preferences.AddCartItem(guess, price.ToString(), quantity);
            //shoppingCartItemAddedMessageBar.Show();
        }

        public ErrorDialogCreator GetShoppingCartAddItemDialog(RecognitionResult recognitionResult, decimal price)
        {
            return new ErrorDialogCreator(context, context.Resources.GetString(Resource.String.shoppingCart),
                context.Resources.GetString(Resource.String.shoppingCartQuestion),
                context.Resources.GetString(Resource.String.positiveMessage),
                context.Resources.GetString(Resource.String.negativeMessage),
                () => AddToShoppingCart(recognitionResult.BestPrediction, price), delegate { });
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