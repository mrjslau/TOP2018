﻿using Java.IO;

namespace ShopLens.Droid
{
    class ShopLensPictureDirectoryCreator : IDirectoryCreator
    {

        public void CreateDirectory(File pictureDirectory)
        {
            CreateDirectoryForPictures(pictureDirectory);
        }

        private void CreateDirectoryForPictures(File pictureDirectory)
        {
            if (!pictureDirectory.Exists())
            {
                pictureDirectory.Mkdirs();
            }
        }
    }
}