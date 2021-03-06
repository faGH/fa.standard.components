﻿using FrostAura.Standard.Components.Razor.Abstractions;
using FrostAura.Standard.Components.Razor.Enums.GoogleMap;
using FrostAura.Standard.Components.Razor.Models.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace FrostAura.Standard.Components.Razor.Navigation
{
    /// <summary>
    /// Google map component.
    /// </summary>
    public partial class GoogleMap : BaseComponent<GoogleMap>
    {
        /// <summary>
        /// Center point for the map.
        /// </summary>
        [Parameter]
        public GeoPoint Center { get; set; }
        /// <summary>
        /// Default zoom of the map.
        /// </summary>
        [Parameter]
        public int Zoom { get; set; }
        /// <summary>
        /// Map type to render initially.
        /// </summary>
        [Parameter]
        public MapType MapType { get; set; }
        /// <summary>
        /// API key to use with the map service.
        /// </summary>
        [Parameter]
        public string ApiKey { get; set; }
        /// <summary>
        /// Event source for when the map is initialized.
        /// </summary>
        [Parameter]
        public EventCallback<GoogleMap> OnMapReady { get; set; }
        /// <summary>
        /// JS runtime engine.
        /// </summary>
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        /// <summary>
        /// Add a marker to the map.
        /// </summary>
        public async Task AddMarkerAsync(string title,
            string subtitle,
            double lat,
            double lng,
            int size = 25,
            string iconUrl = "https://wiredscore.com/wp-content/uploads/2017/02/pin-drop-transparent-white.png")
        {
            var request = new
            {
                title,
                icon = new
                {
                    url = iconUrl,
                    size
                },
                location = new
                {
                    lat,
                    lng
                },
                info = new
                {
                    content = @"
                        <div>
                            <div style='font-weight: bold'>" + title + @"</div>
                            <div>" + subtitle + @"</div>
                        </div>"
                }
            };

            await JsRuntime.InvokeVoidAsync("faGoogleMap.addMarker", Id, request);
        }

        /// <summary>
        /// Clear all markers from the map.
        /// </summary>
        public async Task ClearAllMarkersAsync()
        {
            await JsRuntime.InvokeVoidAsync("faGoogleMap.clearAllMarkers", Id);
        }

        /// <summary>
        /// Initialize the Google map component.
        /// </summary>
        private async Task InitializeMapAsync()
        {
            await JsRuntime.InvokeVoidAsync("faGoogleMap.initializeGoogleMapAsync", new
            {
                id = Id,
                center = new
                {
                    lat = Center?.Latitude,
                    lng = Center?.Longitude
                },
                zoom = Zoom,
                mapType = MapType.ToString().ToLower(),
                apiKey = ApiKey
            });
            await OnMapReady.InvokeAsync(this);
        }
    }
}
