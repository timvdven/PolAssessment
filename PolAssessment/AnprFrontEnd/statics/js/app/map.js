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
         const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");

         map = new Map(document.getElementById("the-map"), {
             zoom: 9,
             center: position,
             mapId: "DEMO_MAP_ID",
         });

         globalSettings.anpr.forEach((anpr) => {
            const position = { lat: anpr.latitude, lng: anpr.longitude };
            const title = anpr.licensePlate;

            const infoWindow = new google.maps.InfoWindow({
                content: getInfoWindowContent(anpr),
            });

            const marker = new AdvancedMarkerElement({
                map: map,
                position: position,
                title: title,
            });
            anpr.marker = marker;

            google.maps.event.addListener(marker, 'click', function () {
                infoWindow.open(map, marker);
            });
         });
     }

    initGoogleMap().then(() => {
        // console.log('Google Maps loaded');
    });
};

const getInfoWindowContent = (anpr) => {
    return `
        <div>
            <h3>${anpr.licensePlate}</h3>
            <strong>${anpr.vehicleBrandName}: ${anpr.vehicleTechnicalName}</strong>
            <p><em>${formatDateTime(anpr.exactDateTime)}</em></p>
            <p>${anpr.locationStreet}<br>${anpr.locationCity}</p>
        </div>
    `;
};

const formatDateTime = (dateTimeString) => {
    const date = new Date(dateTimeString);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are zero-based
    const year = date.getFullYear();
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');

    return `${day}-${month}-${year} ${hours}:${minutes}:${seconds}`;
};