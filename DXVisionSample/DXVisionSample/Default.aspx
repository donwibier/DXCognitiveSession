<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DXVisionSample.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
	<div>
		<h2>Cognitive Vision Sample</h2>
		<dx:ASPxUploadControl ID="ASPxUploadControl1" runat="server" NullText="Upload a file to analyze" UploadMode="Auto" Width="280px" OnFilesUploadComplete="ASPxUploadControl1_FilesUploadComplete" ShowUploadButton="True">
			<AdvancedModeSettings EnableDragAndDrop="True">
			</AdvancedModeSettings>
			<ClientSideEvents FilesUploadComplete="function(s, e) { 
                mmo.SetValue(e.callbackData);
                //resultsLabel.SetText(e.callbackData); 
                
             }" />
		</dx:ASPxUploadControl>
		<div style="clear: both"></div>
		<div>
			<dx:ASPxLabel runat="server" ID="results" ClientInstanceName="resultsLabel" Text="Results"></dx:ASPxLabel>
		</div>
		<div style="clear: both"></div>
		<div>
			<dx:ASPxMemo runat="server" ID="mmoResults" ClientInstanceName="mmo" Height="478px" ReadOnly="True" Width="860px" 
                EnableClientSideAPI="true"></dx:ASPxMemo>
		</div>
        
	</div>
	</form>
</body>
</html>
