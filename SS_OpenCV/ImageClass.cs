using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Windows.Forms;

namespace CG_OpenCV
{
    class ImageClass
    {

        /// <summary>
        /// Image Negative using EmguCV library
        /// Slower method
        /// </summary>
        /// <param name="img">Image</param>
        public static void Negative(Image<Bgr, byte> img)
        {
            int x, y;

            Bgr aux;
            for (y = 0; y < img.Height; y++)
            {
                for (x = 0; x < img.Width; x++)
                {
                    // acesso directo : mais lento 
                    aux = img[y, x];
                    img[y, x] = new Bgr(255 - aux.Blue, 255 - aux.Green, 255 - aux.Red);
                }
            }
        }

        /// <summary>
        /// Image Negative using direct memmory access
        /// Faster method
        /// </summary>
        /// <param name="img">Image</param>
        public static void NegativeFast(Image<Bgr, byte> img)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels;
                int padding = m.widthStep - m.nChannels * m.width;
                int x, y;

                if (m.nChannels == 3) // Checks if image is RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            // Updates values
                            dataPtr[0] = (byte)(255 - dataPtr[0]);
                            dataPtr[1] = (byte)(255 - dataPtr[1]);
                            dataPtr[2] = (byte)(255 - dataPtr[2]);

                            dataPtr += m.nChannels;
                        }
                        dataPtr += padding;
                    }
                }
                
            }
        }

        /// <summary>
        /// Convert to gray
        /// Direct access to memory - faster method
        /// </summary>
        /// <param name="img">image</param>
        public static void ConvertToGray(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //retrive 3 colour components
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            // convert to gray
                            gray = (byte)Math.Round(((int)blue + green + red) / 3.0);

                            // store in the image
                            dataPtr[0] = gray;
                            dataPtr[1] = gray;
                            dataPtr[2] = gray;

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        /// <summary>
        /// Obtains the RED color component of an image (direct memory access)
        /// </summary>
        /// <param name="img"></param>
        public static void RedChannel(Image<Bgr, byte> img)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels;
                int padding = m.widthStep - nChan * width;
                int x, y;

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        // Updates values
                        dataPtr[0] = dataPtr[2];
                        dataPtr[1] = dataPtr[2];

                        dataPtr += nChan;
                    }
                    dataPtr += padding;
                }
            }
        }

        /// <summary>
        /// Adjusts brightness and contrast levels of an image from user input
        /// Fast - Direct memory access
        /// </summary>
        /// <param name="img"></param>
        public static void BrightContrast(Image<Bgr, byte> img, int brightness, double contrast)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels;
                int padding = m.widthStep - nChan * width;

                int x, y;

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        for (int i = 0; i < nChan; i++)
                        {
                            dataPtr[i] = (byte)(contrast * dataPtr[i] + brightness);
                        }

                        dataPtr += nChan;
                    }
                    dataPtr += padding;
                }
            }
        }

        public static void Translation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, int dx, int dy) {
            unsafe {
                MIplImage m = img.MIplImage;
                MIplImage mC = imgCopy.MIplImage;

                byte* dataPtr = (byte*) m.imageData.ToPointer();
                byte* dataPtrC = (byte*) mC.imageData.ToPointer();

                byte blue, green, red;
                int width = img.Width;
                int height = img.Height;
                int wStep = m.widthStep;
                int nChan = m.nChannels;
                int padding = m.widthStep - nChan * m.width;
                int x, y, nx, ny;

                if (nChan == 3) {
                    for (y = 0; y < height; y++) {
                        for (x = 0; x < width; x++) {
                            nx = x - dx;
                            ny = y - dy;

                            if ((nx > 0) && (ny > 0) && (nx < width && ny < height)) {

                            }
                        }
                    }
                }
            }
        }

        public static void Rotation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float angle) {
            // TODO
        }

        public static void Scale(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor) {
            // TODO
        }

        public static void Scale_point_xy(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor, int centerX, int centerY) {
            // TODO
        }
    }
}
