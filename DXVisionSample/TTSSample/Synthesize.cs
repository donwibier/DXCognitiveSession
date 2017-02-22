﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TTSSample
{
	/// <summary>
	/// Sample synthesize request
	/// </summary>
	public class Synthesize
	{
		/// <summary>
		/// Generates SSML.
		/// </summary>
		/// <param name="locale">The locale.</param>
		/// <param name="gender">The gender.</param>
		/// <param name="name">The voice name.</param>
		/// <param name="text">The text input.</param>
		private string GenerateSsml(string locale, string gender, string name, string text)
		{
			var ssmlDoc = new XDocument(
					new XElement("speak",
				  new XAttribute("version", "1.0"),
				  new XAttribute(XNamespace.Xml + "lang", "en-US"),
				  new XElement("voice",
					new XAttribute(XNamespace.Xml + "lang", locale),
					new XAttribute(XNamespace.Xml + "gender", gender),
					new XAttribute("name", name),
					  text)));
			return ssmlDoc.ToString();
		}

		/// <summary>
		/// The input options
		/// </summary>
		private InputOptions inputOptions;

		/// <summary>
		/// Initializes a new instance of the <see cref="Synthesize"/> class.
		/// </summary>
		/// <param name="input">The input.</param>
		public Synthesize(InputOptions input)
		{
			this.inputOptions = input;
		}

		/// <summary>
		/// Called when a TTS request has been completed and audio is available.
		/// </summary>
		public event EventHandler<GenericEventArgs<Stream>> OnAudioAvailable;

		/// <summary>
		/// Called when an error has occured. e.g this could be an HTTP error.
		/// </summary>
		public event EventHandler<GenericEventArgs<Exception>> OnError;

		/// <summary>
		/// Sends the specified text to be spoken to the TTS service and saves the response audio to a file.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A Task</returns>
		public Task Speak(CancellationToken cancellationToken)
		{
			var cookieContainer = new CookieContainer();
			var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
			var client = new HttpClient(handler);

			foreach (var header in this.inputOptions.Headers)
			{
				client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
			}

			var genderValue = "";
			switch (this.inputOptions.VoiceType)
			{
				case Gender.Male:
					genderValue = "Male";
					break;
				case Gender.Female:
				default:
					genderValue = "Female";
					break;

			}

			var request = new HttpRequestMessage(HttpMethod.Post, this.inputOptions.RequestUri)
			{
				Content = new StringContent(GenerateSsml(this.inputOptions.Locale, genderValue, this.inputOptions.VoiceName, this.inputOptions.Text))
			};

			var httpTask = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
			Console.WriteLine("Response status code: [{0}]", httpTask.Result.StatusCode);

			var saveTask = httpTask.ContinueWith(
				async (responseMessage, token) =>
				{
					try
					{
						if (responseMessage.IsCompleted && responseMessage.Result != null && responseMessage.Result.IsSuccessStatusCode)
						{

							var httpStream = await responseMessage.Result.Content.ReadAsStreamAsync().ConfigureAwait(false);
							this.AudioAvailable(new GenericEventArgs<Stream>(httpStream));
						}
						else
						{
							this.Error(new GenericEventArgs<Exception>(new Exception(String.Format("Service returned {0}", responseMessage.Result.StatusCode))));
						}
					}
					catch (Exception e)
					{
						this.Error(new GenericEventArgs<Exception>(e.GetBaseException()));
					}
					finally
					{
						responseMessage.Dispose();
						request.Dispose();
						client.Dispose();
						handler.Dispose();
					}
				},
				TaskContinuationOptions.AttachedToParent,
				cancellationToken);

			return saveTask;
		}

		/// <summary>
		/// Called when a TTS requst has been successfully completed and audio is available.
		/// </summary>
		private void AudioAvailable(GenericEventArgs<Stream> e)
		{
			EventHandler<GenericEventArgs<Stream>> handler = this.OnAudioAvailable;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Error handler function
		/// </summary>
		/// <param name="e">The exception</param>
		private void Error(GenericEventArgs<Exception> e)
		{
			EventHandler<GenericEventArgs<Exception>> handler = this.OnError;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Inputs Options for the TTS Service.
		/// </summary>
		public class InputOptions
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="Input"/> class.
			/// </summary>
			public InputOptions()
			{
				this.Locale = "en-us";
				this.VoiceName = "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)";
				// Default to Riff16Khz16BitMonoPcm output format.
				this.OutputFormat = AudioOutputFormat.Riff16Khz16BitMonoPcm;
			}

			/// <summary>
			/// Gets or sets the request URI.
			/// </summary>
			public Uri RequestUri { get; set; }

			/// <summary>
			/// Gets or sets the audio output format.
			/// </summary>
			public AudioOutputFormat OutputFormat { get; set; }

			/// <summary>
			/// Gets or sets the headers.
			/// </summary>
			public IEnumerable<KeyValuePair<string, string>> Headers
			{
				get
				{
					List<KeyValuePair<string, string>> toReturn = new List<KeyValuePair<string, string>>();
					toReturn.Add(new KeyValuePair<string, string>("Content-Type", "application/ssml+xml"));

					string outputFormat;

					switch (this.OutputFormat)
					{
						case AudioOutputFormat.Raw16Khz16BitMonoPcm:
							outputFormat = "raw-16khz-16bit-mono-pcm";
							break;
						case AudioOutputFormat.Raw8Khz8BitMonoMULaw:
							outputFormat = "raw-8khz-8bit-mono-mulaw";
							break;
						case AudioOutputFormat.Riff16Khz16BitMonoPcm:
							outputFormat = "riff-16khz-16bit-mono-pcm";
							break;
						case AudioOutputFormat.Riff8Khz8BitMonoMULaw:
							outputFormat = "riff-8khz-8bit-mono-mulaw";
							break;
						default:
							outputFormat = "riff-16khz-16bit-mono-pcm";
							break;
					}

					toReturn.Add(new KeyValuePair<string, string>("X-Microsoft-OutputFormat", outputFormat));
					// authorization Header
					toReturn.Add(new KeyValuePair<string, string>("Authorization", this.AuthorizationToken));
					// Refer to the doc
					toReturn.Add(new KeyValuePair<string, string>("X-Search-AppId", "07D3234E49CE426DAA29772419F436CA"));
					// Refer to the doc
					toReturn.Add(new KeyValuePair<string, string>("X-Search-ClientID", "1ECFAE91408841A480F00935DC390960"));
					// The software originating the request
					toReturn.Add(new KeyValuePair<string, string>("User-Agent", "TTSClient"));

					return toReturn;
				}
				set
				{
					Headers = value;
				}
			}

			/// <summary>
			/// Gets or sets the locale.
			/// </summary>
			public String Locale { get; set; }

			/// <summary>
			/// Gets or sets the type of the voice; male/female.
			/// </summary>
			public Gender VoiceType { get; set; }

			/// <summary>
			/// Gets or sets the name of the voice.
			/// </summary>
			public string VoiceName { get; set; }

			/// <summary>
			/// Authorization Token.
			/// </summary>
			public string AuthorizationToken { get; set; }

			/// <summary>
			/// Gets or sets the text.
			/// </summary>
			public string Text { get; set; }
		}
	}
}
