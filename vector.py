from __future__ import annotations
from typing import Iterable

from math import sqrt, pi, sin, cos

class iVector2:
    x: int = 0
    y: int = 0
    def __init__(self, x: int, y: int):
        self.x = x
        self.y = y

    def length(self):
        return sqrt(self.x**2 + self.y**2)

    def rotate(self, angle: float):
        angle *= pi/180 # Convert to radians
        return fVector2(
            x = self.x * cos(angle) - self.y * sin(angle),
            y = self.x * sin(angle) + self.y * cos(angle)
        )
    
    def __iter__(self, other: object) -> Iterable[int]:
        for i in (self.x, self.y):
            yield i

    def __add__(self, other: object) -> iVector2:
        if isinstance(other, iVector2):
            return iVector2(self.x + other.x, self.y + other.y)
        elif isinstance(other, int):
            return iVector2(self.x + other, self.y + other)
        else:
            raise TypeError(f"unsupported operand type(s) for +: '{type(self)}' and '{type(other)}'")

    def __sub__(self, other: object) -> iVector2:
        if isinstance(other, iVector2):
            return iVector2(self.x - other.x, self.y - other.y)
        else:
            raise TypeError(f"unsupported operand type(s) for -: '{type(self)}' and '{type(other)}'")

    def __mul__(self, other: object) -> iVector2:
        if isinstance(other, iVector2):
            return iVector2(self.x * other.x, self.y * other.y)
        if isinstance(other, int):
            return iVector2(self.x * other, self.y * other)
        else:
            raise TypeError(f"unsupported operand type(s) for *: '{type(self)}' and '{type(other)}'")

class fVector2:
    x: float = 0
    y: float = 0
    def __init__(self, x: float, y: float) -> None:
        self.x = x
        self.y = y

    def length(self) -> float:
        return sqrt(self.x**2 + self.y**2)

    def rotate(self, angle: float) -> fVector2:
        angle *= pi/180 # Convert to radians
        return fVector2(
            x = self.x * cos(angle) - self.y * sin(angle),
            y = self.x * sin(angle) + self.y * cos(angle)
        )

    def __iter__(self, other: object) -> Iterable[float]:
        for i in (self.x, self.y):
            yield i

    def __add__(self, other: object) -> fVector2:
        if type(other) is fVector2:
            return fVector2(self.x + other.x, self.y + other.y)
        elif isinstance(other, int) or isinstance(other, float):
            return fVector2(self.x + other, self.y + other)
        else:
            raise TypeError(f"unsupported operand type(s) for +: '{type(self)}' and '{type(other)}'")

    def __sub__(self, other: object) -> fVector2:
        if isinstance(other, fVector2):
            return fVector2(self.x - other.x, self.y - other.y)
        else:
            raise TypeError(f"unsupported operand type(s) for -: '{type(self)}' and '{type(other)}'")

    def __mul__(self, other: object) -> fVector2:
        if isinstance(other, fVector2):
            return fVector2(self.x * other.x, self.y * other.y)
        if isinstance(other, int) or isinstance(other, float):
            return fVector2(self.x * other, self.y * other)
        else:
            raise TypeError(f"unsupported operand type(s) for *: '{type(self)}' and '{type(other)}'")
