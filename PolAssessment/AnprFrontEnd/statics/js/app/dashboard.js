import { updateToken } from './localStorage.js';
import { hide, show, createMarker, fitMapToMarkers } from './utils.js';

export const init = (evt, globalSettings) => {
    refreshToken_if_necessary(globalSettings);
    initDashboard(evt, globalSettings);
};

const initDashboard = (evt, globalSettings) => {
    document.getElementById("logout-button").addEventListener('click', (event) => {
        updateToken(null);
        window.location.reload();
    });

    const form = evt.target.querySelector('form');
    const submitButton = form.querySelector('button[type="submit"]');
    const spinner = form.querySelector('.spinner-border');
    form.addEventListener('submit', (event) => {
        event.preventDefault();
        hide(submitButton);
        show(spinner);

        refreshToken_if_necessary(globalSettings);

        const formData = new FormData(form);

        const plate = formData.get('plate');
        const startDate = formData.get('startDate');
        const endDate = formData.get('endDate');

        filter(globalSettings, spinner, submitButton, plate, startDate, endDate);
    });

    setInterval(() => {
        handleRealTimeUpdate(form, globalSettings);
    }, 10000);
};

const handleRealTimeUpdate = (form, globalSettings) => {
    const target = document.getElementById('real-time-update');
    if (!target || !target.checked) {
        return;
    }
    const formData = new FormData(form);

    const plate = formData.get('plate');
    const startDate = formData.get('startDate');
    const endDate = formData.get('endDate');

    const token = globalSettings.token;
    const { exp } = jwtDecode(token);
    const now = Math.floor(Date.now() / 1000);
    const diff = exp - now;
    if (diff < 0) {
        updateToken(null);
        window.location.reload();
    }
    document.getElementById("jwt-exp-in").innerHTML = Math.round(diff/60);
    const spinner = form.querySelector('.spinner-border');
    filter(globalSettings, spinner, null, plate, startDate, endDate, null, globalSettings.anprLastFetchDate);
};

const filter = (globalSettings, spinner, submitButton, plate, startDate, endDate, hash, minimumUploadDate) => {
    var filters = {};
    if (plate) {
        filters.plate = plate;
    }

    if (startDate) {
        filters.startDate = startDate;
    }

    if (endDate) {
        filters.endDate = endDate;
    }

    if (hash) {
        filters.hash = hash;
    }

    if (minimumUploadDate) {
        filters.minimumUploadDate = minimumUploadDate;
    }

    const queryParams = new URLSearchParams(filters).toString();

    fetch(`${webapi_url}/anpr?${queryParams}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${globalSettings.token}`
        },
    }).then(response => {
        if (response.ok) {
            return response.json();
        }
        throw new Error('Network response was not ok.');
    }).then(data => {
        handleAnpr(globalSettings, data, hash || minimumUploadDate);
    }).catch((error) => {
        console.error('Error:', error);
    }).finally(() => {
        hide(spinner);
        show(submitButton);
    });
};

const handleAnpr = (globalSettings, data, isAutoRefresh) => {
    globalSettings.anpr = isAutoRefresh ? globalSettings.anpr.concat(data.result) : data.result;
    globalSettings.anprHash = data.hash;
    globalSettings.anprLastFetchDate = data.lastFetchDate;

    if (isAutoRefresh) {
        if (data.result.length === 0) {
            return;
        }

        show(document.getElementById('just-added'));
        var justAddedElement = document.getElementById('just-added-amount');

        // Do not refresh the map, simply add these
        data.result.forEach((anpr) => {
            createMarker(globalSettings.anprMap, anpr, true);
            updateAutoUpdateData(justAddedElement);
        });
        const latLngArray = globalSettings.anpr.map(x => ({ lat: x.latitude, lng: x.longitude }));
        fitMapToMarkers(globalSettings.anprMap, latLngArray);
    } else {
        refreshMap();
    }
};

const updateAutoUpdateData = (amount) => {
    var amountValue = parseInt(amount.innerText);
    amount.innerText = amountValue + 1;
};

const refreshMap = () => {
    const theMap = document.getElementById("the-map");
    if (theMap) {
        theMap.remove();
    }
    htmx.ajax('GET', '/app/map/', {target:'#map-wrapper', swap:'innerHTML'});
};

const refreshToken_if_necessary = (globalSettings) => {
    const token = globalSettings.token;
    const { unique_name, exp } = jwtDecode(token);
    document.getElementById("username").innerHTML = unique_name;
    const now = Math.floor(Date.now() / 1000);
    const diff = exp - now;
    if (diff < 0) {
        // Token already expired
        globalSettings.token = null;
        updateToken(null);
        window.location.reload();
    }
    
    document.getElementById("jwt-exp-in").innerHTML = Math.round(diff/60);
    if (diff > 300) {
        // Token expires in more than 5 minutes
        // No need to refresh
        return;
    }

    console.log('Refreshing token');
    fetch(`${globalSettings.webapi_url}/refreshtoken`, {
        method: 'GET', headers: {
            "Authorization": `Bearer ${token}`
        }
    }).then(response => {
        if (response.ok) {
            return response.json();
        }
        throw new Error('Network response was not ok.');
    }).then(data => {
        globalSettings.token = data.token;
        updateToken(data.token);
    }).catch((error) => {
        console.error('Error:', error);
    });
};
