using ImageRecognition.Classificators;
using ShopLensApp.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ShopLensForms.Models;
using VoicedText.TextVoicers;
using VoiceRecognitionWithTextVoicer.VoiceRecognizers;
using ShopLensApp.ExtensionMethods;
using System.Configuration;
using System.Collections.Specialized;

namespace ShopLensForms.Controllers
{

    public class MainController
    {
        public static List<ShoppingItem> shoppingList;

        public string filePath = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName
            + ConfigurationManager.AppSettings.Get("filePath");

        /// <inheritdoc cref="ITextVoicer"/>
        private ITextVoicer _textVoicer;

        /// <inheritdoc cref="IVoiceRecognizer"/>
        private IVoiceRecognizer _voiceRecognizer;

        /// <inheritdoc cref="IImageClassificator"/>
        private IImageClassificator _imageClassifying;

        /// <summary>
        /// A windows form that the controller communicates with.
        /// </summary>
        public IntroForm _introForm;

        /// <inheritdoc cref="_introForm"/>
        public ShopLens _shopLens;

        /// <inheritdoc cref="_introForm"/>
        public MyListForm _myList;

        /// <inheritdoc cref="_introForm"/>
        public MyCartForm _myCart;

        private string helloCmd = ConfigurationManager.AppSettings.Get("helloCmd");
        private string whatIsThisCmd = ConfigurationManager.AppSettings.Get("whatIsThisCmd");
        private string startCmd = ConfigurationManager.AppSettings.Get("startCmd");
        private string exitCmd = ConfigurationManager.AppSettings.Get("exitCmd");
        private string myShoppingListCmd = ConfigurationManager.AppSettings.Get("myShoppingListCmd");
        private string myShoppingCartCmd = ConfigurationManager.AppSettings.Get("myShoppingCartCmd");
        private string addToShoppingListCmd = ConfigurationManager.AppSettings.Get("addToShoppingListCmd");
        private string closeShoppingListCmd = ConfigurationManager.AppSettings.Get("closeShoppingListCmd");
        private string closeShoppingCartCmd = ConfigurationManager.AppSettings.Get("closeShoppingCartCmd");

        public MainController(ITextVoicer textVoicer, IVoiceRecognizer voiceRecognizer
            , IImageClassificator imageClassificator, IntroForm introForm, ShopLens shopLens)
        {
            _textVoicer = textVoicer;
            _voiceRecognizer = voiceRecognizer;
            _imageClassifying = imageClassificator;
            _introForm = introForm;
            _shopLens = shopLens;
            _myList = new MyListForm(this);
            _myCart = new MyCartForm(this);

            _introForm.MainController = this;
            _shopLens.MainController = this;

            shoppingList = new List<ShoppingItem>();
        }

        [STAThread]
        public void StartApp()
        {
            StartVoiceRecognizer();
            LoadList(_myList.MyList_listBox);
            Application.Run(_introForm);
        }

        public void StartVoiceRecognizer()
        {
            _voiceRecognizer.AddCommand(helloCmd, CommandRecognized_Hello);
            _voiceRecognizer.AddCommand(whatIsThisCmd, CommandRecognized_WhatIsThis);
            _voiceRecognizer.AddCommand(startCmd, CommandRecognized_Start);
            _voiceRecognizer.AddCommand(exitCmd, CommandRecognized_Exit);
            _voiceRecognizer.AddCommand(myShoppingListCmd, CommandRecognized_MyShoppingList);
            _voiceRecognizer.AddCommand(myShoppingCartCmd, CommandRecognized_MyShoppingCart);
            _voiceRecognizer.AddCommand(addToShoppingListCmd, CommandRecognized_AddToShoppingList);
            _voiceRecognizer.AddCommand(closeShoppingListCmd, CommandRecognized_CloseShoppingList);
            _voiceRecognizer.AddCommand(closeShoppingCartCmd, CommandRecognized_CloseShoppingCart);
            _voiceRecognizer.StartVoiceRecognition();
        }

        public void StopVoiceRecognizer()
        {
            _voiceRecognizer.StopVoiceRecognition();
        }


        private void InvokeOnGUIThread(Form formToBeInvokedOn,
            Action<object, EventArgs> methodToBeInvoked, object sender, EventArgs e)
        {
            if (formToBeInvokedOn.InvokeRequired)
            {
                formToBeInvokedOn.BeginInvoke(new MethodInvoker(() => methodToBeInvoked(sender, e)));
            }
            else
            {
                methodToBeInvoked(sender, e);
            }
        }

        /// <summary> 
        /// Executes a certain method when a specific command is recognized. 
        /// </summary>
        private void CommandRecognized_Hello(object sender, EventArgs e)
        {
            _introForm.InvokeOnGUIThread_VoidObjEvArgs(_introForm.Enter_btn_Click, sender, e);
        }

        /// <inheritdoc cref="CommandRecognized_Hello(object, EventArgs)"/>
        private void CommandRecognized_WhatIsThis(object sender, EventArgs e)
        {
            _shopLens.InvokeOnGUIThread_VoidObjEvArgs(_shopLens.WhatIsThis_btn_Click, sender, e);
        }

        /// <inheritdoc cref="CommandRecognized_Hello(object, EventArgs)"/>
        private void CommandRecognized_Start(object sender, EventArgs e)
        {
            _shopLens.InvokeOnGUIThread_VoidObjEvArgs(_shopLens.Start_btn_Click, sender, e);
        }

        /// <inheritdoc cref="CommandRecognized_Hello(object, EventArgs)"/>
        private void CommandRecognized_Exit(object sender, EventArgs e)
        {
            _shopLens.InvokeOnGUIThread_VoidObjEvArgs(_shopLens.Exit_btn_Click, sender, e);
        }

        /// <inheritdoc cref="CommandRecognized_Hello(object, EventArgs)"/>
        private void CommandRecognized_MyShoppingList(object sender, EventArgs e)
        {
            InvokeOnGUIThread(_shopLens, _shopLens.MyList_btn_Click, sender, e);
        }

        /// <inheritdoc cref="CommandRecognized_Hello(object, EventArgs)"/>
        private void CommandRecognized_MyShoppingCart(object sender, EventArgs e)
        {
            InvokeOnGUIThread(_shopLens, _shopLens.MyCart_btn_Click, sender, e);
        }

        /// <inheritdoc cref="CommandRecognized_Hello(object, EventArgs)"/>
        private void CommandRecognized_AddToShoppingList(object sender, EventArgs e)
        {
            InvokeOnGUIThread(_myList, _myList.Add_btn_Click, sender, e);
        }

        /// <inheritdoc cref="CommandRecognized_Hello(object, EventArgs)"/>
        private void CommandRecognized_CloseShoppingList(object sender, EventArgs e)
        {
            InvokeOnGUIThread(_myList, _myList.Close_btn_Click, sender, e);
        }

        /// <inheritdoc cref="CommandRecognized_Hello(object, EventArgs)"/>
        private void CommandRecognized_CloseShoppingCart(object sender, EventArgs e)
        {
            InvokeOnGUIThread(_myCart, _myCart.Close_btn_Click, sender, e);
        }

        /// <summary>
        /// Executes logical operations related to the 'What is this' voice command.
        /// </summary>
        /// <param name="videoImage">The image which is evaluated with an Image Recognition model.</param>
        /// <param name="webcamTurnedOff">What to say when the user's webcam is turned off.</param>
        /// <param name="thisIs">A string of a 'This is' message.</param>
        /// <param name="noLblError">What to say when there are no labels in our Image Recognition model.</param>
        public void ExecuteCommand_WhatIsThis(Image videoImage, string webcamTurnedOff, string thisIs, string noLblError)
        {
            if (videoImage != null)
            {
                var image = (Image)videoImage.Clone();

                TextVoicerVoiceMessage(thisIs);

                var mostConfidentResult = GetMostConfidentResult(image);

                if (mostConfidentResult == null)
                {
                    TextVoicerVoiceMessage(noLblError);
                }
                else
                {
                    TextVoicerVoiceMessage(mostConfidentResult);
                }
            }
            else
            {
                TextVoicerVoiceMessage(webcamTurnedOff);
            }
        }

        /// <summary>
        /// Uses a text voicer object to voice a message.
        /// </summary>
        /// <param name="message">The string of a message to be voiced.</param>
        public void TextVoicerVoiceMessage(string message)
        {
            _textVoicer.SayMessage(message);
        }

        /// <summary>
        /// Uses an image classifier object to return a Dictionary of labels and their
        /// probabilities of being in a particular image.
        /// </summary>
        /// <param name="imgArray">A byte array of an image 
        /// to be classified by the image classifier object.</param>
        /// <returns>
        /// The returned dictionary format is: { label name: float (in range 0-1) }
        /// </returns>
        private Dictionary<string, float> ImageClassifyingClassifyImage(byte[] imgArray)
        {
            return _imageClassifying.ClassifyImage(imgArray);
        }

        /// <summary>
        /// Makes a particular Windows form visible to the user.
        /// </summary>
        /// <param name="formToBeShown">The form to be shown to the user.</param>
        /// <remarks>
        /// The if statement makes sure that if the user says, for example, 'Hello'
        /// many times the application will not crash.
        /// </remarks>
        public void ShowForm(Form formToBeShown)
        {
            if (formToBeShown.Visible == false)
            {
                formToBeShown.ShowDialog();
            }
        }

        /// <summary>
        /// Makes a particular Windows form invisible to the user.
        /// </summary>
        /// <param name="formToBeHiden">The form to be hiden from the user.</param>
        /// <remarks>
        /// The if statement makes sure that if the user says, for example, 'Close the list'
        /// many times the application will not crash.
        /// </remarks>
        public void HideForm(Form formToBeHidden)
        {
            if (formToBeHidden.Visible == true)
            {
                formToBeHidden.Hide();
            }
        }

        /// <summary>
        /// Load the list in particular Windows form listbox.
        /// </summary>
        /// <param name="listBoxToBeLoaded">The listbox where the list has to be loaded.</param>
        /// <remarks>
        /// The if statement makes sure that the application will not crash 
        /// after trying to convert null to array.
        /// </remarks>
        public void LoadList(ListBox listBoxToBeLoaded)
        {
            IReader source = new JsonReader();
            shoppingList = source.DeserializeToList(filePath);
            if (shoppingList != null)
            {
                listBoxToBeLoaded.Items.Clear();
                listBoxToBeLoaded.Items.AddRange(shoppingList.ToArray());
            }
        }

        /// <summary>
        /// Adds an item to the shopping list.
        /// </summary>
        public void AddItem()
        {
            string itemName = _myList.ItemToAdd_textBox.Text;
            ShoppingItem itemToAdd = new ShoppingItem(itemName);
            try
            {
                shoppingList.Add(itemToAdd);
            }
            catch (NullReferenceException e)
            {
                shoppingList = new List<ShoppingItem>();
                shoppingList.Add(itemToAdd);
            }

            _myList.MyList_listBox.Items.Clear();
            _myList.MyList_listBox.Items.AddRange(shoppingList.ToArray());

            IWriter write = new JsonWriter();
            write.SerializeFromList(filePath, shoppingList);  
        }

        /// <summary>
        /// Determines what item is most likely to be in a given image.
        /// </summary>
        /// <param name="image">The given image.</param>
        /// <returns>The most confident result of a labeled item being in the given image.</returns>
        public string GetMostConfidentResult(Image image)
        {
            var ms = new MemoryStream();

            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            var classificationResultsDictionary = ImageClassifyingClassifyImage(ms.ToArray());
            var classificationResults = classificationResultsDictionary
                .Select(x => new ImageRecognitionResultRow(x.Key, x.Value))
                .ToImageRecognitionResults();

            var bestResult = classificationResults.MostConfidentResult;

            return bestResult.Label;
        }
    }
}
