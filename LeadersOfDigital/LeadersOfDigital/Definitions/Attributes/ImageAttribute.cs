using System;

namespace LeadersOfDigital.Definitions.Attributes
{
    public class ImageAttribute : Attribute
    {
        public ImageAttribute(string imageResource)
        {
            ImageResource = imageResource;
        }

        public string ImageResource { get; }
    }
}
