using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentReview.Models
{
    public class YoutubeViewModel
    {

        public class Rootobject
        {
            public string nextPageToken { get; set; }
            public Pageinfo pageInfo { get; set; }
            public Item[] items { get; set; }
        }

        public class Pageinfo
        {
            public int totalResults { get; set; }
            public int resultsPerPage { get; set; }
        }

        public class Item
        {
            public Snippet snippet { get; set; }
        }

        public class Snippet
        {
            public string videoId { get; set; }
            public Toplevelcomment topLevelComment { get; set; }
        }

        public class Toplevelcomment
        {
            public Snippet1 snippet { get; set; }
        }

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


}
