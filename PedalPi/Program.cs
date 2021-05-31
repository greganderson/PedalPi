using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PedalPi
{
	class Program
	{
		static void Main(string[] args)
		{
			const int port = 9050;
			byte[] data;

			// height == 1000
			// width == 1500

			// Landscape
			int xNextLandscape = 1500;
			int yNextLandscape = 800;
			int xPreviousLandscape = 0;
			int yPreviousLandscape = 800;

			// Portrait
			int xNextPortrait = 1000;
			int yNextPortrait = 1200;
			int xPreviousPortrait = 0;
			int yPreviousPortrait = 1200;

			const string nextPageMessage = "Next page :)";
			const string previousPageMessage = "Previous page :)";

			using (var sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
			{
				IPEndPoint iep = new IPEndPoint(IPAddress.Any, port);
				sock.Bind(iep);
				EndPoint ep = (EndPoint)iep;
				Console.WriteLine("Ready to receive...");

				while (true)
				{
					data = new byte[1024];
					int recv = sock.ReceiveFrom(data, ref ep);
					string stringData = Encoding.ASCII.GetString(data, 0, recv);
					if (stringData == nextPageMessage)
					{
						Console.WriteLine("received: {0}  from: {1}", stringData, ep.ToString());

						// Landscape
						LeftMouseClick(xNextLandscape, yNextLandscape);

						// Portrait
						//LeftMouseClick(xNextPortrait, yNextPortrait);
					}
					else if (stringData == previousPageMessage)
					{
						Console.WriteLine("received: {0}  from: {1}", stringData, ep.ToString());

						// Landscape
						LeftMouseClick(xPreviousLandscape, yPreviousLandscape);

						// Portrait
						//LeftMouseClick(xPreviousPortrait, yPreviousPortrait);
					}
				}
			}
		}

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		static extern bool SetCursorPos(int x, int y);

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

		public const int MOUSEEVENTF_LEFTDOWN = 0x02;
		public const int MOUSEEVENTF_LEFTUP = 0x04;

		public static void LeftMouseClick(int xpos, int ypos)
		{
			SetCursorPos(xpos, ypos);
			mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
			mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
		}
	}
}
