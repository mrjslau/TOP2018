# ImageRecognition
A component of ShopLens responsible for classifying objects in an image.

## Development notes
If your process is crashing with error "could not find libtenserflow.dll" or similar -
- Find your *"users/User/.nuget/packages/tensorflowsharp"* folder  
- Copy the contents of *"runtimes/{**YOUR OS NAME**}/native/"* into project root folder

