﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Speech.aspx.cs" Inherits="DXVisionSample.Speech" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
</head>
<body>
    <form id="form1" runat="server">
        <table width="100%">
            <tr>
                <td></td>
                <td>
                    <h1 style="font-weight: 500;">Speech Recognition</h1>
                    <h2 style="font-weight: 500;">Microsoft Cognitive Services</h2>
                </td>
            </tr>
            <tr>
                <td align="right"><a href="https://www.microsoft.com/cognitive-services/en-us/sign-up" target="_blank">Subscription</a>:</td>
                <td>
                    <input id="key" type="text" size="40" value="YOUR_BING_SPEECH_API_KEY"></td>
            </tr>
            <tr>
                <td align="right">Language:</td>
                <td align="left">
                    <select id="languageOptions">
                        <option value="zh-CN">Chinese - CN</option>
                        <option value="en-GB">English - GB</option>
                        <option value="en-US" selected="selected">English - US</option>
                        <option value="fr-FR">French - FR</option>
                        <option value="de-DE">German - DE</option>
                        <option value="it-IT">Italian - IT</option>
                        <option value="es-ES">Spanish - ES</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td align="right">Recognition Mode:</td>
                <td align="left">
                    <select id="recognitionMode">
                        <option value="Interactive">Interactive</option>
                        <option value="Conversation">Conversation</option>
                        <option value="Dictation">Dictation</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td align="right">Format:</td>
                <td align="left">
                    <select id="formatOptions">
                        <option value="Simple" selected="selected">Simple Result</option>
                        <option value="Detailed">Detailed Result</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td align="right">Input:</td>
                <td align="left">
                    <select id="inputSource">
                        <option value="Mic" selected="selected">Microphone</option>
                        <option value="File">Audio File</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <button id="startBtn" disabled="disabled">Start</button>
                    <button id="stopBtn" disabled="disabled">Stop</button>
                    <input type="file" id="filePicker" accept=".wav" style="display: none" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <table>
                        <tr>
                            <td>Results:</td>
                            <td>Current hypothesis:</td>
                        </tr>
                        <tr>
                            <td>
                                <textarea id="phraseDiv" style="display: inline-block; width: 500px; height: 200px"></textarea>
                            </td>
                            <td style="vertical-align: top">
                                <span id="hypothesisDiv" style="width: 200px; height: 200px; display: block;"></span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right">Status:</td>
                <td align="left"><span id="statusDiv"></span></td>
            </tr>
            <tr>
                <td align="right">Luis response:</td>
                <td>
                    <textarea id="luisDiv" style="display: inline-block; width: 500px; height: 200px"></textarea>
                </td>
            </tr>
        </table>
        <dx:ASPxCallback ID="cbxLuis" runat="server" ClientInstanceName="luis"
            OnCallback="cbxLuis_Callback">
            <ClientSideEvents 
                CallbackComplete="function(s, e) { document.getElementById('luisDiv').innerHTML = e.result; }" />
        </dx:ASPxCallback>
        <!-- The SDK has a dependency on requirejs (http://requirejs.org/). -->
        <script src="https://cdnjs.cloudflare.com/ajax/libs/require.js/2.3.3/require.min.js"></script>

        <!-- SDK REFERENCE -->
        <script src="Scripts/speech.browser.sdk.js"></script>

        <!-- SDK USAGE -->
        <script>
            // On document load resolve the SDK dependency
            function Initialize(onComplete) {
                require(["Speech.Browser.Sdk"], function (SDK) {
                    onComplete(SDK);
                });
            }

            // Setup the recognizer
            function RecognizerSetup(SDK, recognitionMode, language, format, subscriptionKey) {

                switch (recognitionMode) {
                    case "Interactive":
                        recognitionMode = SDK.RecognitionMode.Interactive;
                        break;
                    case "Conversation":
                        recognitionMode = SDK.RecognitionMode.Conversation;
                        break;
                    case "Dictation":
                        recognitionMode = SDK.RecognitionMode.Dictation;
                        break;
                    default:
                        recognitionMode = SDK.RecognitionMode.Interactive;
                }

                var recognizerConfig = new SDK.RecognizerConfig(
                    new SDK.SpeechConfig(
                        new SDK.Context(
                            new SDK.OS(navigator.userAgent, "Browser", null),
                            new SDK.Device("SpeechSample", "SpeechSample", "1.0.00000"))),
                    recognitionMode,
                    language, // Supported languages are specific to each recognition mode. Refer to docs.
                    format); // SDK.SpeechResultFormat.Simple (Options - Simple/Detailed)

                // Alternatively use SDK.CognitiveTokenAuthentication(fetchCallback, fetchOnExpiryCallback) for token auth
                var authentication = new SDK.CognitiveSubscriptionKeyAuthentication(subscriptionKey);

                var files = document.getElementById('filePicker').files;
                if (!files.length) {
                    return SDK.CreateRecognizer(recognizerConfig, authentication);
                } else {
                    return SDK.CreateRecognizerWithFileAudioSource(recognizerConfig, authentication, files[0]);
                }
            }

            // Start the recognition
            function RecognizerStart(SDK, recognizer) {
                recognizer.Recognize((event) => {
                    /*
                     Alternative syntax for typescript devs.
                     if (event instanceof SDK.RecognitionTriggeredEvent)
                    */
                    switch (event.Name) {
                        case "RecognitionTriggeredEvent":
                            UpdateStatus("Initializing");
                            break;
                        case "ListeningStartedEvent":
                            UpdateStatus("Listening");
                            break;
                        case "RecognitionStartedEvent":
                            UpdateStatus("Listening_Recognizing");
                            break;
                        case "SpeechStartDetectedEvent":
                            UpdateStatus("Listening_DetectedSpeech_Recognizing");
                            console.log(JSON.stringify(event.Result)); // check console for other information in result
                            break;
                        case "SpeechHypothesisEvent":
                            UpdateRecognizedHypothesis(event.Result.Text, false);
                            console.log(JSON.stringify(event.Result)); // check console for other information in result
                            break;
                        case "SpeechFragmentEvent":
                            UpdateRecognizedHypothesis(event.Result.Text, true);
                            console.log(JSON.stringify(event.Result)); // check console for other information in result
                            break;
                        case "SpeechEndDetectedEvent":
                            OnSpeechEndDetected();
                            UpdateStatus("Processing_Adding_Final_Touches");
                            console.log(JSON.stringify(event.Result)); // check console for other information in result
                            break;
                        case "SpeechSimplePhraseEvent":
                            UpdateRecognizedPhrase(JSON.stringify(event.Result, null, 3));
                            break;
                        case "SpeechDetailedPhraseEvent":
                            UpdateRecognizedPhrase(JSON.stringify(event.Result, null, 3));
                            break;
                        case "RecognitionEndedEvent":
                            OnComplete();
                            UpdateStatus("Idle");
                            console.log(JSON.stringify(event)); // Debug information
                            break;
                        default:
                            console.log(JSON.stringify(event)); // Debug information
                    }
                })
                    .On(() => {
                        // The request succeeded. Nothing to do here.
                    },
                    (error) => {
                        console.error(error);
                    });
            }

            // Stop the Recognition.
            function RecognizerStop(SDK, recognizer) {
                // recognizer.AudioSource.Detach(audioNodeId) can be also used here. (audioNodeId is part of ListeningStartedEvent)
                recognizer.AudioSource.TurnOff();
            }
        </script>

        <!-- Browser Hooks -->
        <script>
            var startBtn, stopBtn, hypothesisDiv, phraseDiv, statusDiv;
            var key, languageOptions, formatOptions, recognitionMode, inputSource, filePicker;
            var SDK;
            var recognizer;
            var previousSubscriptionKey;

            document.addEventListener("DOMContentLoaded", function () {
                createBtn = document.getElementById("createBtn");
                startBtn = document.getElementById("startBtn");
                stopBtn = document.getElementById("stopBtn");
                phraseDiv = document.getElementById("phraseDiv");
                hypothesisDiv = document.getElementById("hypothesisDiv");
                statusDiv = document.getElementById("statusDiv");
                key = document.getElementById("key");
                languageOptions = document.getElementById("languageOptions");
                formatOptions = document.getElementById("formatOptions");
                inputSource = document.getElementById("inputSource");
                recognitionMode = document.getElementById("recognitionMode");
                filePicker = document.getElementById('filePicker');

                languageOptions.addEventListener("change", Setup);
                formatOptions.addEventListener("change", Setup);
                recognitionMode.addEventListener("change", Setup);

                startBtn.addEventListener("click", function () {
                    if (key.value == "" || key.value == "YOUR_BING_SPEECH_API_KEY") {
                        alert("Please enter your Bing Speech subscription key!");
                        return;
                    }
                    if (inputSource.value === "File") {
                        filePicker.click();
                    } else {
                        if (!recognizer || previousSubscriptionKey != key.value) {
                            previousSubscriptionKey = key.value;
                            Setup();
                        }

                        hypothesisDiv.innerHTML = "";
                        phraseDiv.innerHTML = "";
                        RecognizerStart(SDK, recognizer);
                        startBtn.disabled = true;
                        stopBtn.disabled = false;
                    }
                });

                key.addEventListener("focus", function () {
                    if (key.value == "YOUR_BING_SPEECH_API_KEY") {
                        key.value = "";
                    }
                });

                key.addEventListener("focusout", function () {
                    if (key.value == "") {
                        key.value = "YOUR_BING_SPEECH_API_KEY";
                    }
                });

                filePicker.addEventListener("change", function () {
                    Setup();
                    hypothesisDiv.innerHTML = "";
                    phraseDiv.innerHTML = "";
                    RecognizerStart(SDK, recognizer);
                    startBtn.disabled = true;
                    stopBtn.disabled = false;
                    filePicker.value = "";
                });

                stopBtn.addEventListener("click", function () {
                    RecognizerStop(SDK, recognizer);
                    startBtn.disabled = false;
                    stopBtn.disabled = true;
                });

                Initialize(function (speechSdk) {
                    SDK = speechSdk;
                    startBtn.disabled = false;
                });
            });

            function Setup() {
                recognizer = RecognizerSetup(SDK, recognitionMode.value, languageOptions.value, SDK.SpeechResultFormat[formatOptions.value], key.value);
            }

            function UpdateStatus(status) {
                statusDiv.innerHTML = status;
            }

            function UpdateRecognizedHypothesis(text, append) {
                if (append)
                    hypothesisDiv.innerHTML += text + " ";
                else
                    hypothesisDiv.innerHTML = text;

                var length = hypothesisDiv.innerHTML.length;
                if (length > 403) {
                    hypothesisDiv.innerHTML = "..." + hypothesisDiv.innerHTML.substr(length - 400, length);
                }
            }

            function OnSpeechEndDetected() {
                stopBtn.disabled = true;
            }

            function UpdateRecognizedPhrase(json) {
                hypothesisDiv.innerHTML = "";
                phraseDiv.innerHTML += json + "\n";
            }

            function OnComplete() {
                var r = JSON.parse(phraseDiv.innerHTML);
                luis.PerformCallback(r.DisplayText);
                startBtn.disabled = false;
                stopBtn.disabled = true;
            }
        </script>
    </form>
</body>
</html>
