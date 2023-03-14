using System;
using System.Diagnostics;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace CG_OpenCV
{ 
    public partial class MainForm : Form
    {
        Image<Bgr, Byte> img = null; // working image
        Image<Bgr, Byte> imgUndo = null; // undo backup image - UNDO
        string title_bak = "";

        public MainForm()
        {
            InitializeComponent();
            title_bak = Text;
        }

        /// <summary>
        /// Opens a new image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                img = new Image<Bgr, byte>(openFileDialog1.FileName);
                Text = title_bak + " [" +
                        openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf("\\") + 1) +
                        "]";
                imgUndo = img.Copy();
                ImageViewer.Image = img.Bitmap;
                ImageViewer.Refresh();
            }
        }

        /// <summary>
        /// Saves an image with a new name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ImageViewer.Image.Save(saveFileDialog1.FileName);
            }
        }

        /// <summary>
        /// Closes the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// restore last undo copy of the working image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imgUndo == null) // verify if the image is already opened
                return; 
            Cursor = Cursors.WaitCursor;
            img = imgUndo.Copy();

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        /// <summary>
        /// Change visualization mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // zoom
            if (autoZoomToolStripMenuItem.Checked)
            {
                ImageViewer.SizeMode = PictureBoxSizeMode.Zoom;
                ImageViewer.Dock = DockStyle.Fill;
            }
            else // with scroll bars
            {
                ImageViewer.Dock = DockStyle.None;
                ImageViewer.SizeMode = PictureBoxSizeMode.AutoSize;
            }
        }

        /// <summary>
        /// Show authors form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuthorsForm form = new AuthorsForm();
            form.ShowDialog();
        }

        /// <summary>
        /// Calculate the image negative
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void negativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.Negative(img);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        /// <summary>
        /// Calculate the image negative (the fast way, i.e. direct memory access)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void negativeFastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verifies if the image is already open
                return;

            Cursor = Cursors.WaitCursor;

            // copy Undo Image
            imgUndo = img.Copy();

            ImageClass.NegativeFast(img);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refreshes the image on the screen

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Call automated image processing check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void evalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EvalForm eval = new EvalForm();
            eval.ShowDialog();
        }

        /// <summary>
        /// Call image convertion to gray scale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.ConvertToGray(img);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        /// <summary>
        /// Image convertion to RED grayscale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void redGrayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verifies if the image is already open
                return;

            Cursor = Cursors.WaitCursor;

            // copy Undo Image
            imgUndo = img.Copy();

            ImageClass.RedChannel(img);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh();

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Image brightness and contrast adjustment from user input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void adjustBrightnessAndContrastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verifies if the image is already open
                return;

            Cursor = Cursors.WaitCursor;

            // copy Undo Image
            imgUndo = img.Copy();

            // Gets user input
            InputBox brightnessInput = new InputBox("Brightness (0-255):");
            brightnessInput.ShowDialog();

            InputBox contrastInput = new InputBox("Contrast (0-3):");
            contrastInput.ShowDialog();

            int brightness = Convert.ToInt32(brightnessInput.ValueTextBox.Text);
            float contrast = float.Parse(contrastInput.ValueTextBox.Text);

            ImageClass.BrightContrast(img, brightness, contrast);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh();

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Image translation from user input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void translationToolStripMenuItem_Click(object sender, EventArgs e) {
            if (img == null)
                return;

            Cursor = Cursors.WaitCursor;

            // copy Undo image
            imgUndo = img.Copy();

            // Gets user input
            InputBox translationInputX = new InputBox("Translation (X axis)");
            translationInputX.ShowDialog();

            InputBox translationInputY = new InputBox("Translation (Y axis)");
            translationInputY.ShowDialog();

            int translationX = Convert.ToInt32(translationInputX.ValueTextBox.Text);
            int translationY = Convert.ToInt32(translationInputY.ValueTextBox.Text);

            // Original image copy
            Image<Bgr, byte> imgCopy = img.Copy();

            ImageClass.Translation(img, imgCopy, translationX, translationY);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh();

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Image rotation from user input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rotationToolStripMenuItem_Click(object sender, EventArgs e) {
            if (img == null)
                return;

            Cursor = Cursors.WaitCursor;

            // copy Undo image
            imgUndo = img.Copy();

            // Gets user input
            InputBox angleInDegrees = new InputBox("Angle (degrees)");
            angleInDegrees.ShowDialog();

            float angleInRad = (float)(Convert.ToInt32(angleInDegrees.ValueTextBox.Text) * Math.PI / 180);

            // Original image copy
            Image<Bgr, Byte> imgCopy = img.Copy();

            ImageClass.Rotation(img, imgCopy, angleInRad);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh();

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Image scaling from user input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zoomToolStripMenuItem_Click(object sender, EventArgs e) {
            if (img == null)
                return;

            Cursor = Cursors.WaitCursor;

            // copy Undo image
            imgUndo = img.Copy();

            // Gets user input
            InputBox scaleFactor = new InputBox("Scale factor ('1' original size)");
            scaleFactor.ShowDialog();

            float scaleFactorVal = (float)Convert.ToDouble(scaleFactor.ValueTextBox.Text);

            // Original image copy
            Image<Bgr, byte> imgCopy = img.Copy();

            ImageClass.Scale(img, imgCopy, scaleFactorVal);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh();

            Cursor = Cursors.Default;
        }
    }
}