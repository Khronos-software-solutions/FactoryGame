"""
This is a module with functions too general to be categorized in seperate files
"""

import numpy as np

def perlin(
        generator: np.random.Generator, shape: tuple[int, int], res: tuple[int, int], tileable: tuple[bool, bool] = (False, False)):
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


def distance(p1: tuple[int,int], p2: tuple[int,int]):
    return np.sqrt((p1[0] - p2[0])**2 + (p1[1] - p2[1])**2)

def draw_rectangle(x, y, width: int, height: int, rotation: int=0) -> tuple[int,int]:
    points = []
    radius = np.sqrt((height / 2)**2 + (width / 2)**2)
    angle = np.arctan2(height / 2, width / 2)
    rot_radians = (np.pi / 180) * rotation
    for angle in [angle, -angle + np.pi, angle + np.pi, -angle]:
        y_offset = -1 * radius * np.sin(angle + rot_radians)
        x_offset = radius * np.cos(angle + rot_radians)
        points.append((x + x_offset, y + y_offset))
    return points 
