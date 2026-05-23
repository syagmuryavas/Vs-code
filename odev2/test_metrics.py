import unittest
from metrics import calculate_loc, calculate_complexity


class TestMetrics(unittest.TestCase):

    def test_simple_code(self):
        code = [
            "a = 5\n",
            "b = 10\n",
            "print(a + b)\n"
        ]
        self.assertEqual(calculate_loc(code), 3)

    def test_if_code(self):
        code = [
            "if a > b:\n",
            "    print(a)\n"
        ]
        self.assertEqual(calculate_complexity(code), 2)

    def test_loop_code(self):
        code = [
            "for i in range(5):\n",
            "    print(i)\n"
        ]
        self.assertEqual(calculate_complexity(code), 2)


if __name__ == "__main__":
    print("TESTLER BASLIYOR")
    unittest.main()