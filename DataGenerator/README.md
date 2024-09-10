# Generate some test data for POL Assessment

[Main README](../README.md)
[Architecture README](../DOCS/ARCHITECTURE/README.md)
[ANPR JSON format README](../DOCS/ARCHITECTURE/ANPR_JSON_FORMAT.md)

## Description
These scripts and csv files will help to generate some test data for the POL Assessments solution. The csv files contains of one thousand random locations around max. 100 km away from Utrecht. Unfortunately, not all location resolve to an actual place which can be represented by a street and city combination. The license plates csv contains of one thousand recently added (31st of August 2024) license plates.

## Prerequisites
You need to have a recent version of Python 3 installed.

## How to

Execute the following Python script if you want to generate 50 anpr json files in the directory `jsons`:
```bash
python generate_random_anpr_json_files.py 50 jsons
```

Execute the following in order to pack those files into a tar.gz file, creating the file `jsons_archive.tar.gz`:
```bash
tar -czf jsons_archive.tar.gz -C jsons .
```

Finally you can use this generated file and copy-paste it into the hot folder mentioned in the POL Assessments solution of which the main README.md file can be found [here](../PolAssessment/README.md).
