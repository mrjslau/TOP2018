# ImageRecognition
A component of ShopLens responsible for classifying objects in an image.

## Development notes

### Prerequisites

If your process is crashing with error "could not find libtenserflow.dll" or similar -
- Find your *"users/User/.nuget/packages/tensorflowsharp"* folder  
- Copy the contents of *"runtimes/{**YOUR OS NAME**}/native/"* into project root folder


For OCR: https://github.com/doxakis/How-to-use-tesseract-ocr-4.0-with-csharp


If you want to use the web classifier - ask a teammate for the App.config containing
needed secret keys.