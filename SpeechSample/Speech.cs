using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;

namespace SpeechSample
{
	public class SpeechResult<TResult>
	{
		public SpeechResult(bool success, TResult result, CancellationErrorCode errorCode = CancellationErrorCode.NoError, string errorDetails = "")
		{
			ErrorDetails = errorDetails;
			ErrorCode = errorCode;
			Success = success;
			Result = result;
		}
		public bool Success { get; set; }
		public TResult Result { get; set; }
		public CancellationErrorCode ErrorCode { get; set;}
		public string ErrorDetails { get; set; }

	}

	public static class Speech
	{
		public static async Task<SpeechResult<string>> Listen(SpeechConfig config)
		{
			// Creates a speech recognizer.
			using (var recognizer = new SpeechRecognizer(config))
			{
				var result = await recognizer.RecognizeOnceAsync();
				//var result = await recognizer.StartContinuousRecognitionAsync();

				// Checks result.
				if (result.Reason == ResultReason.RecognizedSpeech)
				{
					return new SpeechResult<string>(true, result.Text);
				}
				else if (result.Reason == ResultReason.NoMatch)
				{
					return new SpeechResult<string>(false, "I don't understand what you're saying.");
				}
				else if (result.Reason == ResultReason.Canceled)
				{
					var cancellation = CancellationDetails.FromResult(result);
					return new SpeechResult<string>(false, "The operation was cancelled. Is you key OK?", cancellation.ErrorCode, cancellation.ErrorDetails);
				}
			}
			return new SpeechResult<string>(false, "Something weird happened");
		}

		public static async Task<SpeechResult<string>> Speak(SpeechConfig config, string text)
		{
			// Creates a speech synthesizer using the default speaker as audio output.
			using (var synthesizer = new SpeechSynthesizer(config))
			{
				using (var result = await synthesizer.SpeakTextAsync(text))
				{
					if (result.Reason == ResultReason.SynthesizingAudioCompleted)
					{
						return new SpeechResult<string>(true, "Speech synthesized to speaker");
					}
					else if (result.Reason == ResultReason.Canceled)
					{
						var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
						var r = new SpeechResult<string>(false, $"CANCELED: Reason={cancellation.Reason}");

						if (cancellation.Reason == CancellationReason.Error)
						{
							r.ErrorCode = cancellation.ErrorCode;
							r.ErrorDetails = cancellation.ErrorDetails;
						}
						return r;
					}
				}
			}
			return new SpeechResult<string>(false, "Something weird happened");
		}
	}
}
