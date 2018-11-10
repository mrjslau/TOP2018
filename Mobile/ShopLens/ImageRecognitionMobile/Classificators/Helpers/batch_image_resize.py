# NOTE:
# Need to have Python installed
# Need to have PIL (Python module) installed with pip
# ! Don't forget to change the path and out_dir variables

import os

from PIL import Image

# Set this to directory path
path = "C:\\Users\\Edvard\\RiderProjects\\TOP2018\\Mobile\\ShopLens\\fruits"
out_dir = "C:\\Users\\Edvard\\RiderProjects\\TOP2018\\Mobile\\ShopLens\\fruits_resized"
dirs = os.listdir(path)
final_size = 512

def resize_aspect_fit():
    for directory in dirs:
        print(f"Looking at dir '{directory}'")
        for item in os.listdir(os.path.join(path, directory)):
            fullFileName = os.path.join(path, directory, item)
            if os.path.isfile(fullFileName):
                try:
                    im = Image.open(fullFileName)
                    size = im.size
                    ratio = float(final_size) / min(size)
                    new_image_size = tuple([int(x*ratio) for x in size])
                    im = im.resize(new_image_size, Image.ANTIALIAS)
                    new_im = Image.new("RGB", (min(final_size, new_image_size[0]), min(final_size, new_image_size[1])))
                    new_im.paste(im, ((final_size-new_image_size[0])//2, (final_size-new_image_size[1])//2))

                    out_path = os.path.join(out_dir, directory)

                    if not os.path.exists(out_path):
                        os.makedirs(out_path)

                    new_im.save(os.path.join(out_path, item + '_resized.png'), 'PNG')

                except Exception as e:
                    print(f"Could not load file '{fullFileName}': {e}")
                    # just ignore any errors, probably corrupted files
                
resize_aspect_fit()