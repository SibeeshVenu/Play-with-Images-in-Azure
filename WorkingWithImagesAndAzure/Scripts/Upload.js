var startDateTime;
var mediaServiceAccount = 'picsiimediaservice';
$(document).ready(function () {
    function formatSizeUnits(bytes) {
        if (bytes >= 1000000000) { bytes = (bytes / 1000000000).toFixed(2) + ' GB'; }
        else if (bytes >= 1000000) { bytes = (bytes / 1000000).toFixed(2) + ' MB'; }
        else if (bytes >= 1000) { bytes = (bytes / 1000).toFixed(2) + ' KB'; }
        else if (bytes > 1) { bytes = bytes + ' bytes'; }
        else if (bytes == 1) { bytes = bytes + ' byte'; }
        else { bytes = '0 byte'; }
        return bytes;
    }
    function findDateDiff(date1, date2) {
        //Get 1 day in milliseconds
        var one_day = 1000 * 60 * 60 * 24;

        // Convert both dates to milliseconds
        var date1_ms = date1.getTime();
        var date2_ms = date2.getTime();

        // Calculate the difference in milliseconds
        var difference_ms = date2_ms - date1_ms;
        //take out milliseconds
        difference_ms = difference_ms / 1000;
        var seconds = Math.floor(difference_ms % 60);
        difference_ms = difference_ms / 60;
        var minutes = Math.floor(difference_ms % 60);
        difference_ms = difference_ms / 60;

        //var hours = Math.floor(difference_ms % 24);
        //var days = Math.floor(difference_ms / 24);

        return minutes + ' minute (s), and ' + seconds + ' second (s)';
    };
    $('#btnSubmit').click(function () {
        $('#fiesInfo').html('');
        $('#divOutput').html('');
        startDateTime = new Date();
        $('#fiesInfo').append('<br/><br/><span><b>Uploading starts at</b></span>' + startDateTime);
        var data = new FormData();
        var files = $("#myFile").get(0).files;
        if (files.length > 0) {
            for (var i = 0; i < files.length; i++) {
                data.append("UploadedImage_" + i, files[i]);
            }
            var ajaxRequest = $.ajax({
                type: "POST",
                url: "http://localhost:5022/Home/UploadFile",
                contentType: false,
                processData: false,
                data: data,
                cache: false,
                success: function (data, status) {
                    debugger;
                    var totSize = 0;
                    $('#divOutput').hide();
                    $('#fiesInfo').append('<table></table>');
                    for (var i = 0; i < data.length; i++) {
                        totSize = totSize + parseFloat(data[i].ImageSize);
                        $('#divOutput').append('<img style="float: left;padding:10px;margin:5px;"  src=https://' + mediaServiceAccount + '.blob.core.windows.net/' + data[i].AssetID + '/' + data[i].Title + ' />');
                    }
                    $('#fiesInfo table').append('<tr><td><b>No of files uploaded: </b></td><td>' + data.length + '</td></tr>');
                    $('#fiesInfo table').append('<tr><td><b>Total size uploaded: </b></td><td>' + formatSizeUnits(totSize) + '</td></tr>');
                    var endDateTime = new Date();
                    $('#fiesInfo table').append('<tr><td><b>Uploading ends at </b></td><td>' + endDateTime + '</td></tr>');
                    $('#fiesInfo table').append('<tr><td><b>The time taken is </b></td><td>' + findDateDiff(startDateTime, endDateTime) + ' </td></tr>');
                    $('#divOutput').show();
                },
                error: function (xhr, desc, err) {
                    $('#divOutput').html('Error: ' + err);
                }
            });
        }

    });
});