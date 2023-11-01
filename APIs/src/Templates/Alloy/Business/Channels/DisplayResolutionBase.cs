using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Web;

namespace AlloyTemplates.Business.Channels
{
    /// <summary>
    /// Base class for all resolution definitions
    /// </summary>
    public abstract class DisplayResolutionBase : IDisplayResolution
    {
        private readonly LocalizationService _localizationService;
        private readonly string _name;

        protected DisplayResolutionBase(LocalizationService localizationService, string name, int width, int height)
        {
            _localizationService = localizationService;
            _name = name;
            Id = GetType().FullName;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets the unique ID for this resolution
        /// </summary>
        public string Id { get; protected set; }

        /// <summary>
        /// Gets the name of resolution
        /// </summary>
        public string Name => Translate(_name);

        /// <summary>
        /// Gets the resolution width in pixels
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// Gets the resolution height in pixels
        /// </summary>
        public int Height { get; protected set; }

        private string Translate(string resurceKey)
        {
            string value;

            if (!_localizationService.TryGetString(resurceKey, out value))
            {
                value = resurceKey;
            }

            return value;
        }
    }
}
