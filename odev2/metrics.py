import math

def calculate_loc(lines):
    count = 0
    for line in lines:
        stripped = line.strip()
        if stripped != "" and not stripped.startswith("#"):
            count += 1
    return count


def calculate_complexity(lines):
    keywords = ['if ', 'elif', 'for ', 'while', 'except', ' and ', ' or ']
    complexity = 1

    for line in lines:
        for key in keywords:
            complexity += line.count(key)

    return complexity


def calculate_halstead(lines):
    operators = [
        'if', 'elif', 'for', 'while', 'return', 'except',
        '+', '-', '*', '/', '=', '==', '>', '<', 'and', 'or'
    ]

    unique_ops = set()
    unique_operands = set()

    total_ops = 0
    total_operands = 0

    text = "".join(lines)

    for ch in "()[],:":
        text = text.replace(ch, " ")

    words = text.split()

    for w in words:
        if w in operators:
            unique_ops.add(w)
            total_ops += 1
        else:
            unique_operands.add(w)
            total_operands += 1

    n = len(unique_ops) + len(unique_operands)
    N = total_ops + total_operands

    if n == 0:
        return 0

    volume = N * math.log2(n)
    return int(volume)


def calculate_mi(loc, cc, volume):
    if loc <= 0 or volume <= 0:
        return 0

    mi = 171 - 5.2 * math.log(volume) - 0.23 * cc - 16.2 * math.log(loc)
    return round(mi, 1)