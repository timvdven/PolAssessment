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
}

export const isInteger = (str) => {
    // Probeer de string te converteren naar een integer
    const parsed = parseInt(str, 10);
    
    // Controleer of de geconverteerde waarde een getal is en of de oorspronkelijke string geen extra tekens bevat
    return !isNaN(parsed) && parsed.toString() === str;
}
