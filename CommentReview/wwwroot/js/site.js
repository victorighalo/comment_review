// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// front end functionality for YouTube Comments Scrapping
$(document).ready(function () {
    let count = 0;
    let totalComment = 0;
    let commentsArray = [];
    let youtubeLink;


    //Get Youtube comments wiht link from server
    function GetComments(token = "", link) {
        $.ajax(
            {
                url: 'http://localhost:51965/youtubecommentsjson/'+link+"/"+token
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
                download('data:text/plain,'+data+"", "comments.csv", "text/csv");
            })
            .fail(function (error) {
                $("#indicator").text("Failed!");
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
            GetComments(jsonData.nextPageToken, youtubeLink);
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
        var link = $("#youtubelink").val();
        youtubeLink = youtube_parser(link)
        if (link === false) {
            alert("Enter a valid Youtube link");
            return false;
        }
        GetComments("", youtubeLink);
    })

    $("#downloadCommentsAction").click(function (event) {
        event.preventDefault();
        $("#indicator").fadeOut();
        PostComments();
    })

    //Credit Aaron Thoma
    //Source: https://stackoverflow.com/questions/3452546/how-do-i-get-the-youtube-video-id-from-a-url
    //Youtube URL Parser
    function youtube_parser(url) {
        var regExp = /^.*((youtu.be\/)|(v\/)|(\/u\/\w\/)|(embed\/)|(watch\?))\??v?=?([^#\&\?]*).*/;
        var match = url.match(regExp);
        return (match && match[7].length === 11) ? match[7] : false;
    }
})