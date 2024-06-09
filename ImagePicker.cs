using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Maker_Frontend
{
    public class ImagePicker
    {
        public async Task<FileResult> PickImageAsync()
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Please pick a photo"
            });
            return result;
        }
    }
}
