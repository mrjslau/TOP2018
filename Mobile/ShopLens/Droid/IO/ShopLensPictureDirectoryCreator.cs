using Java.IO;

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
                if (!pictureDirectory.Mkdirs())
                {
                    throw new System.IO.DirectoryNotFoundException("Unable to create directory for pictures.");  
                }
            }
        }
    }
}