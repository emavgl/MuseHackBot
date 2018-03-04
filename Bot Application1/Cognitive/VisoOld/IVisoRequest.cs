using System.Threading.Tasks;

namespace ChatBot.Cognitive.Viso
{
    /// <summary>
    /// Common behaviour of all classes that are going to use the Microsoft Azure Viso services.
    /// </summary>
    interface IVisoRequest
    {

        /// <summary>
        /// Makes a server request with a given image as directory path or url.
        /// </summary>
        /// <param name="imagePath">path of the image that can be and url or directory.</param>
        /// <param name="isUrl">true if the path is an url false if is a directory path.</param>
        /// <returns>task that contains the response.</returns>
        Task<VisoResponseRootObject> MakeVisoRequest(string imagePath, bool isUrl);

        /// <summary>
        /// Makes a server request with a given byte array representing the image.
        /// </summary>
        /// <param name="image">byte array representing the image.</param>
        /// <returns>task that contains the response.</returns>
        Task<VisoResponseRootObject> MakeVisoRequest(byte[] image);
  
    }
}
