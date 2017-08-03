using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using payloader.Properties;

namespace payloader
{
    public partial class payloader : Form {
        /// <summary>
        /// Variable to store the IP address internaly.
        /// </summary>
        private IPAddress ip;

        /// <summary>
        /// Variable to store the Port address internaly.
        /// </summary>
        private int port;

        /// <summary>
        /// Determine if we have a correct input for the Path string.
        /// </summary>
        private bool correctInput;

        /// <summary>
        /// Ussed to store the last valid path input.
        /// </summary>
        private string backup = string.Empty;

        /// <summary>
        /// Represents the Button Ok.
        /// </summary>
        private MessageBoxButtons ok = MessageBoxButtons.OK;

        /// <summary>
        /// Represents the Icon error.
        /// </summary>
        private MessageBoxIcon err = MessageBoxIcon.Error;

        /// <summary>
        /// A settings accessor.
        /// </summary>
        private Settings set = new Settings();


        /// <summary>
        /// Initializer.
        /// </summary>
        public payloader() {
            InitializeComponent();
            openPayload.Title = "Payload Öffnen";
        }

        /// <summary>
        /// Open a payload file.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void buttonOpen_Click(object sender, EventArgs e) {
            if (set.path != string.Empty) openPayload.InitialDirectory = set.path;
            else openPayload.InitialDirectory = Directory.GetCurrentDirectory();

            if (openPayload.ShowDialog() == DialogResult.OK) {
                correctInput = true;
                textBoxPath.Text = openPayload.FileName;
                set.path = textBoxPath.Text;
                set.Save();
                set.Reload();
            }
        }

        /// <summary>
        /// Send the payload to a server using the IP and Port.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void buttonSend_Click(object sender, EventArgs e) {
            Application.DoEvents();     // Ensure that user can cancel the process if needed.
            if (!IPAddress.TryParse(textBoxIP.Text, out ip)) MessageBox.Show("That's not a valid IP Address.", "IP Missmatch", ok, err);
            else if (!int.TryParse(textBoxPort.Text, out port)) MessageBox.Show("That's not a valid Port Address.", "Port Missmatch", ok, err);
            else if (port < 1 || port > 65535) MessageBox.Show("Port number is out of Range.", "Out of Range", ok, err);
            else if (!magic.ConnectAndSend(ip, port, textBoxPath.Text)) MessageBox.Show("Could not establish connection to " + textBoxIP.Text + " on port " + textBoxPort.Text + ".", "Connection Error", ok, err);
            else MessageBox.Show("Done.");
        }

        /// <summary>
        /// Check if Path would be either selected or a file dropped.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void textBoxPath_TextChanged(object sender, EventArgs e) {
            if (correctInput) {
                backup = textBoxPath.Text;
                correctInput = false;
            } else textBoxPath.Text = backup;
        }

        /// <summary>
        /// Is Drop valid ?
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void textBoxPath_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false)) e.Effect = DragDropEffects.Copy;
            else e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Check the droped file before to overload it into the path field.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void textBoxPath_DragDrop(object sender, DragEventArgs e) {
            string[] check = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (check.Length > 1) MessageBox.Show("Please only drop one file into the Gui.", "Multi Drop", ok, err);
            else if (!check.Contains(".bin") || !check.Contains(".elf")) MessageBox.Show("Please use .bin files.", "Wrong Format", ok, err);
            else {
                textBoxPath.Text = check[0];
                correctInput = true;
            }
        }        

        /// <summary>
        /// Close the Form, cancel the connection or sending process if needed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void payloader_FormClosing(object sender, FormClosingEventArgs e) { magic.CancelConnect(); }
    }
}
