import numpy as np
from pygame import Surface

from util import perlin

class Generator:
    def __init__(self, seed: int | None) -> None:
        self.seed = seed
        self.generator = np.random.Generator(np.random.PCG64(seed))
        self.perlin_map = perlin(self.generator, (256, 256), (8, 8))
        
    
class GroundMap:
    def __init__(self, size: tuple[int, int]) -> None:
        self.surface = Surface(size)