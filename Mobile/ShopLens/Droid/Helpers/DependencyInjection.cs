using ImageRecognition.Classificators;
using ImageRecognitionMobile.Classificators;
using Unity;
using Unity.Lifetime;

namespace ShopLens.Droid.Helpers
{
    public static class DependencyInjection
    {
        public static IUnityContainer Container { get; } = new UnityContainer();

        public static void RegisterInterfaces()
        {
            // Singleton-like
            Container.RegisterType<IDirectoryCreator, ShopLensPictureDirectoryCreator>(
                new ContainerControlledLifetimeManager());

            // New instance each time resolved
            Container.RegisterType<IAsyncImageClassificator, WebClassificator>(
                new ContainerControlledTransientManager());
        }
    }
}