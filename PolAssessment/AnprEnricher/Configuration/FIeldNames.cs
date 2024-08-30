namespace PolAssessment.AnprEnricher.Configuration;

internal static class FieldNames
{
    internal class HotFolder
    {
        public const string TgzPath = "HotFolder:TgzPath";
        public const string DataPath = "HotFolder:DataPath";
    }
    
    public const string MaxRetriesForReadingFile = "MaxRetriesForReadingFile";
    public const string TimeOutForRetry = "TimeOutForRetry";

    internal static class VehicleEnricher
    {
        public const string BaseUrl = "VehicleEnricher:BaseUrl";
        public const string Query = "VehicleEnricher:Query";
    }

    internal static class LocationEnricher
    {
        public const string BaseUrl = "LocationEnricher:BaseUrl";
        public const string ApiKey = "LocationEnricher:ApiKey";
    }

    internal static class AnprDataProcessor
    {
        public const string BaseUrl = "AnprDataProcessor:BaseUrl";
        internal static class Operation
        {
            public const string Anpr = "AnprDataProcessor:Operation:Anpr";
            public const string Authorize = "AnprDataProcessor:Operation:Authorize";
        }
        public const string ClientId = "AnprDataProcessor:ClientId";
        public const string ClientSecret = "AnprDataProcessor:ClientSecret";
    }
}
