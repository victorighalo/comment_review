using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CommentReview.Models.CommentsViewModel
{
    [JsonObject(MemberSerialization.OptOut)]
    public class CommentsViewModel
    {
        public Snippet Snippet { get; set; }
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class Snippet
    {
        public string VideoId { get; set; }
        public Toplevelcomment TopLevelComment { get; set; }
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class Toplevelcomment
    {
        public Snippet1 Snippet { get; set; }
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class Snippet1
    {
        public string authorDisplayName { get; set; }
        public string videoId { get; set; }
        public string textDisplay { get; set; }
        public string textOriginal { get; set; }
        public string viewerRating { get; set; }
        public DateTime publishedAt { get; set; }
    }

}
