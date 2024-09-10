# Assessment

[Main README](../README.md)

## ANPR Data Processing:

A system is needed that enriches and stores incoming ANPR data so that users can view it via a web interface.

Build a system that:
- Picks up new ANPR files from a folder. Each file contains one or more files compressed into a gzipped tar. Each file in the tar contains one ANPR record.
- Properties of ANPR records are:
  - License plate (with dashes between letters and numbers, e.g., HV-100-H)
  - GPS position as a latitude and longitude in Dutch decimal notation
  - Date and time
- Reads the contents of these files, enriches them, and uploads them to the API
- Enrichments are:
  - License plate to trade name, brand, and APK expiration date of the vehicle (https://data.overheid.nl/dataset/basisregistratie-voertuigen—rdw#panel-resources)
  - GPS position to street/road (https://www.geoapify.com/reverse-geocoding-api/)

Build an API that:
- Receives information and stores it in a database.
- Authorizes via a key whether the uploader is allowed to upload.
- Logs who uploaded what.

Build a web service (backend + frontend) that:
- Allows only authorized users through username and password.
- The user can select a license plate and date range. The corresponding ANPR records are then displayed on a map (https://developers.google.com/maps/documentation/javascript/adding-a-google-map). RDW information of the license plate is also shown.
- When new records for the selected license plate come in, the user will see them in the frontend.

Conditions:
- Everything must be able to run in a test environment using Docker containers.
- (Optional) A functional design.
- We want tests added wherever possible.
- Code must be uploaded to GitHub so that we can also see it. Please include as many commits as possible with descriptions of what is in the commit (this will give us insight into the thought process and changes made along the way).
- Provide your own test data. Keep this limited due to restrictions in the used online API.
- All components must be clearly documented (readme.md):
  - How to install
  - How to configure (including examples)
  - Minimum requirements
