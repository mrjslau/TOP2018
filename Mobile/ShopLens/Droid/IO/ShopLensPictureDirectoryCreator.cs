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
            pictureDirectory = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "DCIM");
            if (!pictureDirectory.Exists())
            {
                if (!pictureDirectory.Mkdirs())
                {
                    // TO DO: throw valid exception.
                    throw new System.Exception();
                }
            }
        }
    }
}