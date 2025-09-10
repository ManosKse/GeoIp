namespace GeoIpProject.Api.Controllers
{
    /// <summary>
    /// Names of the actions in the controller
    /// </summary>
    public static class ActionNames
    {
        /// <summary>
        /// GetIpInfo endpoint
        /// </summary>
        public const string GetIpInfo = "{ip}";

        /// <summary>
        /// GetBatchesProcessingStatus endpoint
        /// </summary>
        public const string GetBatchesProcessingStatus = "{id}";

        /// <summary>
        /// CreateBatch endpoint
        /// </summary>
        public const string CreateBatch = "createBatch";
    }
}
