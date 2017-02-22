using System;
using System.Linq;

namespace TTSSample
{
	/// <summary>
	/// Gender of the voice.
	/// </summary>
	public enum Gender
	{
		Female,
		Male
	}

	/// <summary>
	/// Voice output formats.
	/// </summary>
	public enum AudioOutputFormat
	{
		/// <summary>
		/// raw-8khz-8bit-mono-mulaw request output audio format type.
		/// </summary>
		Raw8Khz8BitMonoMULaw,
		/// <summary>
		/// raw-16khz-16bit-mono-pcm request output audio format type.
		/// </summary>
		Raw16Khz16BitMonoPcm,
		/// <summary>
		/// riff-8khz-8bit-mono-mulaw request output audio format type.
		/// </summary>
		Riff8Khz8BitMonoMULaw,
		/// <summary>
		/// riff-16khz-16bit-mono-pcm request output audio format type.
		/// </summary>
		Riff16Khz16BitMonoPcm,
	}
	/// <summary>
	/// Generic event args
	/// </summary>
	/// <typeparam name="T">Any type T</typeparam>
	public class GenericEventArgs<T> : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GenericEventArgs{T}" /> class.
		/// </summary>
		/// <param name="eventData">The event data.</param>
		public GenericEventArgs(T eventData)
		{
			this.EventData = eventData;
		}

		/// <summary>
		/// Gets the event data.
		/// </summary>
		public T EventData { get; private set; }
	}
}
