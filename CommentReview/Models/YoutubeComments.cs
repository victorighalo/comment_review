﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CommentReview.Models
{
    [JsonObject]
    public partial class YoutubeComments
    {
        [JsonProperty("nextPageToken")]
        public string NextPageToken { get; set; }

        [JsonProperty("pageInfo")]
        public PageInfo PageInfo { get; set; }

        [JsonProperty("items")]
        public Item[] Items { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("snippet")]
        public ItemSnippet Snippet { get; set; }
    }

    public partial class ItemSnippet
    {
        [JsonProperty("videoId")]
        public VideoId VideoId { get; set; }

        [JsonProperty("topLevelComment")]
        public TopLevelComment TopLevelComment { get; set; }
    }

    public partial class TopLevelComment
    {
        [JsonProperty("snippet")]
        public TopLevelCommentSnippet Snippet { get; set; }
    }

    public partial class TopLevelCommentSnippet
    {
        [JsonProperty("authorDisplayName")]
        public string AuthorDisplayName { get; set; }

        [JsonProperty("videoId")]
        public VideoId VideoId { get; set; }

        [JsonProperty("textDisplay")]
        public string TextDisplay { get; set; }

        [JsonProperty("textOriginal")]
        public string TextOriginal { get; set; }

        [JsonProperty("viewerRating")]
        public ViewerRating ViewerRating { get; set; }

        [JsonProperty("publishedAt")]
        public DateTimeOffset PublishedAt { get; set; }
    }

    public partial class PageInfo
    {
        [JsonProperty("totalResults")]
        public long TotalResults { get; set; }

        [JsonProperty("resultsPerPage")]
        public long ResultsPerPage { get; set; }
    }

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
                VideoIdConverter.Singleton,
                ViewerRatingConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
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
