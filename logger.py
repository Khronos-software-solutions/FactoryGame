import sys, os

from util import loadYML, writeline, getCallerName
from datetime import datetime

class color:
    DEBUG = '\033[38;5;7m'
    INFO = '\033[38;5;231m'
    WARNING = '\033[38;5;226m'
    ERROR = '\033[38;5;196m'
    END = '\033[0m'

class Logger:
    def __init__(self, configpath: str, logfile: str | None = None):
        f = loadYML(configpath)['loglevel']
        if f not in ['debug', 'info', 'warn', 'error', 'silent']:
            sys.stdout.write(f'{configpath}: loglevel not valid, falling back to "warn"\n')
            self.loglevel = 2
        else: self.loglevel = ['debug', 'info', 'warn', 'error', 'silent'].index(f)
        
        # If no logfile path is given, make one unless loglevel is 4 (silent).
        if logfile == None and not self.loglevel == 4:
            if not os.path.exists('./log'):
                os.mkdir('./log')
            self.logfile = f'{os.curdir}/log/{datetime.now().strftime('%d%m%Y_%H%M%S')}.log'
        elif not logfile == None and not self.loglevel == 4: self.logfile = logfile
        else: self.logfile = os.devnull # Write to /dev/null if silent


    def debug(self, str: str):
        if self.loglevel == 0:
            time = datetime.now().strftime('%H:%M:%S.%f')[:-3]
            writeline(self.logfile ,f'[DEBUG] @ {time} from {getCallerName()} : {str}\n')
            sys.stdout.write(f'{color.DEBUG} [DEBUG] @ {time} from {getCallerName()} : {str}{color.END}\n')
            sys.stdout.flush()
    
    def info(self, str: str):
        if self.loglevel <= 1:
            time = datetime.now().strftime('%H:%M:%S.%f')[:-3]
            writeline(self.logfile ,f'[INFO] @ {time} from {getCallerName()} : {str}\n')
            sys.stdout.write(f'{color.INFO} [INFO] @ {time} from {getCallerName()} : {str}{color.END}\n')
            sys.stdout.flush()

    def warn(self, str: str):
        if self.loglevel <= 2:
            time = datetime.now().strftime('%H:%M:%S.%f')[:-3]
            writeline(self.logfile ,f'[WARN] @ {time} from {getCallerName()} : {str}\n')
            sys.stdout.write(f'{color.WARNING} [WARN] @ {time} from {getCallerName()} : {str}{color.END}\n')
            sys.stdout.flush()

    def error(self, str: str):
        if self.loglevel <= 3:
            time = datetime.now().strftime('%H:%M:%S.%f')[:-3]
            writeline(self.logfile ,f'[ERROR] @ {time} from {getCallerName()} : {str}\n')
            sys.stdout.write(f'{color.ERROR} [ERROR] @ {time} from {getCallerName()} : {str}{color.END}\n')
            sys.stdout.flush()
