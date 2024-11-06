import pygame as pg

from logger import Logger

console = Logger('./config/general.yml')

class CameraInstance:
    def __init__(self, size: tuple[int, int], pos: tuple[float, float], show_fps: bool = False):
        self.size: tuple[int, int] = size
        self.pos: tuple[float, float] = pos
        self.show_fps: bool = show_fps
        self.screen = pg.display.set_mode(size)
        self.FONT = pg.font.SysFont('Sans Serif', 30)


    def render(self, world: pg.Surface, focus: tuple[float, float], clock: pg.time.Clock | None):
        """Renders world to screen while making sure to not show out of bounds.

        Args:
            world (pygame.Surface): The world to render. Must have a set size (height and width).
            focus (tuple[float, float]): The focus point of the camera, for example a player's location.
            clock (pygame.time.Clock): clock to show frames per second. If `show_fps` is false or clock is not given, the FPS counter will not be shown
        """

        # 'Clear' screen
        self.screen.fill((0,0,0))

        self.pos = (focus[0] - self.size[0] // 2, focus[1] - self.size[1] // 2)

        # check world bounds
        if self.pos[0] < 0:
            self.pos = (0 , self.pos[1])
        if self.pos[0] > world.get_width() - self.size[0]:
            self.pos =  (world.get_width() - self.size[0], self.pos[1])
        if self.pos[1] < 0:
            self.pos = (self.pos[0], 0)
        if self.pos[1] > world.get_height() - self.size[1]:
            self.pos = (self.pos[0], world.get_height() - self.size[1])

        self.screen.blit(world, (0, 0), (self.pos[0], self.pos[1], self.size[0], self.size[1]))

        if self.show_fps and clock != None:
            self.screen.blit(self.FONT.render(str(round(clock.get_fps(), 2)), False, pg.Color(0,0,0)), (0,0))

if __name__ == "__main__":
    console.info('This is a module and therefore cannot be executed.')
    console.info('You can use this module by importing it into another script using `import camera`')