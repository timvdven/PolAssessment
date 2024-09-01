from sys import argv
from hashlib import sha256

def get_hash(input:str) -> str:
    return sha256(input.encode()).hexdigest()

if __name__ == '__main__':
    if len(argv) == 1:
        print("Usage: python hash_sha256.py <input>")
        print("Example: python hash_sha256.py MyPa$$w0rd")
        exit(1)

    print(get_hash(argv[1]))
