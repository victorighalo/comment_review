// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// front end functionality for YouTube Comments Scrapping
$(document).ready(function () {
    let count = 0;
    let totalComment = 0;
    let commentsArray = [];
    let youtubeLink;
    let baseUrl = window.location.origin;

    //Get Youtube comments wiht link from server
    function GetComments(token = "", link) {
        $.ajax(
            {
                url: baseUrl+"/youtubecommentsjson/"+link+"/"+token
            }).
            done(function (data) {
                ProcessComments(data);
            }).
            fail(function (e) {
                console.log(e)
                alert("An Error Occured.");
                $("#indicator").text("Failed!");
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
                url: baseUrl+'/export/',
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
        var link = $("#youtubelink").val();
        youtubeLink = youtube_parser(link)
        if (link.replace(/\s/g, "") == "") {
            alert("Please enter a YouTube Video Link");
            return false;
        }
        if (youtubeLink === false) {
            alert("Enter a valid Youtube Video link");
            return false;
        }
        $("#downloadCommentsAction").fadeOut();
        $("#indicator").fadeIn();
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



    //Amazon

    //Get Youtube comments wiht link from server
    function GetAmazonReviews(link) {
        var data = {"name":"link", "content": link}
        $.ajax(
            {
                method: 'POST',
                contentType: 'application/json; charset=utf-8',
                dateType: 'json',
                processData: false,
                url: baseUrl+'/amazonreviewsjson/',
                data: JSON.stringify(data)
            }).
            done(function (data) {
                PostReviews(data)
            }).
            fail(function () {
                $("#indicator").text("Failed!");
                alert("An Error Occured. Check the link.")
            })
    }

    function PostReviews(data) {
        $.ajax(
            {
                method: 'POST',
                contentType: 'application/json; charset=utf-8',
                dateType: 'json',
                processData: false,
                url: baseUrl+'/exportreviews/',
                data: JSON.stringify(data)
            }).
            done(function (data) {
                download('data:text/plain,' + data + "", "comments.csv", "text/csv");
                $("#indicator").fadeOut();
            })
            .fail(function (error) {
                $("#indicator").text("Failed!");
            })
    }

    $("#amazon_extract_action").click(function (event) {
        event.preventDefault();
        var link = $("#amazonlink").val();
        if (link.replace(/\s/g, "") == "") {
            alert("Enter a valid Amazon link");
            return false;
        }
        $("#downloadReviewsAction").fadeOut();
        $("#indicator").fadeIn();
        GetAmazonReviews(link);
    })

})