﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace quick_picture_viewer
{
	partial class InfoForm : Form
	{
		private string fullPath = null;
		private bool darkMode;

		public InfoForm(Bitmap bitmap, string directoryName, string fileName, bool darkMode)
		{
			this.darkMode = darkMode;
			if (darkMode)
			{
				this.HandleCreated += new EventHandler(ThemeManager.formHandleCreated);
			}

			InitializeComponent();

			copyTooltip.SetToolTip(copyNameButton, "Copy value");
			copyTooltip.SetToolTip(copyFolderButton, "Copy value");
			copyTooltip.SetToolTip(copyPathButton, "Copy value");

			if (darkMode)
			{
				this.BackColor = ThemeManager.BackColorDark;
				this.ForeColor = Color.White;

				fileGroup.Paint += ThemeManager.PaintDarkGroupBox;
				sizeGroup.Paint += ThemeManager.PaintDarkGroupBox;
				dateGroup.Paint += ThemeManager.PaintDarkGroupBox;

				propertiesButton.BackColor = ThemeManager.SecondColorDark;
				propertiesButton.Image = Properties.Resources.white_imgfile;

				okButton.BackColor = ThemeManager.SecondColorDark;

				fileNameTextBox.BackColor = ThemeManager.SecondColorDark;
				fileNameTextBox.ForeColor = Color.White;

				folderTextBox.BackColor = ThemeManager.SecondColorDark;
				folderTextBox.ForeColor = Color.White;

				fullPathTextBox.BackColor = ThemeManager.SecondColorDark;
				fullPathTextBox.ForeColor = Color.White;

				compressionTextBox.BackColor = ThemeManager.SecondColorDark;
				compressionTextBox.ForeColor = Color.White;

				extensionTextBox.BackColor = ThemeManager.SecondColorDark;
				extensionTextBox.ForeColor = Color.White;

				sizeTextBox.BackColor = ThemeManager.SecondColorDark;
				sizeTextBox.ForeColor = Color.White;

				megapixelsTextBox.BackColor = ThemeManager.SecondColorDark;
				megapixelsTextBox.ForeColor = Color.White;

				resolutionTextBox.BackColor = ThemeManager.SecondColorDark;
				resolutionTextBox.ForeColor = Color.White;

				inchesTextBox.BackColor = ThemeManager.SecondColorDark;
				inchesTextBox.ForeColor = Color.White;

				cmTextBox.BackColor = ThemeManager.SecondColorDark;
				cmTextBox.ForeColor = Color.White;

				diskSizeTextBox.BackColor = ThemeManager.SecondColorDark;
				diskSizeTextBox.ForeColor = Color.White;

				ratioTextBox.BackColor = ThemeManager.SecondColorDark;
				ratioTextBox.ForeColor = Color.White;

				createdTextBox.BackColor = ThemeManager.SecondColorDark;
				createdTextBox.ForeColor = Color.White;

				modifiedTextBox.BackColor = ThemeManager.SecondColorDark;
				modifiedTextBox.ForeColor = Color.White;

				copyNameButton.Image = Properties.Resources.white_copy;
				copyNameButton.BackColor = ThemeManager.BackColorDark;
				copyFolderButton.Image = Properties.Resources.white_copy;
				copyFolderButton.BackColor = ThemeManager.BackColorDark;
				copyPathButton.Image = Properties.Resources.white_copy;
				copyPathButton.BackColor = ThemeManager.BackColorDark;
			}

			if (directoryName != null)
			{
				string path = Path.Combine(directoryName, fileName);
				fullPath = path;

				fileNameTextBox.Text = fileName;
				folderTextBox.Text = directoryName;
				fullPathTextBox.Text = path;

				diskSizeTextBox.Text = bytesToSize(path);
				extensionTextBox.Text = Path.GetExtension(path).Substring(1, Path.GetExtension(path).Length - 1).ToLower();

				createdTextBox.Text = File.GetCreationTime(path).ToShortDateString() + " - " + File.GetCreationTime(path).ToLongTimeString();
				modifiedTextBox.Text = File.GetLastWriteTime(path).ToShortDateString() + " - " + File.GetLastWriteTime(path).ToLongTimeString();

				propertiesButton.Enabled = true;
			} 
			else
			{
				if (darkMode)
				{
					propertiesButton.Image = null;
					propertiesButton.BackColor = ThemeManager.BackColorDark;
				}
			}

			double inchesWidth = bitmap.Width / bitmap.HorizontalResolution;
			double inchesHeight = bitmap.Height / bitmap.VerticalResolution;
			double cmWidth = inchesWidth * 2.54;
			double cmHeight = inchesHeight * 2.54;

			compressionTextBox.Text = getImageCompression(bitmap);

			sizeTextBox.Text = bitmap.Width + " x " + bitmap.Height + " pixels";
			megapixelsTextBox.Text = ((((float) bitmap.Height * bitmap.Width) / 1000000)).ToString("0.##") + " megapixels";
			resolutionTextBox.Text = Math.Round(bitmap.HorizontalResolution) + " x " + Math.Round(bitmap.VerticalResolution) + " DPI";
			inchesTextBox.Text = inchesWidth.ToString("0.##") + " x " + inchesHeight.ToString("0.##") + " inches";
			cmTextBox.Text = cmWidth.ToString("0.##") + " x " + cmHeight.ToString("0.##") + " centimeters";
			ratioTextBox.Text = string.Format("{0} : {1}", bitmap.Width / GCD(bitmap.Width, bitmap.Height), bitmap.Height / GCD(bitmap.Width, bitmap.Height));
		}

		private int GCD(int a, int b)
		{
			int Remainder;

			while (b != 0)
			{
				Remainder = a % b;
				a = b;
				b = Remainder;
			}

			return a;
		}

		private string bytesToSize(string path)
		{
			string[] sizes = { "B", "KB", "MB", "GB", "TB" };
			double len = new FileInfo(path).Length;
			int order = 0;
			while (len >= 1024 && order < sizes.Length - 1)
			{
				order++;
				len = len / 1024;
			}

			return String.Format("{0:0.##} {1}", len, sizes[order]);
		}

		private string getImageCompression(Bitmap bitmap)
		{
			string result = "Unknown";

			if (bitmap.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
			{
				result = "PNG";
			}
			else if (bitmap.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
			{
				result = "JPG";
			}
			else if (bitmap.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Exif))
			{
				result = "EXIF";
			}
			else if (bitmap.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
			{
				result = "GIF";
			}
			else if (bitmap.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
			{
				result = "BMP";
			}

			return result;
		}

		private void InfoForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.Close();
			}
		}

		private void propertiesButton_Click(object sender, EventArgs e)
		{
			ShellManager.ShowFileProperties(fullPath);
		}

		private void propertiesButton_Paint(object sender, PaintEventArgs e)
		{
			if (darkMode)
			{
				Button btn = (Button)sender;

				if (!btn.Enabled)
				{
					btn.Text = string.Empty;
					TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;

					TextRenderer.DrawText(e.Graphics, "File properties", btn.Font, e.ClipRectangle, ThemeManager.SecondColorDark, flags);
				}
			}
		}

		private void copyNameButton_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(fileNameTextBox.Text);
		}

		private void copyFolderButton_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(folderTextBox.Text);
		}

		private void copyPathButton_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(fullPathTextBox.Text);
		}
	}
}
