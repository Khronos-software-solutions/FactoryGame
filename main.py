import sys
from logger import Logger

import pygame as pg

import tile
import machine

from camera import CameraInstance
from sound import SoundHandler
from util import loadYML, isIntersecting

pg.init()

config = loadYML('./config/general.yml')
loglevel = config['loglevel']

FONT = pg.font.SysFont('Sans Serif', 30)

SCREEN_WIDTH = 800
SCREEN_HEIGHT = 600

WORLD_WIDTH = 1600
WORLD_HEIGHT = 1600

GRID_SIZE = 50

PLAYER_SIZE = 25
PLAYER_SPEED = 2

UI_GRID_SIZE = 50
UI_GRID_ROWS = 5
UI_GRID_COLS = 5
UI_GRID_COLOR = (200, 200, 200)
UI_BACKGROUND_COLOR = (150, 150, 150)

WHITE = (255, 255, 255)
BLACK = (0, 0, 0)

pg.display.set_caption("Game")

console = Logger(configpath='./config/general.yml')

sounds: SoundHandler = SoundHandler()
sounds.loadSounds()

world_grid = tile.Grid((32,32), 50, False)
camera = CameraInstance((800, 600), (0,0))

screen = camera.screen
world = world_grid.surface

class Player:
    def __init__(self, x: int, y: int):

        self.x: float = x
        self.y: float = y
        self.vel_x = 0
        self.vel_y = 0

        self.max_vel = 20
        
        self.decel = 0.2

        self.inventory = { 'iron_ore': 2 }
        self.rect = pg.Rect(self.x, self.y, PLAYER_SIZE, PLAYER_SIZE)

    def move(self, vel_x: float, vel_y: float):
        if self.vel_x <= self.max_vel and self.vel_x >= -self.max_vel:
                self.vel_x += vel_x
        else:
            console.debug('you\'re going too fast!')
            
        if self.vel_y <= self.max_vel and self.vel_y >= -self.max_vel:
            self.vel_y += vel_y
        else:
            console.debug('you\'re going too fast!')
            
        
        new_x = self.x + self.vel_x
        new_y = self.y + self.vel_y

        if 0 <= new_x <= WORLD_WIDTH - PLAYER_SIZE:
            self.x = new_x
        else:
            self.x = new_x -self.vel_x
            
        if 0 <= new_y <= WORLD_HEIGHT - PLAYER_SIZE:
            self.y = new_y
        else:
            self.y = new_y -self.vel_y 
            
        self.vel_x -= self.vel_x * self.decel
        self.vel_y -= self.vel_y * self.decel
        
    
        self.rect.topleft = (int(self.x), int(self.y))
    
    def draw(self, surface: pg.Surface):
        pg.draw.rect(surface, BLACK, self.rect)

class Machine:
    def __init__(self, x: float, y: float, sx: float, sy: float, color: pg.Color) -> None:
        self.x = x
        self.y = y
        self.size = (sx, sy)
        self.color = color
        self.rect = pg.Rect(self.x, self.y, sx, sy)
        self.selected: bool = False

    def draw(self, surface: pg.Surface):
        pg.draw.rect(surface, self.color, self.rect)

    def collides_with(self, other_rect: pg.Rect):
        return self.rect.colliderect(other_rect)

class Miner(Machine):
    def __init__(self, x: int, y: int):
        super().__init__(x, y, 50, 50, pg.Color(200,50,50))

    def place(self, sound: SoundHandler):
        sound.playSound('place-small')

    def update(self):
        if self.selected:
            self.color = pg.Color(50,50,200)
        else:
            self.color = pg.Color(200,50,50)

class UI:
    def __init__(self, player: Player):
        self.visible = False
        self.player = player

    def toggle(self):
        self.visible = not self.visible

    def draw(self, surface: pg.Surface):
        if not self.visible:
            return

        ui_background = pg.Rect(SCREEN_WIDTH // 2 - UI_GRID_SIZE * UI_GRID_COLS // 2,
                                    SCREEN_HEIGHT // 2 - UI_GRID_SIZE * UI_GRID_ROWS // 2,
                                    UI_GRID_SIZE * UI_GRID_COLS,
                                    UI_GRID_SIZE * UI_GRID_ROWS)
        pg.draw.rect(surface, UI_BACKGROUND_COLOR, ui_background)

        for row in range(UI_GRID_ROWS):
            for col in range(UI_GRID_COLS):
                grid_rect = pg.Rect(SCREEN_WIDTH // 2 - UI_GRID_SIZE * UI_GRID_COLS // 2 + col * UI_GRID_SIZE,
                                       SCREEN_HEIGHT // 2 - UI_GRID_SIZE * UI_GRID_ROWS // 2 + row * UI_GRID_SIZE,
                                       UI_GRID_SIZE,
                                       UI_GRID_SIZE)
                pg.draw.rect(surface, UI_GRID_COLOR, grid_rect, 1)

def grid(pos: float | int):
    return int(pos // GRID_SIZE) * GRID_SIZE

player = Player(WORLD_WIDTH // 2, WORLD_HEIGHT // 2)

ui = UI(player)

objects: dict[str, Miner] = {}

belts: list[machine.Cable] = []

running = True
clock = pg.time.Clock()

interactPressed = False

test = tile.Tile((200, 200), texture=pg.image.load(".\\assets\\placeholder.png").convert_alpha())

world_grid.place(test, 'test')

selectedBuilding = 1
previousSelectedBuilding = 1

while running:
    mouse = None
    for event in pg.event.get():
        if event.type == pg.QUIT:
            running = False
        elif event.type == pg.KEYDOWN:
            k = event.key
            if event.key == pg.K_ESCAPE:
                running = False
            if event.key == pg.K_e:
                interactPressed = True
            if k == pg.K_1:
                selectedBuilding = 1
            elif k == pg.K_2:
                selectedBuilding = 2
            elif k == pg.K_3:
                selectedBuilding = 3
            elif k == pg.K_4:
                selectedBuilding = 4
            elif k == pg.K_5:
                selectedBuilding = 5
            elif k == pg.K_6:
                selectedBuilding = 6
            console.debug(f'selectedBuilding is {selectedBuilding}')
        elif event.type == pg.MOUSEBUTTONDOWN:
            mouse = event
            console.debug(str(mouse))

    
    keys = pg.key.get_pressed()
    dx, dy = 0, 0
    if keys[pg.K_LEFT] or keys[pg.K_a]:
        dx = -PLAYER_SPEED
    if keys[pg.K_RIGHT] or keys[pg.K_d]:
        dx = PLAYER_SPEED
    if keys[pg.K_UP] or keys[pg.K_w]:
        dy = -PLAYER_SPEED
    if keys[pg.K_DOWN] or keys[pg.K_s]:
        dy = PLAYER_SPEED
    
    player.move(dx, dy)

    if selectedBuilding != previousSelectedBuilding:
        sounds.playSound('change-building')
        previousSelectedBuilding = selectedBuilding

    world_grid.update()
    world = world_grid.surface

    cur = pg.Rect(grid(camera.pos[0] + pg.mouse.get_pos()[0] ),grid(camera.pos[1] + pg.mouse.get_pos()[1]),10,10)

    pg.draw.rect(world, BLACK, cur)

    can_place: bool = True

    if mouse:
        if getattr(mouse, 'button') == 1:
            for i in objects:
                if objects[i].collides_with(cur):
                    can_place = False
                    break
                else:
                    can_place = True
        
            if can_place:
                objects.update({f'miner_{len(objects)}': Miner(grid(camera.pos[0] + pg.mouse.get_pos()[0]),grid(camera.pos[1] + pg.mouse.get_pos()[1]))})
                sounds.playSound('build-small')
            else:
                console.info(f'Cannot place object "miner_{len(objects)}" at x:{grid(camera.pos[0] + pg.mouse.get_pos()[0] )}, y:{grid(camera.pos[1] + pg.mouse.get_pos()[1])}; Space is possibly occupied!')

    temp_objects = objects.copy()
    selectedObjects: list[str] = []
    for i in temp_objects:
        if objects[i].selected:
            selectedObjects.append(i)

        objects[i].draw(world)
        objects[i].update()
        if temp_objects[i].collides_with(cur):
            if pg.mouse.get_pressed()[2]:
                sounds.playSound('deconstruct-small')
                objects.pop(i, None)  
            elif interactPressed:
                if objects[i].selected:
                    objects[i].selected = False
                else:
                    objects[i].selected = True
                objects[i].update()
    del temp_objects
    
    temp_belts = belts.copy()
    for i in temp_belts:
        if isIntersecting(i.start_pos, i.end_pos, cur.topleft, (cur.topleft[0] + 50, cur.topleft[1] + 50)) or isIntersecting(i.start_pos, i.end_pos, (cur.topleft[0], cur.topleft[1] + 50), (cur.topleft[0], cur.topleft[1] + 50)):
            if pg.mouse.get_pressed()[2]:
                belts.remove(i)

    del temp_belts

    if len(selectedObjects) == 2:
        objects[selectedObjects[0]].selected = False
        objects[selectedObjects[1]].selected = False
        belts.append(machine.Cable((objects[selectedObjects[0]].x + (GRID_SIZE // 2), objects[selectedObjects[0]].y + (GRID_SIZE // 2)), (objects[selectedObjects[1]].x + (GRID_SIZE // 2), objects[selectedObjects[1]].y + (GRID_SIZE // 2))))
        sounds.playSound('belt-connect')
        selectedObjects = []
        
    for i in belts:
        i.draw(world)
    
    player.draw(world)
    camera.render(world, (player.x, player.y), None)
    
    ui.draw(screen)

    interactPressed = False
    pg.display.flip()
    
    clock.tick(60)

pg.quit()
sys.exit()

