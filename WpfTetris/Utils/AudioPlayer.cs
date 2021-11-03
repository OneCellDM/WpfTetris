using ManagedBass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTetris.Utils
{
	public static class AudioPlayer
	{
		private static int _stream;

		static AudioPlayer()
		{
			Bass.Init();
		}

		public static void SetStreamFrommUrl(string url) =>
			_stream = Bass.CreateStream(url, 0, BassFlags.Default, null, IntPtr.Zero);

		public static void SetStreamFromPath(string path) =>
			_stream = Bass.CreateStream(path, 0, new FileInfo(path).Length, BassFlags.Default);

		public static void SetPositon(double val)
		{
			try
			{
				Bass.ChannelSetPosition(_stream, Bass.ChannelSeconds2Bytes(_stream, val), PositionFlags.Bytes);
			}
			catch (Exception)
			{
				return;
			}
		}

		public static void Update() => Bass.ChannelUpdate(_stream, 0);

		public static bool Play(bool repeat = false)
		{
			try
			{
				Stop();
				return Bass.ChannelPlay(_stream, repeat);
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool Stop()
		{
			try
			{
				Bass.ChannelStop(_stream);
				Bass.StreamFree(_stream);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool Pause()
		{
			try
			{
				return Bass.ChannelPause(_stream);
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static double GetVolume() => Bass.ChannelGetAttribute(_stream, ChannelAttribute.Volume);

		public static bool SetVolume(double volume) => Bass.ChannelSetAttribute(_stream, ChannelAttribute.Volume, volume);
	}
}