using System;

namespace BLL_Core.Attributes
{
    /// <summary>
    ///     EnumDescriptionAttribute attribute class
    /// </summary>
    public class EnumDescriptionAttribute : Attribute
    {
        public EnumDescriptionAttribute(string text_)
        {
            Text = text_;
        }

        public string Text { get; set; }
    }
}