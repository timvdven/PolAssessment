import random
import string
import json
import generate_random_anpr_json as grj
from os import path, makedirs
from sys import argv

def generate_random_string(length: int=8) -> str:
    return ''.join(random.choices(string.ascii_lowercase, k=length))

def generate_random_filename() -> str:
    return f"{generate_random_string()}.json"

def save_random_anpr_json(output_dir: str, filename: str) -> None:
    if not path.exists(output_dir):
        makedirs(output_dir)
    filepath = path.join(output_dir, filename)
    with open(filepath, 'w') as file:
        dump = json.dumps(grj.get_random_anpr_json(), indent=4)
        file.write(dump)

def main(num_files: int=10, output_dir: str='jsons') -> None:
    for _ in range(num_files):
        filename = generate_random_filename()
        save_random_anpr_json(output_dir, filename)

if __name__ == "__main__":
    amount = 10 if len(argv) < 1 else int(argv[1])
    output_dir = 'jsons' if len(argv) < 2 else argv[2]
    main(amount, output_dir)
