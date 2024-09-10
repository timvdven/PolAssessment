const { AdvancedMarkerElement, PinElement } = await google.maps.importLibrary("marker");

export const parseQueryStringToDictionary = (queryString) => {
	var dictionary = {};
	
	// remove the '?' from the beginning of the
	// if it exists
	if (queryString.indexOf('?') === 0) {
		queryString = queryString.substr(1);
	}
	
	// Step 1: separate out each key/value pair
	var parts = queryString.split('&amp;');
	
	for(var i = 0; i < parts.length; i++) {
		var p = parts[i];
		// Step 2: Split Key/Value pair
		var keyValuePair = p.split('=');
		
		// Step 3: Add Key/Value pair to Dictionary object
		var key = keyValuePair[0];
		var value = keyValuePair[1];
		
		// decode URI encoded string
		value = decodeURIComponent(value);
		value = value.replace(/\+/g, ' ');
		if (isInteger(value)) {
			value = parseInt(value, 10);
		}

		dictionary[key] = value;
	}
	
	// Step 4: Return Dictionary Object
	return dictionary;
};

export const isInteger = (str) => {
    // Probeer de string te converteren naar een integer
    const parsed = parseInt(str, 10);
    
    // Controleer of de geconverteerde waarde een getal is en of de oorspronkelijke string geen extra tekens bevat
    return !isNaN(parsed) && parsed.toString() === str;
};

export const hide = (element) => {
	element?.classList?.add('hidden');
};

export const show = (element) => {
	element?.classList?.remove('hidden');
};

export const createMarker = (map, anpr, isAfterAutoRefresh) => {
	if (!anpr || anpr.length === 0) {
		return;
	}

	const position = { lat: anpr.latitude, lng: anpr.longitude };
	const title = anpr.licensePlate;

	const infoWindow = new google.maps.InfoWindow({
		content: getInfoWindowContent(anpr),
	});

	const marker = new AdvancedMarkerElement({
		map: map,
		position: position,
		title: title,
		content: isAfterAutoRefresh ? getAfterRefreshPinElement().element : null,
	});
	marker.infoWindowIsOpen = false;
	anpr.marker = marker;

	google.maps.event.addListener(marker, 'click', function () {
		if (marker.infoWindowIsOpen) {
			infoWindow.close();
			marker.infoWindowIsOpen = false;
			return;
		}
		infoWindow.open(map, marker);
		marker.infoWindowIsOpen = true;
	});
};

const getAfterRefreshPinElement = () => {
	return new PinElement({
        background: '#FFFF00',
		borderColor: 'black',
		glyph: '!',
		glyphColor: 'black',
    });
};

const getInfoWindowContent = (anpr) => {
    return `
        <div>
            <h3>${anpr.licensePlate}</h3>
            <strong>${anpr.vehicleBrandName}: ${anpr.vehicleTechnicalName}</strong>
            <p>Gezien op: <em>${formatDateTime(anpr.exactDateTime)}</em></p>
			<p>APK vervaldatum: <em>${formatDateTime(anpr.vehicleApkExpirationDate, true)}</em></p>
            <p>${anpr.locationStreet}<br>${anpr.locationCity}</p>
        </div>
    `;
};

const formatDateTime = (dateTimeString, dateOnly) => {
    const date = new Date(dateTimeString);
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are zero-based
    const year = date.getFullYear();
	if (dateOnly) {
		return `${day}-${month}-${year}`;
	}

    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');

    return `${day}-${month}-${year} ${hours}:${minutes}:${seconds}`;
};

export const fitMapToMarkers = (map, latLngArray) => {
	var bounds = new google.maps.LatLngBounds();

	latLngArray.forEach((latLng) => {
		bounds.extend(latLng);
	});

	map.fitBounds(bounds);
};
