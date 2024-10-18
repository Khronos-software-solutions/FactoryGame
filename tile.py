import pygame as pg
from random import randint

class Tile:
    def __init__(self, pos: tuple[int,int], size: tuple[int,int] | None = (50,50),texture: pg.Surface = pg.Surface((50,50))) -> None:
        self.x = pos[0]
        self.y = pos[1]
        
        self.size = size
        self.texture: pg.Surface = texture
        
    def draw(self, surface: pg.Surface):
        surface.blit(self.texture, (self.x, self.y))
        

class Grid:
    """Creates a grid with a set size and scale
    """    
    def __init__(self, size: tuple[int, int], scale: int = 50, is_world: bool = False):
        """Initialize a Grid

        Args:
            size (tuple[x, y]): The size of the grid in cells
            scale (int): Size of grid cells in pixels, default is 50px
            is_world (bool): If the Grid should use world generation
        """        
        self.size = size
        self.scale = scale
        self.is_world = is_world
        self.surface: pg.Surface = pg.Surface((self.size[0] * self.scale, self.size[1] * self.scale))
        
        # color map for worlds
        self.map: list[list[tuple[int,int,int]]] = [[]]
        
        
        self.tiles: dict[str, Tile] = {}
        
        if self.is_world:
            for x in range(self.size[0]):
                self.map.append([])
                for y in range(self.size[1]):
                    self.map[x].append((randint(40,70),randint(70,90),randint(0,10)))
        else:
            for x in range(self.size[0]):
                self.map.append([])
                for y in range(self.size[1]):
                    print((x+y) % 2 == 0)
                    if (x + y) % 2 == 0:
                        self.map[x].append((100,100,100))
                    else:
                        self.map[x].append((200,200,200))

        print(self.map)
        
        for y in range(self.size[1]):
            for x in range(self.size[0]):
                rect = pg.Rect(x * self.scale, y * self.scale, self.scale, self.scale)
                
                self.surface.fill(self.map[x][y],rect)
            
    def place(self, tile: Tile, uid: str):
        self.tiles[uid] = tile
        
    def update(self):

        for y in range(self.size[1]):
            for x in range(self.size[0]):
                rect = pg.Rect(x * self.scale, y * self.scale, self.scale, self.scale)
                
                self.surface.fill(self.map[x][y],rect)

        for item in self.tiles:
            self.tiles[item].draw(self.surface)
    


if __name__ == "__main__":
    print('This is a module and therefore cannot be executed.')
    print('You can use this module by importing it into another script using `import tile`')