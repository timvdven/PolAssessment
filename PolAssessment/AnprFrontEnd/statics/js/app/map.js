import { createMarker, fitMapToMarkers } from "./utils.js";

export const init = (evt, globalSettings) => {
    initMap(evt, globalSettings);
};

const initMap = (evt, globalSettings) => {
    document.getElementById("map-data-totaal").innerText = globalSettings.anpr.length;

    // Initialize and add the map
    let map;

    async function initGoogleMap() {
         const position = { lat: 52.086922, lng: 5.111845 };
         const { Map } = await google.maps.importLibrary("maps");

         map = new Map(document.getElementById("the-map"), {
             zoom: 9,
             center: position,
             mapId: "DEMO_MAP_ID",
         });

         globalSettings.anprMap = map;
         globalSettings.anpr.forEach((anpr) => {createMarker(map, anpr, false);});
         const latLngArray = globalSettings.anpr.map(x => ({ lat: x.latitude, lng: x.longitude }));
         fitMapToMarkers(map, latLngArray);
     }

    initGoogleMap().then(() => {
        // console.log('Google Maps loaded');
    });
};
