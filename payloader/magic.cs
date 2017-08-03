using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace payloader
{
    /// <summary>
    /// String extansion.
    /// </summary>
    public static class String {
        /// <summary>
        /// Checks if a string do contain a specific string including StringCoparison option.
        /// </summary>
        /// <param name="source">The source string to check.</param>
        /// <param name="toCheck">The string that shall be looked for.</param>
        /// <param name="comparison">String Comparison options.</param>
        /// <returns>True if the string to look for was found., else false.</returns>
        public static bool Contains(this string source, string toCheck, StringComparison comparison) { return source.IndexOf(toCheck, comparison) >= 0; }
    }

    public class magic {
        /// <summary>
        /// Determine if the connection process is still running.
        /// </summary>
        private static bool isRunning = false;

        /// <summary>
        /// Store the IP Address.
        /// </summary>
        public static IPAddress _ip;

        /// <summary>
        /// Store the Port.
        /// </summary>
        public static int _port;

        /// <summary>
        /// Store the path.
        /// </summary>
        public static string _path = string.Empty;

        /// <summary>
        /// StringComparison to ignore Cases for our modded string extansions.
        /// </summary>
        private static StringComparison ignoreCase = StringComparison.CurrentCultureIgnoreCase;

        /// <summary>
        /// The TCP Client to send the payload and to connect to the server.
        /// </summary>
        private static TcpClient client = null;

        /// <summary>
        /// A network stream.
        /// </summary>
        private static NetworkStream network = null;



        /// <summary>
        /// Connect o given IP and Port and send the payload.
        /// </summary>
        /// <param name="ip">The IP to connect to.</param>
        /// <param name="Port">The Port to use.</param>
        /// <param name="path">The path to the payload to send.</param>
        /// <returns>true when payload could be sended, else false</returns>
        public static bool ConnectAndSend(IPAddress ip, int port, string path) {
            isRunning = true;
            FileInfo fi = new FileInfo(path);
            byte[] data = new byte[fi.Length];
            using (BinaryReader br = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read))) {
                data = br.ReadBytes((int)fi.Length);
                br.Close();
            }

            client = new TcpClient(ip.ToString(), port);
            network = client.GetStream();
            network.Write(data, 0, (int)fi.Length);
            network.Close();
            client.Close();
            isRunning = false;
            return true;
        }

        /// <summary>
        /// Cancel the connection and sending process if needed.
        /// </summary>
        public static void CancelConnect() {
            if (isRunning) {
                network.Close();
                client.Close();
                isRunning = false;
            }
        }

        /// <summary>
        /// Print the Usage.
        /// </summary>
        public static void PrintUsage() {
            Console.WriteLine("payloader IP Port path_to_your_payload\nor\npayloader Port IP path_to_your_payload\n\n..simple eye !?\n\n");
            Console.ReadLine();
        }

        /// <summary>
        /// Print a nice Welcome.
        /// </summary>
        public static void PrintWelcome() { Console.WriteLine("Welcome to simple payloader !\n@cfwprpht 2017\n\n"); }

        /// <summary>
        /// Print a byby.
        /// </summary>
        public static void PrintByBy() {
            Console.WriteLine("\nthx for using this tool !\n\n");
            Console.ReadLine();
        }

        /// <summary>
        /// Print a error.
        /// </summary>
        public static void PrintMeh() { Console.WriteLine("Could not send the payload. :(\n"); }

        /// <summary>
        /// Check arguments.
        /// </summary>
        /// <param name="args">The arguments to check.</param>
        /// <returns>true when arguments are ok, else false.</returns>
        public static bool CheckArgs(string[] args) {
            bool ip_, port_;
            ip_ = port_ = false;
            _ip = null;
            _port = 0;
            _path = string.Empty;

            if (!IPAddress.TryParse(args[0], out _ip)) {
                if (IPAddress.TryParse(args[1], out _ip)) ip_ = true;
                else Console.WriteLine("Could not parse IP Address !\nPlease use format '122.555.999.777'\n");
            } else ip_ = true;

            if (!int.TryParse(args[1], out _port)) {
                if (int.TryParse(args[0], out _port)) port_ = true;
                else Console.WriteLine("Could not parse Port !\n");
            } else port_ = true;

            if (ip_ && port_) {
                if (args[2].Contains(".bin", ignoreCase) || args[2].Contains(".elf", ignoreCase)) {
                    if (File.Exists(args[2])) _path = args[2];
                    else {
                        Console.WriteLine("File does not exists or is not access able !\n");
                        Console.ReadLine();
                        return false;
                    }
                }
                else {
                    Console.WriteLine("Wrong file format !\nPlease use .bin or .elf files as input.\n");
                    Console.ReadLine();
                    return false;
                }
            } else {
                Console.ReadLine();
                return false;
            }
            return true;
        }
    }
}
