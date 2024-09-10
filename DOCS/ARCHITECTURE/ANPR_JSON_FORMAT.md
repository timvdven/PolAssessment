# ANPR JSON format

[Main README](../../README.md)
[Architecture README](./README.md)
[Data Generator README](../../DataGenerator/README.md)


The proper JSON format used for reading ANPR data is:

```json
{
    "Plate": "X-123-XX",
    "Coordinates": {
        "Latitude": 52.0246,
        "Longitude": 4.1750
    },
    "DateTime": "2024-08-01T12:00:00"
}
```

In which:
- "Plate": The license plate with dashes;
- "Coordiantes": An object containing:
  - "Latitude": A double representing the latitude value;
  - "Longitude": A double representing the longitude value;
- "DateTime": A date in the following string format: "YYYY-MM-DDTHH:mm:ss"
