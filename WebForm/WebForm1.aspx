<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebForm.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="ExternalFile/jquery-1.11.2.js"></script>
    <link href="ExternalFile/jquery-ui.css" rel="stylesheet" />

    
    <script src="ExternalFile/jquery-ui.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var outputSpan = $('#spanOutput');
            var sliderDiv = $('#slider');

            sliderDiv.slider({
                range: true,
                min: 18,
                max: 100,
                values: [20, 30],
                slide: function (event, ui) {
                    outputSpan.html(ui.values[0] + ' - ' + ui.values[1] + ' Years');
                },
                stop: function (event, ui) {
                    $('#txtMinAge').val(ui.values[0]);
                    $('#txtMaxAge').val(ui.values[1]);
                }
            });

            outputSpan.html(sliderDiv.slider('values', 0) + ' - '
                + sliderDiv.slider('values', 1) + ' Years');
            $('#txtMinAge').val(sliderDiv.slider('values', 0));
            $('#txtMaxAge').val(sliderDiv.slider('values', 1));
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Age : <span id="spanOutput"></span>
            <br />
            <br />
            <div id="slider"></div>
            <br />
            <label for="txtMinAge">Minimum Age</label>
            <input type="text" id="txtMinAge" />
            <br />
            <br />
            <label for="txtMaxAge">Maximum Age</label>
            <input type="text" id="txtMaxAge" />

        </div>
    </form>
</body>
</html>
