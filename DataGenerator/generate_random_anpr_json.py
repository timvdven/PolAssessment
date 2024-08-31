import json
import random
import datetime
import csv

def get_json(plate:str, lat:float, lng:float, dt:datetime) -> dict:
    return {
        "Plate": plate,
        "Coordinates": {
            "Latitude": lat,
            "Longitude": lng
        },
        "DateTime": format_dt(dt)
    }

def add_dashes_to_plate(plate:str) -> str:
    groups = group_letters_and_digits(plate)
    return "-".join(groups)

def group_letters_and_digits(s: str) -> list:
    if not s:
        return []

    groups = []
    current_group = s[0]

    for char in s[1:]:
        if (char.isdigit() and current_group[-1].isdigit()) or (char.isalpha() and current_group[-1].isalpha()):
            current_group += char
        else:
            groups.append(current_group)
            current_group = char

    if (len(current_group) == 4):
        groups.append(current_group[:2])
        groups.append(current_group[2:])
    else:
        groups.append(current_group)
    return groups

def format_dt(dt:datetime) -> str:
    return dt.isoformat().replace('+00:00', 'Z')

def read_random_line_from_csv(file_path: str) -> list:
    with open(file_path, mode='r') as file:
        reader = csv.reader(file)

        skip_lines = random.randint(0, 999)
        for _ in range(skip_lines):
            next(reader, None)
        
        return next(reader, None)

def get_random_plate() -> str:
    random_plate = read_random_line_from_csv("random_plates.csv")[0]
    return add_dashes_to_plate(random_plate)

def get_random_location() -> dict:
    random_location = read_random_line_from_csv("random_locations.csv")[0]
    groups = random_location.split('\t')
    lat = float(groups[1])
    lng = float(groups[3])
    return { 'lat':lat, 'lng':lng }

def get_random_time_past_week() -> datetime.datetime:
    now = datetime.datetime.now()
    seconds_in_week = 7 * 24 * 60 * 60
    random_seconds = random.randint(0, seconds_in_week)
    random_past_time = now - datetime.timedelta(seconds=random_seconds)
    return random_past_time

def get_random_anpr_json() -> dict:
    plate = get_random_plate()
    lat, lng = get_random_location().values()
    dt = get_random_time_past_week()
    return get_json(plate, lat, lng, dt)

if __name__ == "__main__":
    print(json.dumps(get_random_anpr_json(), indent=4))
