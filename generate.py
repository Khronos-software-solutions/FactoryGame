import numpy as np

from util import perlin

class Generator:
    def __init__(self, seed: int | None) -> None:
        self.seed = seed
        self.generator = np.random.Generator(np.random.PCG64(seed))
        self.generator.random()        
    
class GroundMap:
    def __init__(self, size: tuple[int, int]) -> None:
        pass