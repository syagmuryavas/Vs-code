from metrics import (
    calculate_loc,
    calculate_complexity,
    calculate_halstead,
    calculate_mi
)


def main():
    file_name = input("Dosya adini gir: ")

    try:
        with open(file_name, "r", encoding="utf-8") as f:
            lines = f.readlines()

        loc = calculate_loc(lines)
        cc = calculate_complexity(lines)
        volume = calculate_halstead(lines)
        mi = calculate_mi(loc, cc, volume)

        print(f"LOC : {loc}")
        print(f"Cyclomatic Complexity : {cc}")
        print(f"Halstead Volume : {volume}")
        print(f"Maintainability Index : {mi}")

    except FileNotFoundError:
        print("Dosya bulunamadi!")


if __name__ == "__main__":
    main()