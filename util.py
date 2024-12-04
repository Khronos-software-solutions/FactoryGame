"""
This is a module with functions too general to be categorized in seperate files
"""
import yaml

import numpy as np

from inspect import stack as stk

def perlin(generator: np.random.Generator, shape: tuple[int, int], res: tuple[int, int], tileable: tuple[bool, bool] = (False, False)):
    """Generate a 2D numpy array of perlin noise.

    Args:
        shape: The shape of the generated array (tuple of two ints).
            Must be a multple of res.
        res: The number of periods of noise to generate along each
            axis (tuple of two ints). Note shape must be a multiple of
            res.
        tileable: If the noise should be tileable along each axis
            (tuple of two bools). Defaults to (False, False).

    Returns:
        A numpy array of shape shape with the generated noise.
    """
    delta = (res[0] / shape[0], res[1] / shape[1])
    d = (shape[0] // res[0], shape[1] // res[1])
    grid = np.mgrid[0:res[0]:delta[0], 0:res[1]:delta[1]]\
             .transpose(1, 2, 0) % 1

    # Gradients
    angles = 2*np.pi*generator.random((res[0]+1, res[1]+1))
    gradients = np.dstack((np.cos(angles), np.sin(angles)))

    if tileable[0]:
        gradients[-1,:] = gradients[0,:]
    if tileable[1]:
        gradients[:,-1] = gradients[:,0]

    gradients = gradients.repeat(d[0], 0).repeat(d[1], 1)

    g00 = gradients[    :-d[0],    :-d[1]]
    g10 = gradients[d[0]:     ,    :-d[1]]
    g01 = gradients[    :-d[0],d[1]:     ]
    g11 = gradients[d[0]:     ,d[1]:     ]

    # Ramps
    n00 = np.sum(np.dstack((grid[:,:,0]  , grid[:,:,1]  )) * g00, 2)
    n10 = np.sum(np.dstack((grid[:,:,0]-1, grid[:,:,1]  )) * g10, 2)
    n01 = np.sum(np.dstack((grid[:,:,0]  , grid[:,:,1]-1)) * g01, 2)
    n11 = np.sum(np.dstack((grid[:,:,0]-1, grid[:,:,1]-1)) * g11, 2)

    # Interpolation
    t = grid**3*(grid*(grid*6 - 15) + 10)
    n0 = n00*(1-t[:,:,0]) + t[:,:,0]*n10
    n1 = n01*(1-t[:,:,0]) + t[:,:,0]*n11

    return np.sqrt(2)*((1-t[:,:,1])*n0 + t[:,:,1]*n1)

def isIntersecting(a: tuple[float, float], b: tuple[float, float], c: tuple[float, float], d: tuple[float, float]) -> bool:
    """Checks whether lines ab and cd are intersecting

    Args:
        a (tuple[float, float]): point 1 of line 1
        b (tuple[float, float]): point 2 of line 1
        c (tuple[float, float]): point 1 of line 2
        d (tuple[float, float]): point 2 of line 2

    Returns:
        bool: whether the lines intersect
    """
    dn = ((b[0] - a[0]) * (d[1] - c[1])) - ((b[1] - a[1]) * (d[0] - c[0]))
    n1  = ((a[1] - c[1]) * (d[0] - c[0])) - ((a[0] - c[0]) * (d[1] - c[1]))
    n2  = ((a[1] - c[1]) * (b[0] - a[0])) - ((a[0] - c[0]) * (b[1] - a[1]))
    
    # If the lines coincide, this will not work, but I'm too lazy to handle this edge case.
    if (dn == 0): 
        return n1 == 0 and n2 == 0
    
    r = n1 / dn
    s = n2 / dn
    
    return (r >= 0 and r <= 1) and (s >= 0 and s <= 1)

def quadratic_bezier(start: tuple[float, float], control: tuple[float, float], end: tuple[float, float], steps: int = 100) -> list[tuple[float, float]]:
    points: list[tuple[float, float]] = []
    for t in range(steps + 1):
        t /= steps
        x = (1 - t)**2 * start[0] + 2 * (1 - t) * t * control[0] + t**2 * end[0]
        y = (1 - t)**2 * start[1] + 2 * (1 - t) * t * control[1] + t**2 * end[1]
        points.append((x, y))
    return points

def distance(p1: tuple[float, float], p2: tuple[float, float]) -> float:
    return np.sqrt((p1[0] - p2[0])**2 + (p1[1] - p2[1])**2)

def hex_to_rgb(value: str) -> tuple[int, ...]: 
    value = value.lstrip('#')
    lv = len(value)
    return tuple(int(value[i:i + lv // 3], 16) for i in range(0, lv, lv // 3))

def loadYML(path: str) -> dict[str, object]:
    with open(path) as f:
        yml = f.read()
    return yaml.safe_load(yml)

def writeline(path: str, value: str) -> None:
    """Appends a string of text to a file

    Args:
        path (str): Path to the file
        value (str): String to be appended
    """
    with open(path, 'a') as f:
        f.write(value)

def getCallerName():
    return stk()[2].frame.f_globals['__name__']
