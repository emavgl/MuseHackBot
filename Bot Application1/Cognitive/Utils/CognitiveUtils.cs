using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Bot_Application1.Cognitive.Utils
{
    /// <summary>
    /// Useful methods that are needed for the cognitive services.
    /// </summary>
    public class CognitiveUtils
    {

        /// <summary>
        /// Converts an image path to a byte array.
        /// </summary>
        /// <param name="imagePath">path of the image.</param>
        /// <returns>the byte array representing that image.</returns>
        public static byte[] GetImageFromPath(string imagePath)
        {
            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

    }
}