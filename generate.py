import numpy as np

from util import perlin

class Generator:
    def __init__(self, seed: int = np.random.randint(-1000000,1000000)) -> None:
        self.seed = seed
        state = np.random.RandomState(seed)
    