<h1> HexWorld </h1>
<h3> This project is an implementation of an endless hex tile world generation system in Unity. It utilizes chunk loading/unloading system for efficient rendering and optimization, as well as a pathfinding algorithm for navigating the generated world using Unity's DOTS. The project aims to create vast and procedurally generated hex tile-based environments.</h3> *(Below images are hex chunks which formed by hex tiles) <hr>

# Seed & Frequency
<h2> Using the seed value, you can generate different variations of the world. <br> <br> Tweaking the noise frequency allows you to control the level of detail and the overall visual appearance of the generated world. </h2>
<img src="https://github.com/dev-hasanolgun/HexWorld/blob/main/Gifs/Seed.gif" width="400" height="400" align="left"/> 
<img src="https://github.com/dev-hasanolgun/HexWorld/blob/main/Gifs/Frequency.gif" width="400" height="400" align="left"/> <br clear="left"/> <small> *(single hex chunk of size 100) </small> <hr>
<h1> Chunk Loading / Unloading & Distance </h1>
<h2> By adjusting the chunk load distance, you can balance performance and visual quality, ensuring that only relevant chunks are loaded based on the player's position and chunks outside this distance are unloaded. </h2>
<img src="https://github.com/dev-hasanolgun/HexWorld/blob/main/Gifs/LoadDistance.gif" width="266" height="266" align="left"/>
<img src="https://github.com/dev-hasanolgun/HexWorld/blob/main/Gifs/ChunkLoading.gif" width="266" height="266" align="left"/>
<img src="https://github.com/dev-hasanolgun/HexWorld/blob/main/Gifs/ChunkUnloading.gif" width="266" height="266" align="left"/> <br clear="left"/> <small> *(chunk size 5 with load distance 3 & unload distance 5) </small> <hr>
<h1> Obstacle Placement </h1>
<h2> Utilizing the FloodFill algorithm, you can dynamically and procedurally generate realistic and varied obstacle layouts as well as using a simple noise map for obstacle placement. </h2>
<img src="https://github.com/dev-hasanolgun/HexWorld/blob/main/Gifs/ObstacleFloodFill.gif" width="400" height="400" align="left"/>
<img src="https://github.com/dev-hasanolgun/HexWorld/blob/main/Gifs/ObstacleNoiseBasedFill.gif" width="400" height="400" align="left"/> <br clear="left"/> <small> *(chunk size 20 with load distance 2) </small> <hr>
<h1> Pathfinding </h1>
<h2> The combination of A* and Flowfield pathfinding techniques provides an efficient and dynamic solution for navigating entities in a hex tile world. </h2>
<img src="https://github.com/dev-hasanolgun/HexWorld/blob/main/Gifs/Pathfinding1.gif" width="400" height="400" align="left"/>
<img src="https://github.com/dev-hasanolgun/HexWorld/blob/main/Gifs/Pathfinding2.gif" width="400" height="400" align="left"/> <br clear="left"/>

