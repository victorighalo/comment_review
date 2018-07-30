using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CommentReview.Models.YoutubeResponseModel
{

    public partial class YoutubeResponseModel
    {
        [JsonProperty("snippet")]
        public YoutubeResponseModelSnippet Snippet { get; set; }
    }

    public partial class YoutubeResponseModelSnippet
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

    public enum VideoId { IublAxAm8Hq };

    public enum ViewerRating { None };

    public partial class YoutubeResponseModel
    {
        public static YoutubeResponseModel[] FromJson(string json) => JsonConvert.DeserializeObject<YoutubeResponseModel[]>(json, CommentReview.Models.YoutubeResponseModel.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this YoutubeResponseModel[] self) => JsonConvert.SerializeObject(self, CommentReview.Models.YoutubeResponseModel.Converter.Settings);
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
            if (value == "iublAXAm8HQ")
            {
                return VideoId.IublAxAm8Hq;
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
            if (value == VideoId.IublAxAm8Hq)
            {
                serializer.Serialize(writer, "iublAXAm8HQ");
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
