import os
import pygame as pg

from vector import iVector2 as vec2

from tile import Tile
from util import distance, quadratic_bezier


def load_images(path: str) -> list[pg.Surface]:
    """
    Loads all images in directory. The directory must only contain images.

    Args:
        path (str): The relative or absolute path to the directory to load images from.

    Returns:
        list[pygame.Surface]
    """
    images: list[pg.Surface] = []
    for file_name in os.listdir(path):
        image = pg.image.load(path + os.sep + file_name).convert()
        images.append(image)
    return images

class Sprite:
    def __init__(self, path: str, pos: tuple[int, int]) -> None:
        """Creates a surface

        Args:
            path (str): path to image
            size (tuple[int, int]): size of the sprite in pixels, should be the the same size as the image.
            updatespeed (int): amount of frames between different sprite frames
        """

        self.image = pg.image.load(path).convert_alpha()
        self.rect = self.image.get_rect()
        
        self.animation_time = 0.1
        self.current_time = 0
        self.index = 0

    def collides_with(self, other_rect: pg.Rect) -> bool:
        return self.rect.colliderect(other_rect)

class Belt:
    def __init__(self, start_pos: tuple[float, float], end_pos: tuple[float, float]):
        self.start_pos = start_pos
        self.end_pos = end_pos

    def draw(self, surface: pg.Surface):
        pg.draw.line(surface, (0,0,0), self.start_pos, self.end_pos, 20)
        
class Cable:
    def __init__(self, start_pos: tuple[float, float], end_pos: tuple[float, float]):
        self.start_pos = start_pos
        self.end_pos = end_pos
        self.points = quadratic_bezier(start_pos, (end_pos[0] - start_pos[0], start_pos[1] + 50), end_pos)
    
    def draw(self, surface: pg.Surface):
        pg.draw.lines(surface, (0,0,0), False, self.points, 2)

class Machine:
    """Base class for objects that have (an) input(s) and/or output(s)
    """
    def __init__(self, pos: tuple[int,int], size: tuple[int,int], sprite: Sprite):
        self.pos = pos
        self.size = size
        self.sprite = sprite
        