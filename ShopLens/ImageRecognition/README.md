# ImageRecognition
A component of ShopLens responsible for classifying objects in an image.

## Development notes

### Prerequisites

You should have .NET Core installed. 
https://www.microsoft.com/net/learn/dotnet/hello-world-tutorial (choose your OS in dropdown)

If your process is crashing with error "could not find libtenserflow.dll" or similar -
- Find your *"users/User/.nuget/packages/tensorflowsharp"* folder  
- Copy the contents of *"runtimes/{**YOUR OS NAME**}/native/"* into project root folder

