using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CommentReview.Models
{
    public partial class YoutubeComments
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("nextPageToken")]
        public string NextPageToken { get; set; }

        [JsonProperty("pageInfo")]
        public PageInfo PageInfo { get; set; }

        [JsonProperty("items")]
        public Item[] Items { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("kind")]
        public ItemKind Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("snippet")]
        public ItemSnippet Snippet { get; set; }

        [JsonProperty("replies", NullValueHandling = NullValueHandling.Ignore)]
        public Replies Replies { get; set; }
    }

    public partial class Replies
    {
        [JsonProperty("comments")]
        public Comment[] Comments { get; set; }
    }

    public partial class Comment
    {
        [JsonProperty("kind")]
        public TopLevelCommentKind Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("snippet")]
        public TopLevelCommentSnippet Snippet { get; set; }
    }

    public partial class TopLevelCommentSnippet
    {
        [JsonProperty("authorDisplayName")]
        public string AuthorDisplayName { get; set; }

        [JsonProperty("authorProfileImageUrl")]
        public string AuthorProfileImageUrl { get; set; }

        [JsonProperty("authorChannelUrl")]
        public string AuthorChannelUrl { get; set; }

        [JsonProperty("authorChannelId")]
        public AuthorChannelId AuthorChannelId { get; set; }

        [JsonProperty("videoId")]
        public VideoId VideoId { get; set; }

        [JsonProperty("textDisplay")]
        public string TextDisplay { get; set; }

        [JsonProperty("textOriginal")]
        public string TextOriginal { get; set; }

        [JsonProperty("parentId", NullValueHandling = NullValueHandling.Ignore)]
        public string ParentId { get; set; }

        [JsonProperty("canRate")]
        public bool CanRate { get; set; }

        [JsonProperty("viewerRating")]
        public ViewerRating ViewerRating { get; set; }

        [JsonProperty("likeCount")]
        public long LikeCount { get; set; }

        [JsonProperty("publishedAt")]
        public DateTimeOffset PublishedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }
    }

    public partial class AuthorChannelId
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class ItemSnippet
    {
        [JsonProperty("videoId")]
        public VideoId VideoId { get; set; }

        [JsonProperty("topLevelComment")]
        public Comment TopLevelComment { get; set; }

        [JsonProperty("canReply")]
        public bool CanReply { get; set; }

        [JsonProperty("totalReplyCount")]
        public long TotalReplyCount { get; set; }

        [JsonProperty("isPublic")]
        public bool IsPublic { get; set; }
    }

    public partial class PageInfo
    {
        [JsonProperty("totalResults")]
        public long TotalResults { get; set; }

        [JsonProperty("resultsPerPage")]
        public long ResultsPerPage { get; set; }
    }

    public enum ItemKind { YoutubeCommentThread };

    public enum TopLevelCommentKind { YoutubeComment };

    public enum VideoId { WtLjPvx7Ys };

    public enum ViewerRating { None };

    public partial class YoutubeComments
    {
        public static YoutubeComments FromJson(string json) => JsonConvert.DeserializeObject<YoutubeComments>(json, CommentReview.Models.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this YoutubeComments self) => JsonConvert.SerializeObject(self, CommentReview.Models.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                ItemKindConverter.Singleton,
                TopLevelCommentKindConverter.Singleton,
                VideoIdConverter.Singleton,
                ViewerRatingConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ItemKindConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ItemKind) || t == typeof(ItemKind?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "youtube#commentThread")
            {
                return ItemKind.YoutubeCommentThread;
            }
            throw new Exception("Cannot unmarshal type ItemKind");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ItemKind)untypedValue;
            if (value == ItemKind.YoutubeCommentThread)
            {
                serializer.Serialize(writer, "youtube#commentThread");
                return;
            }
            throw new Exception("Cannot marshal type ItemKind");
        }

        public static readonly ItemKindConverter Singleton = new ItemKindConverter();
    }

    internal class TopLevelCommentKindConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TopLevelCommentKind) || t == typeof(TopLevelCommentKind?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "youtube#comment")
            {
                return TopLevelCommentKind.YoutubeComment;
            }
            throw new Exception("Cannot unmarshal type TopLevelCommentKind");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TopLevelCommentKind)untypedValue;
            if (value == TopLevelCommentKind.YoutubeComment)
            {
                serializer.Serialize(writer, "youtube#comment");
                return;
            }
            throw new Exception("Cannot marshal type TopLevelCommentKind");
        }

        public static readonly TopLevelCommentKindConverter Singleton = new TopLevelCommentKindConverter();
    }

    internal class VideoIdConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(VideoId) || t == typeof(VideoId?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "wtLJPvx7-ys")
            {
                return VideoId.WtLjPvx7Ys;
            }
            throw new Exception("Cannot unmarshal type VideoId");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (VideoId)untypedValue;
            if (value == VideoId.WtLjPvx7Ys)
            {
                serializer.Serialize(writer, "wtLJPvx7-ys");
                return;
            }
            throw new Exception("Cannot marshal type VideoId");
        }

        public static readonly VideoIdConverter Singleton = new VideoIdConverter();
    }

    internal class ViewerRatingConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ViewerRating) || t == typeof(ViewerRating?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "none")
            {
                return ViewerRating.None;
            }
            throw new Exception("Cannot unmarshal type ViewerRating");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ViewerRating)untypedValue;
            if (value == ViewerRating.None)
            {
                serializer.Serialize(writer, "none");
                return;
            }
            throw new Exception("Cannot marshal type ViewerRating");
        }

        public static readonly ViewerRatingConverter Singleton = new ViewerRatingConverter();
    }
}
