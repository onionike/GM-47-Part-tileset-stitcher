using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.IO;


namespace TileStitcher
{
    public partial class Form1 : Form
    {
        public const int DEST_TILES_WIDE = 16;
        public const int DEST_TILES_HIGH = 14;
        public const int SOURCE_TILES_WIDE = 4;
        public const int SOURCE_TILES_HIGH = 6;

        //public const int DEST_TILES_WIDE = 8;
        //public const int DEST_TILES_HIGH = 6;
        //public const int SOURCE_TILES_WIDE = 5;
        //public const int SOURCE_TILES_HIGH = 4;
        //public const int SUBTILE_WIDTH = 32;
        //public const int SUBTILE_HEIGHT = 32;
        //public const int TILE_WIDTH = SUBTILE_WIDTH * 2;
        //public const int TILE_HEIGHT = SUBTILE_HEIGHT * 2;

        //The hard coded instructions for how to cut tiles from the source to the final stitched image (DO NOT CHANGE!)
        public int[] instructions = new int[] {
            5,
            6,
        21,
        6,
        5,
        20,
        21,
        20,
        5,
        6,
        21,
        6,
        5,
        20,
        21,
        20,
        9,
        10,
        9,
        10,
        9,
        10,
        9,
        10,
        9,
        16,
        9,
        16,
        9,
        16,
        9,
        16,
        5,
        6,
        21,
        6,
        5,
        20,
        21,
        20,
        5,
        6,
        21,
        6,
        5,
        20,
        21,
        20,
        17,
        10,
        17,
        10,
        17,
        10,
        17,
        10,
        17,
        16,
        17,
        16,
        17,
        16,
        17,
        16,
        4,
        5,
        4,
        20,
        4,
        5,
        4,
        20,
        1,
        2,
        1,
        2,
        1,
        2,
        1,
        2,
        8,
        9,
        8,
        9,
        8,
        16,
        8,
        16,
        5,
        6,
        5,
        16,
        17,
        6,
        17,
        16,
        6,
        7,
        6,
        7,
        21,
        7,
        21,
        7,
        9,
        10,
        21,
        10,
        9,
        20,
        21,
        20,
        10,
        11,
        17,
        11,
        10,
        11,
        17,
        11,
        13,
        14,
        13,
        14,
        13,
        14,
        13,
        14,
        4,
        7,
        1,
        2,
        0,
        1,
        0,
        1,
        2,
        3,
        2,
        3,
        10,
        11,
        21,
        11,
        8,
        11,
        13,
        14,
        4,
        5,
        4,
        16,
        6,
        7,
        17,
        7,
        14,
        15,
        14,
        15,
        8,
        9,
        8,
        20,
        0,
        3,
        0,
        1,
        8,
        11,
        2,
        3,
        0,
        3,
        5,
        6,
        12,
        13,
        12,
        13,
        4,
        7,
        12,
        13,
        12,
        15,
        14,
        15,
        12,
        15,
        9,
        10,
        18,
        19,
        5,
        5,
        5,
        5,
        5,
        5,
        5,
        5,
        5,
        5,
        5,
        5,
        5,
        5,
        22,
        23,
        5,
        5,
        5,
        5,
        5,
        5,
        5,
        5,
        5,
        5,
        5,
        5,
        5,
        5 }; //This line is new
        /*
            OLD INSTRUCTIONS
            
            public int[] instructions = new int[] {6,7,11,12,19,7,11,12,6,9,11,12,19,9,11,12,6,7,11,4,
            19,7,11,4,6,9,11,4,19,9,11,4,6,7,14,12,19,7,14,12,6,9,14,12,19,9,14,12,6,7,14,4,19,7,14,4,
            6,9,14,4,19,9,14,4,5,6,10,11,5,9,10,11,5,6,10,4,5,9,10,4,1,2,6,7,1,2,6,4,1,2,14,7,1,2,14,4,
            7,8,12,13,7,8,14,13,19,8,12,13,19,8,14,13,11,12,16,17,19,12,16,17,11,9,16,17,19,9,16,17,5,8,
            10,13,1,2,16,17,0,1,5,6,0,1,5,4,2,3,7,8,2,3,14,8,12,13,17,18,19,13,17,18,10,11,15,16,10,9,15,
            16,0,3,5,8,0,1,15,16,10,13,15,18,2,3,17,18,0,3,15,18,6,7,11,12};
        */

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void LoadTilesets_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.openFileDialog1.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                progressBar1.Value = 0;
                foreach (String file in openFileDialog1.FileNames)
                {
                    if (checkedListBox1.Items.Contains(file) == false)
                    {
                        checkedListBox1.Items.Add(file, true);
                    }
                }
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {
        }

        private void StitchSelectedTilesets_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.folderBrowserDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                int TILE_WIDTH = (int)finalTileWidth.Value / 2;
                int TILE_HEIGHT = (int)finalTileHeight.Value / 2;

                if (checkedListBox1.CheckedItems.Count > -1)
                {
                    progressBar1.Maximum = checkedListBox1.CheckedItems.Count;


                    foreach (string imagePath in checkedListBox1.CheckedItems)
                    {
                        //Load the image
                        Image originalImage = Image.FromFile(imagePath);
                        Bitmap sourceBitmap = new Bitmap(originalImage);

                        //Create the stitched image
                        Bitmap stitchedBitmap = new Bitmap(DEST_TILES_WIDE * TILE_WIDTH, DEST_TILES_HIGH * TILE_HEIGHT);

                        //Stitch image
                        //For stepping through the output 47tileset and source 20tileset
                        int index = 0;
                        int sourcex = 0;
                        int sourcey = 0;
                        int destx = 0;
                        int desty = 0;
                        int instructionIndex = 0;
                        int instructionLength = instructions.Length;
                        //Cycle through positions in the 47 piece output tileset
                        for (int dy = 0; dy < DEST_TILES_HIGH; dy++)
                        {
                            for (int dx = 0; dx < DEST_TILES_WIDE; dx++)
                            {
                                ////Cycle through subpositions of tiles
                                //for (int sdy = 0; sdy < 2; sdy++)
                                //{
                                //    for (int sdx = 0; sdx < 2; sdx++)
                                //    {
                                //        //Calculate the drawing position on the destination bitmap
                                //        destx = TILE_WIDTH * dx + SUBTILE_WIDTH * sdx;
                                //        desty = TILE_HEIGHT * dy + SUBTILE_HEIGHT * sdy;

                                //        //Get instruction
                                //        index = instructions[instructionIndex];
                                //        instructionIndex++;

                                //        //Handle instruction
                                //        sourcex = index % SOURCE_TILES_WIDE;
                                //        sourcey = index / SOURCE_TILES_WIDE;

                                //        sourcex *= SUBTILE_WIDTH;
                                //        sourcey *= SUBTILE_HEIGHT;

                                //        //Copy the pixels manually from the source to the destination
                                //        for (int py = 0; py < SUBTILE_HEIGHT; py++)
                                //        {
                                //            for (int px = 0; px < SUBTILE_WIDTH; px++)
                                //            {
                                //                stitchedBitmap.SetPixel(destx + px, desty + py, sourceBitmap.GetPixel(sourcex + px, sourcey + py));
                                //            }
                                //        }
                                //    }
                                //}
                               
                                //Calculate the drawing position on the destination bitmap
                                destx = TILE_WIDTH * dx;
                                desty = TILE_HEIGHT * dy;

                                //Get instruction
                                index = instructions[instructionIndex];
                                instructionIndex++;

                                //Handle instruction
                                sourcex = index % SOURCE_TILES_WIDE;
                                sourcey = index / SOURCE_TILES_WIDE;

                                sourcex *= TILE_WIDTH;
                                sourcey *= TILE_HEIGHT;

                                //Copy the pixels manually from the source to the destination
                                for (int py = 0; py < TILE_HEIGHT; py++)
                                {
                                    for (int px = 0; px < TILE_WIDTH; px++)
                                    {
                                        if (instructionIndex <= instructionLength)
                                        {
                                            stitchedBitmap.SetPixel(destx + px, desty + py, sourceBitmap.GetPixel(sourcex + px, sourcey + py));
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }//end for

                        //Save final result
                        string[] sections = imagePath.Split('\\');
                        string newPath = folderBrowserDialog1.SelectedPath + '\\' + sections[sections.Length - 1];
                        stitchedBitmap.Save(newPath);
                        progressBar1.PerformStep();
                    }//end foreach

                    MessageBox.Show("Done stitching tiles");

                    if (openFolderAfterStitching.Enabled)
                    {
                        //DialogResult showResultDialogResult = this.openFileResultDialog.ShowDialog();
                        Process.Start(folderBrowserDialog1.SelectedPath);
                    }
                }
                else
                {
                    MessageBox.Show("You must select tileset images to stitch");
                }
            }
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
