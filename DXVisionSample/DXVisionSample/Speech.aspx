<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Speech.aspx.cs" Inherits="DXVisionSample.Speech" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script src="speech.1.0.0.js" type="text/javascript"></script>
	<script type="text/javascript">
		var client;
		var request;

		function getKey() {
			return "f55a5268279b448c9eaf7b96b692887a";
		}

		function getLanguage() {
			return "en-us";
		}

		function clearText() {
			document.getElementById("output").value = "";
		}

		function setText(text) {
			document.getElementById("output").value += text;
		}

		function start() {
			var mode = Microsoft.CognitiveServices.SpeechRecognition.SpeechRecognitionMode.shortPhrase;

			clearText();

				
			client = Microsoft.CognitiveServices.SpeechRecognition.SpeechRecognitionServiceFactory.createMicrophoneClient(
					mode,
					getLanguage(),
					getKey()
			);
				
			client.startMicAndRecognition();

			setTimeout(function () {
				client.endMicAndRecognition();
			}, 5000);

			client.onPartialResponseReceived = function (response) {
				setText(response);
			}

			client.onFinalResponseReceived = function (response) {
				setText(JSON.stringify(response));
			}

			client.onIntentReceived = function (response) {
				setText(response);
			};
		}

	</script>
</head>
<body>
	<form id="form1" runat="server">
		 <table width="100%">
			<tr><td><h1>Speech.JS</h1><h2>Microsoft Cognitive Services</h2></td></tr>
			<tr>				
				<td><button type="button" onclick="start()">Start</button></td>
			</tr>
			<tr>
				<td>
					<textarea id="output" style='width:400px;height:200px'></textarea>
				</td>
			</tr>
			
		</table>  

	</form>
</body>
</html>
