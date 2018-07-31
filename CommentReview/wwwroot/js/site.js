// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    let count = 0;
    let totalComment = 0;
    let commentsArray = [];

    //Ajax Operations
    //Get Youtube comments wiht link from server
    function GetComments(token = "") {
        $.ajax(
            {
                url: 'http://localhost:51965/youtubecommentsjson/'+token
            }).
            done(function (data) {
                ProcessComments(data);
            })
    }

    //Send Comments Json to Server for Conversion
    function PostComments() {
        $.ajax(
            {
                method: 'POST',
                contentType: 'application/json; charset=utf-8',
                dateType: 'json',
                processData:false,
                url: 'http://localhost:51965/export/',
                data: JSON.stringify(commentsArray)
            }).
            done(function (data) {
                console.info(data);
                download('data:text/plain,'+data+"", "comments.csv", "text/csv");
            })
            .fail(function (error) {
                $("#indicator").text("Failed!");
                console.log(error)
            })
    }

    function ProcessComments(data) {
        
        let token = "";
        let jsonData = JSON.parse(data);
        if (jsonData.nextPageToken || count === 0) {
            count++;
            $.each(jsonData.items, function (index, data) {
                commentsArray.push(data); 
            })
            console.log(commentsArray)
            GetComments(jsonData.nextPageToken);
        }
        else {
            $("#indicator").fadeOut();
            $("#downloadCommentsAction").fadeIn();
            }
        
    }
    
    $("#extract_action").click(function (event) {
        event.preventDefault();
        $("#downloadCommentsAction").fadeOut();
        $("#indicator").fadeIn();
        GetComments();
    })

    $("#downloadCommentsAction").click(function (event) {
        event.preventDefault();
        $("#indicator").fadeOut();
        PostComments();
    })
})