import os

import pygame as pg
import yaml

from logger import Logger

console = Logger('./config/general.yml')

class SoundHandler:
    def __init__(self) -> None:
        pg.mixer.init(48000, channels=2, buffer=256)
        
        self.sfx: dict[str, pg.mixer.Sound] = {}
    
    def loadSounds(self):
        with open('.\\assets\\sound\\sound_index.yml', 'r') as f:
            s: dict[str, list[dict[str,str]]] = yaml.safe_load(f)
        
        print(s)
        for item in s['sounds']:
            try:
                self.sfx.update({item['name']: pg.mixer.Sound(item['path'])})
            except FileNotFoundError:
                console.warn(f'Something went wrong while loading sound "{item['name']}": File at path "{os.path.abspath(item['path'])}" does not exist.')
    
    def playSound(self, name: str):
        if name in self.sfx:
            self.sfx[name].play()
        else:
            console.warn(f'Error: "{name}" may not be a valid sound or may not be loaded.')
