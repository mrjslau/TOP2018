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
using System.Threading;
using Java.Util;

namespace ShopLens.Droid
{
    class ImageRecognizer
    {
        public static bool areVoiceCommandsOn = false;

        TextToSpeech tts;
        bool isTtsInitialized = false;
        ShopLensTextToSpeech sltts;
        ShopLensSpeechRecognizer shopLensVoiceRec;
        TextRecognizer textRecognizer;
        ActivityPreferences prefs;
        Context context;
        Activity activity;
        Bitmap bitmap;
        Camera2Fragment owner;
        public static MainActivity mainMenu;
        private readonly string prefsName = ConfigurationManager.AppSettings["ShopCartPrefs"];
        readonly string shopName = ConfigurationManager.AppSettings["ShopName"];

        string guess;
        decimal price;

        public ImageRecognizer(Context context, Activity activity)
        {
            this.activity = activity;
            this.context = context;
            textRecognizer = new TextRecognizer.Builder(context).Build();
            shopLensVoiceRec = new ShopLensSpeechRecognizer(OnVoiceRecResults);
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

        private void OnVoiceRecResults(object sender, ShopLensSpeechRecognizedEventArgs e)
        {
            var results = e.Phrase;
            if (results == "yes")
            {
                AddToShoppingCart(guess, price);
                tts.Speak(guess + " was added to your shopping cart.", QueueMode.Flush, null, null);
                while (tts.IsSpeaking)
                {
                    Thread.Sleep(500);
                }

                mainMenu.WhatWouldUHaveMeDo();
            }
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
                    var ocrResult = recognitionResult.RawOcrResult;
                    string productName = recognitionResult.BestPrediction;

                    if (productName == "can" || productName == "packet")
                    {
                        productName += " of " + ocrResult;
                    }

                    guess = productName.FirstCharToUpper();
                    productName = productName.FirstCharToUpper();

                    price = 0;

                    var simpleProductName = recognitionResult.BestPrediction;

                    var product =
                            MainActivity.shopLensDbContext.Products
                                .FirstOrDefault(p => p.Name == simpleProductName && p.Shop.Name == shopName);

                    if (product != null)
                    {
                        price = product.FullPrice;
                    }

                    var thingsToSay = "This is " + productName + ". It costs " + price + " euros.";
                    tts.SetSpeechRate((float)0.8);
                    tts.Speak(
                        thingsToSay,
                        QueueMode.Flush,
                        null,
                        null);

                    while (tts.IsSpeaking) { Thread.Sleep(500); }

                    //if (!string.IsNullOrEmpty(ocrResult))
                    //new MessageBarCreator(rootView, $"OCR: {ocrResult}").Show();
                    if (areVoiceCommandsOn)
                    {
                        tts.Speak("Would you like to add " + productName + " to your shopping cart?", QueueMode.Flush, null, null);
                        while (tts.IsSpeaking) { Thread.Sleep(500);}
                        shopLensVoiceRec.ListenForAPhrase();
                    }
                    else
                    {
                        GetShoppingCartAddItemDialog(productName, price).Show();
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private Bitmap GetResizedImageForClassification(Bitmap bitmap)
        {
            var maxWebClassifierImageSize = int.Parse(ConfigurationManager.AppSettings["webClassifierImgSize"]);
            bitmap = BitmapHelper.ScaleDown(bitmap, maxWebClassifierImageSize);
            return bitmap;
        }

        public void AddToShoppingCart(string guess, decimal price, int quantity = 1)
        {
            var preferences = new ActivityPreferences(context, prefsName);
            preferences.AddCartItem(guess, price.ToString(), quantity);
            //shoppingCartItemAddedMessageBar.Show();
        }

        public ErrorDialogCreator GetShoppingCartAddItemDialog(string recognitionResult, decimal price)
        {
            return new ErrorDialogCreator(context, context.Resources.GetString(Resource.String.shoppingCart),
                context.Resources.GetString(Resource.String.shoppingCartQuestion),
                context.Resources.GetString(Resource.String.positiveMessage),
                context.Resources.GetString(Resource.String.negativeMessage),
                () => AddToShoppingCart(recognitionResult, price), delegate { });
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
                    ocrResultBuilder.Append(" ");
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